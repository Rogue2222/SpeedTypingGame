using System;
using Newtonsoft.Json.Linq;

using SpeedTypingGame.Game.Persistence;

namespace SpeedTypingGame.Game.Excercises
{
    [Serializable]
    public class ExcerciseData : IPersistable
    {
        // Fields 
        public long _timestamp;
        private float _duration;
        private int _length;
        private int _hits;
        private int _misses;


        // Properties
        public DateTime Timestamp => DateTimeOffset.FromUnixTimeMilliseconds(_timestamp).DateTime.ToLocalTime();
        public float Duration => _duration;
        public int Length => _length;
        public int Hits => _hits;
        public int Misses => _misses;


        // Methods
        public ExcerciseData(float duration, Excercise excercise) :
            this(duration, excercise.Length, excercise.Hits, excercise.Misses) { }

        public ExcerciseData(float duration = 0f, int length = 0, int hits = 0, int misses = 0)
        {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _duration = duration;
            _length = length;
            _hits = hits;
            _misses = misses;
        }

        public JToken ToJSON()
        {
            return new JObject()
            {
                { "t", _timestamp },
                { "d", _duration },
                { "l", _length },
                { "h", _hits },
                { "m", _misses }
            };
        }

        public void FromJSON(JToken json)
        {
            _timestamp = json["t"].Value<long>();
            _duration = json["d"].Value<float>();
            _length = json["l"].Value<int>();
            _hits = json["h"].Value<int>();
            _misses = json["m"].Value<int>();
        }
    }
}