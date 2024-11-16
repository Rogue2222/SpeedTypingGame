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
        public CharacterData(int hits = 0, int misses = 0)
        {
            _hits = hits;
            _misses = misses;
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

        public static CharacterData operator +(CharacterData characterData, int hits)
        {
            return hits >= 0 ?
                new(characterData.Hits + hits, characterData.Misses) :
                new(characterData.Hits, characterData.Misses + Math.Abs(hits));
        }

        public static CharacterData operator -(CharacterData characterData, int misses)
        {
            return misses >= 0 ?
                new(characterData.Hits, characterData.Misses + misses) :
                new(characterData.Hits + Math.Abs(misses), characterData.Misses);
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