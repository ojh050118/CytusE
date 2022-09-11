using CytusE.Game.Configuration;
using CytusE.Game.Graphics;
using CytusE.Game.Levels;
using CytusE.Game.Overlays;
using CytusE.Resources;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;

namespace CytusE.Game
{
    public class CytusEGameBase : osu.Framework.Game
    {
        protected override Container<Drawable> Content { get; }

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected CytusEConfigManager LocalConfig { get; private set; }

        public bool IsDevelopmentBuild { get; }

        protected Storage Storage { get; set; }

        protected LevelStore LevelStore { get; set; }

        protected LevelManager LevelManager { get; set; }

        protected Bindable<Level> Level { get; set; }

        protected MusicController MusicController { get; set; }

        protected DummyLevel DummyLevel { get; set; }

        public string FrameworkVersion
        {
            get
            {
                var version = typeof(osu.Framework.Game).Assembly.GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        protected CytusEGameBase()
        {
            IsDevelopmentBuild = DebugUtils.IsDebugBuild;
            Name = $"cytus e! {(IsDevelopmentBuild ? "(Development build)" : string.Empty)}";

            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768)
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var files = Storage.GetStorageForDirectory("files");
            var export = Storage.GetStorageForDirectory("export");
            var largeStore = new LargeTextureStore(Host.Renderer, Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"Textures")));
            largeStore.AddTextureSource(Host.CreateTextureLoaderStore(new OnlineStore()));

            Resources.AddStore(new DllResourceStore(typeof(CytusEResources).Assembly));

            AddFont(Resources, @"Fonts/Electrolize");

            dependencies.CacheAs(largeStore);
            dependencies.CacheAs(LevelStore = new LevelStore(files, Host, Audio));
            dependencies.CacheAs(LevelManager = new LevelManager(export, LevelStore));
            dependencies.CacheAs(Storage);
            dependencies.CacheAs(LocalConfig);

            DummyLevel = new DummyLevel(Audio.Tracks, largeStore);
            dependencies.CacheAs(DummyLevel);
            dependencies.CacheAs<IBindable<Level>>(Level = new NonNullableBindable<Level>(DummyLevel));
            dependencies.CacheAs(Level);
            dependencies.CacheAs(new CytusEColour());
            dependencies.CacheAs(this);

            AddInternal(MusicController = new MusicController());
            dependencies.CacheAs(MusicController);
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage ??= Host.Storage;
            LocalConfig ??= new CytusEConfigManager(Storage);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            LocalConfig?.Dispose();
        }
    }
}
