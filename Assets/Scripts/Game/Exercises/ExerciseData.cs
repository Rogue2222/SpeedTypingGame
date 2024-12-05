using System;
using Newtonsoft.Json.Linq;

using SpeedTypingGame.Game.Persistence;

namespace SpeedTypingGame.Game.Exercises
{
    [Serializable]
    public class ExerciseData : IPersistable
    {
        // Fields 
        private long _timestamp;
        private double _accuracy;
        private double _wordsPerMinute;


        // Properties
        public DateTime Timestamp => DateTimeOffset.FromUnixTimeMilliseconds(_timestamp).DateTime.ToLocalTime();
        public double Accuracy => _accuracy;
        public double WordsPerMinute => _wordsPerMinute;


        // Methods
        public ExerciseData(Exercise exercise) : this(exercise.Accuracy, exercise.WordsPerMinute) { }

        public ExerciseData(double accuracy = 0, double wordsPerMinute = 0)
        {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _accuracy = accuracy;
            _wordsPerMinute = wordsPerMinute;
        }

        public JToken ToJSON()
        {
            return new JObject()
            {
                { "t", _timestamp },
                { "a", _accuracy },
                { "w", _wordsPerMinute },
            };
        }

        public void FromJSON(JToken json)
        {
            _timestamp = json["t"].Value<long>();
            _accuracy = json["a"].Value<double>();
            _wordsPerMinute = json["w"].Value<double>();
        }
    }
}