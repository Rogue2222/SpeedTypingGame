using System;
using Newtonsoft.Json.Linq;
using SpeedTypingGame.Game.Persistence;

namespace SpeedTypingGame.Game.Excercises
{
    [Serializable]
    public record ExcerciseData : IPersistable
    {
        // Fields 
        public long _timestamp;
        public float _duration;
        public int _correctCharacters;
        public int _incorrectCharacters;


        // Properties
        public long Timestamp => _timestamp;
        public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
        public float Duration => _duration;
        public int CorrectCharacters => _correctCharacters;
        public float IncorrectCharacters => _incorrectCharacters;


        // Methods
        public ExcerciseData(float duration, int correctCharacters, int incorrectCharacters)
        {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _duration = duration;
            _correctCharacters = correctCharacters;
            _incorrectCharacters = incorrectCharacters;
        }

        public JObject ToJSON()
        {
            return new()
            {
                {"t", Timestamp },
                {"d", _duration },
                {"c", _correctCharacters },
                {"i", _incorrectCharacters }
            };
        }

        public void FromJSON(JObject json)
        {
            _timestamp = json["t"].Value<long>();
            _duration = json["d"].Value<float>();
            _correctCharacters = json["c"].Value<int>();
            _incorrectCharacters = json["i"].Value<int>();
        }
    }
}