using System.Collections.Generic;
using UnityEngine;

using SpeedTypingGame.Game.Excercises;

namespace SpeedTypingGame.Game.Persistence
{
    [AddComponentMenu("SpeedTypingGame/Game/")]
    public abstract class PersistenceHandler : MonoBehaviour
    {
        // Fields
        [SerializeField] protected GameManager _game;

        protected readonly Dictionary<char, CharacterData> _characterDataCollection = new();
        protected readonly List<ExcerciseData> _excerciseDataCollection = new();


        // Properties
        public int CharacterDataCount { get; }
        public int ExcerciseDataCount { get; }


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
            if (_characterDataCollection.ContainsKey(character))
            {
                _characterDataCollection[character].IncreaseCorrectTypings();
            }
            else
            {
                _characterDataCollection.Add(character, new(1, 0));
            }
        }

        public void AddIncorrectCharacter(char character)
        {
            if (_characterDataCollection.ContainsKey(character))
            {
                _characterDataCollection[character].IncreaseIncorrectTypings();
            }
            else
            {
                _characterDataCollection.Add(character, new(0, 1));
            }
        }

        public void AddExcerciseData(ExcerciseData excerciseData)
        {
            _excerciseDataCollection.Add(excerciseData);
        }

        public abstract void Save();

        public abstract void Load();

        public void Clear()
        {
            _characterDataCollection.Clear();
            _excerciseDataCollection.Clear();

            Save();
        }
    }
}