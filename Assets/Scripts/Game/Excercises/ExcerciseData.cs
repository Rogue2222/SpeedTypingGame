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
        private int _correctCharacters;
        private int _incorrectCharacters;


        // Properties
        public long Timestamp => _timestamp;
        public DateTime DateTime => DateTimeOffset.FromUnixTimeMilliseconds(Timestamp).DateTime;
        public float Duration => _duration;
        public int CorrectCharacters => _correctCharacters;
        public float IncorrectCharacters => _incorrectCharacters;


        // Methods
        public ExcerciseData(float duration = 0f, int length = 0,
            int correctCharacters = 0, int incorrectCharacters = 0)
        {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _duration = duration;
            _length = length;
            _correctCharacters = correctCharacters;
            _incorrectCharacters = incorrectCharacters;
        }

        public JToken ToJSON()
        {
            return new JObject()
            {
                { "t", Timestamp },
                { "d", _duration },
                { "l", _length },
                { "c", _correctCharacters },
                { "i", _incorrectCharacters }
            };
        }

        public void FromJSON(JToken json)
        {
            _timestamp = json["t"].Value<long>();
            _duration = json["d"].Value<float>();
            _length = json["l"].Value<int>();
            _correctCharacters = json["c"].Value<int>();
            _incorrectCharacters = json["i"].Value<int>();
        }
    }
}