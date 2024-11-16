using System;
using Newtonsoft.Json.Linq;

using SpeedTypingGame.Game.Persistence;

namespace SpeedTypingGame.Game.Excercises
{
    [Serializable]
    public class CharacterData : IPersistable
    {
        // Fields
        private int _hits;
        private int _misses;


        // Properties
        public int Hits => _hits;
        public int Misses => _misses;
        public int Total => _hits + _misses;
        public float Accuracy => _hits / Total;
        

        // Methods
        public CharacterData(int correctTypings = 0, int incorrectTypings = 0)
        {
            _hits = correctTypings;
            _misses = incorrectTypings;
        }

        public void Clear()
        {
            _hits = 0;
            _misses = 0;
        }

        public void IncreaseCorrectTypings(int amount = 1)
        {
            _hits += amount;
        }

        public void IncreaseIncorrectTypings(int amount = 1)
        {
            _misses += amount;
        }

        public static CharacterData operator +(CharacterData characterData, int typings)
        {
            return typings >= 0 ?
                new(characterData.Hits + typings, characterData.Misses) :
                new(characterData.Hits, characterData.Misses + Math.Abs(typings));
        }

        public static CharacterData operator -(CharacterData characterData, int typings)
        {
            return typings >= 0 ?
                new(characterData.Hits, characterData.Misses + typings) :
                new(characterData.Hits + Math.Abs(typings), characterData.Misses);
        }

        public JToken ToJSON()
        {
            return new JObject()
            {
                { "h", _hits },
                { "m", _misses }
            };
        }

        public void FromJSON(JToken json)
        {
            _hits = json["h"].Value<int>();
            _misses = json["m"].Value<int>();
        }
    }
}