namespace CytusE.Game.Gameplay.Objects.Types
{
    public interface IHasDuration
    {
        double EndTime { get; }

        double Duration { get; set; }
    }
}
