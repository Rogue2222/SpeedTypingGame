using System;

namespace SpeedTypingGame.Game.Excercises
{
    public record ExcerciseData
    {
        // Fields 
        public readonly float Duration;
        public readonly int CorrectKeys;
        public readonly int WrongKeys;
        private long _dateTimeOffset;


        // Properties
        public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(_dateTimeOffset).DateTime;


        // Methods
        public ExcerciseData(float duration, int correctKeys, int wrongKeys)
        {
            Duration = duration;
            CorrectKeys = correctKeys;
            WrongKeys = wrongKeys;
            _dateTimeOffset = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}