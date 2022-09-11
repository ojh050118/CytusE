using CytusE.Game.Gameplay.Scoring;

namespace CytusE.Game.Gameplay.Judgements
{
    public class Judgement
    {
        /// <summary>
        /// The score awarded for a small bonus.
        /// </summary>
        public const int SMALL_BONUS_SCORE = 10;

        /// <summary>
        /// The score awarded for a large bonus.
        /// </summary>
        public const int LARGE_BONUS_SCORE = 50;

        /// <summary>
        /// The default health increase for a maximum judgement, as a proportion of total health.
        /// By default, each maximum judgement restores 5% of total health.
        /// </summary>
        protected const double DEFAULT_MAX_HEALTH_INCREASE = 0.05;

        /// <summary>
        /// The maximum <see cref="HitResult"/> that can be achieved.
        /// </summary>
        public virtual HitResult MaxResult => HitResult.TPPerfect;

        /// <summary>
        /// The minimum <see cref="HitResult"/> that can be achieved - the inverse of <see cref="MaxResult"/>.
        /// </summary>
        public HitResult MinResult => HitResult.Miss;

        /// <summary>
        /// The numeric score representation for the maximum achievable result.
        /// </summary>
        public int MaxNumericResult => ToNumericResult(MaxResult);

        /// <summary>
        /// The health increase for the maximum achievable result.
        /// </summary>
        public double MaxHealthIncrease => HealthIncreaseFor(MaxResult);

        /// <summary>
        /// Retrieves the numeric score representation of a <see cref="JudgementResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to find the numeric score representation for.</param>
        /// <returns>The numeric score representation of <paramref name="result"/>.</returns>
        public int NumericResultFor(JudgementResult result) => ToNumericResult(result.Type);

        /// <summary>
        /// Retrieves the numeric health increase of a <see cref="HitResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="HitResult"/> to find the numeric health increase for.</param>
        /// <returns>The numeric health increase of <paramref name="result"/>.</returns>
        protected virtual double HealthIncreaseFor(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;

                case HitResult.Miss:
                    return -DEFAULT_MAX_HEALTH_INCREASE * 2;

                case HitResult.Bad:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.05;

                case HitResult.Good:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.75;

                case HitResult.Perfect:
                    return DEFAULT_MAX_HEALTH_INCREASE;

                case HitResult.TPPerfect:
                    return DEFAULT_MAX_HEALTH_INCREASE * 1.05;
            }
        }

        /// <summary>
        /// Retrieves the numeric health increase of a <see cref="JudgementResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to find the numeric health increase for.</param>
        /// <returns>The numeric health increase of <paramref name="result"/>.</returns>
        public double HealthIncreaseFor(JudgementResult result) => HealthIncreaseFor(result.Type);

        public override string ToString() => $"MaxResult:{MaxResult} MaxScore:{MaxNumericResult}";

        public static int ToNumericResult(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;

                case HitResult.Bad:
                    return 50;

                case HitResult.Good:
                    return 100;

                case HitResult.Perfect:
                    return 300;

                case HitResult.TPPerfect:
                    return 315;
            }
        }
    }
}
