using System.Collections.Generic;
using UnityEngine;

using SpeedTypingGame.Game.Excercises;

namespace SpeedTypingGame.Game.Persistence
{
    [AddComponentMenu("SpeedTypingGame/Game/")]
    public abstract class PersistenceHandler : MonoBehaviour
    {
        // Fields
        protected readonly Dictionary<char, CharacterData> _characterData = new();
        protected readonly List<ExcerciseData> _excerciseData = new();


        // Properties
        public int CharacterCount { get; }
        public int ExcerciseCount { get; }


        // Methods
        protected void Start()
        {
            Load();
        }

        protected void OnApplicationQuit()
        {
            Save();
        }

        public void AddCorrectCharacter(char character)
        {
            if (_characterData.ContainsKey(character))
            {
                _characterData[character].IncreaseCorrectTypings();
            }
            else
            {
                _characterData.Add(character, new(1, 0));
            }
        }

        public void AddIncorrectCharacter(char character)
        {
            if (_characterData.ContainsKey(character))
            {
                _characterData[character].IncreaseIncorrectTypings();
            }
            else
            {
                _characterData.Add(character, new(0, 1));
            }
        }

        public void AddExcercise(ExcerciseData excerciseData)
        {
            _excerciseData.Add(excerciseData);
        }

        public abstract void Save();

        public abstract void Load();

        public void Clear()
        {
            _characterData.Clear();
            _excerciseData.Clear();
        }
    }
}