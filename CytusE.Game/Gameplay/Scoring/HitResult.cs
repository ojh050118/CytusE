using System;
using System.Diagnostics;
using System.Linq;

namespace CytusE.Game.Gameplay.Scoring
{
    public enum HitResult
    {
        None,
        Miss,
        Bad,
        Good,
        Perfect,
        TPPerfect
    }

    public static class HitResultExtensions
    {
        /// <summary>
        /// Whether a <see cref="HitResult"/> increases the combo.
        /// </summary>
        public static bool IncreasesCombo(this HitResult result)
            => AffectsCombo(result) && IsHit(result);

        /// <summary>
        /// Whether a <see cref="HitResult"/> breaks the combo and resets it back to zero.
        /// </summary>
        public static bool BreaksCombo(this HitResult result)
            => AffectsCombo(result) && !IsHit(result);

        /// <summary>
        /// Whether a <see cref="HitResult"/> increases/breaks the combo, and affects the combo portion of the score.
        /// </summary>
        public static bool AffectsCombo(this HitResult result)
        {
            switch (result)
            {
                case HitResult.Miss:
                case HitResult.Bad:
                case HitResult.Good:
                case HitResult.Perfect:
                case HitResult.TPPerfect:
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Whether a <see cref="HitResult"/> affects the accuracy portion of the score.
        /// </summary>
        public static bool AffectsAccuracy(this HitResult result)
            => IsScorable(result) && !IsBonus(result);

        /// <summary>
        /// Whether a <see cref="HitResult"/> is a non-tick and non-bonus result.
        /// </summary>
        public static bool IsBasic(this HitResult result)
            => IsScorable(result) && !IsTick(result) && !IsBonus(result);

        /// <summary>
        /// Whether a <see cref="HitResult"/> should be counted as a tick.
        /// </summary>
        public static bool IsTick(this HitResult result) => false;

        /// <summary>
        /// Whether a <see cref="HitResult"/> should be counted as bonus score.
        /// </summary>
        public static bool IsBonus(this HitResult result) => false;

        /// <summary>
        /// Whether a <see cref="HitResult"/> represents a successful hit.
        /// </summary>
        public static bool IsHit(this HitResult result)
        {
            switch (result)
            {
                case HitResult.None:
                case HitResult.Miss:
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Whether a <see cref="HitResult"/> is scorable.
        /// </summary>
        public static bool IsScorable(this HitResult result) => result >= HitResult.Miss && result < HitResult.TPPerfect;

        /// <summary>
        /// An array of all scorable <see cref="HitResult"/>s.
        /// </summary>
        public static readonly HitResult[] ALL_TYPES = ((HitResult[])Enum.GetValues(typeof(HitResult))).ToArray();

        /// <summary>
        /// Whether a <see cref="HitResult"/> is valid within a given <see cref="HitResult"/> range.
        /// </summary>
        /// <param name="result">The <see cref="HitResult"/> to check.</param>
        /// <param name="minResult">The minimum <see cref="HitResult"/>.</param>
        /// <param name="maxResult">The maximum <see cref="HitResult"/>.</param>
        /// <returns>Whether <see cref="HitResult"/> falls between <paramref name="minResult"/> and <paramref name="maxResult"/>.</returns>
        public static bool IsValidHitResult(this HitResult result, HitResult minResult, HitResult maxResult)
        {
            if (result == HitResult.None)
                return false;

            if (result == minResult || result == maxResult)
                return true;

            Debug.Assert(minResult <= maxResult);
            return result > minResult && result < maxResult;
        }
    }
}
