using System;
using Newtonsoft.Json.Linq;

using SpeedTypingGame.Game.Persistence;

namespace SpeedTypingGame.Game.Excercises
{
    [Serializable]
    public class CharacterData : IPersistable
    {
        // Fields
        private int _correctTypings;
        private int _incorrectTypings;


        // Properties
        public int CorrectTypings => _correctTypings;
        public int IncorrectTypings => _incorrectTypings;
        public int TotalTypings => _correctTypings + _incorrectTypings;
        public float Accuracy => _correctTypings / TotalTypings;
        

        // Methods
        public CharacterData(int correctTypings = 0, int incorrectTypings = 0)
        {
            _correctTypings = correctTypings;
            _incorrectTypings = incorrectTypings;
        }

        public void Clear()
        {
            _correctTypings = 0;
            _incorrectTypings = 0;
        }

        public void IncreaseCorrectTypings(int amount = 1)
        {
            _correctTypings += amount;
        }

        public void IncreaseIncorrectTypings(int amount = 1)
        {
            _incorrectTypings += amount;
        }

        public static CharacterData operator +(CharacterData characterData, int typings)
        {
            return typings >= 0 ?
                new(characterData.CorrectTypings + typings, characterData.IncorrectTypings) :
                new(characterData.CorrectTypings, characterData.IncorrectTypings + typings);
        }

        public static CharacterData operator -(CharacterData characterData, int typings)
        {
            return typings >= 0 ?
                new(characterData.CorrectTypings, characterData.IncorrectTypings + typings) :
                new(characterData.CorrectTypings + typings, characterData.IncorrectTypings);
        }

        public JObject ToJSON()
        {
            return new()
            {
                {"c", CorrectTypings },
                {"i", IncorrectTypings }
            };
        }

        public void FromJSON(JObject json)
        {
            _correctTypings = json["c"].Value<int>();
            _incorrectTypings = json["i"].Value<int>();
        }
    }
}