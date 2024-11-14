using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

using SpeedTypingGame.Game.Excercises;

namespace SpeedTypingGame.Game.Persistence
{
    [AddComponentMenu("SpeedTypingGame/Game/Persistence/Filebase")]
    public class Filebase : PersistenceHandler
    {
        // Fields
        private static string _DirectoryPath;
        private static string _FilePath;

#if UNITY_EDITOR
        [SerializeField] private bool _prettyPrint;
#endif


        // Methods
        private void Awake()
        {
            _DirectoryPath = $"{Application.persistentDataPath}/Filebase";
            _FilePath = $"{_DirectoryPath}/save.json";

            AddCorrectCharacter('a');
            AddCorrectCharacter('a');
            AddCorrectCharacter('a');
            AddIncorrectCharacter('a');

            AddCorrectCharacter('b');
            AddIncorrectCharacter('b');
            AddCorrectCharacter('b');
            AddIncorrectCharacter('b');

            AddExcercise(new(24.56f, 48, 7));
            AddExcercise(new(27.23f, 50, 5));
            AddExcercise(new(21.78f, 46, 9));
        }

        public override void Save()
        {
            JObject saveData = new()
            {
                { "characterData", CharacterDataToJSON() },
                { "excerciseData", ExcerciseDataToJSON() }
            };

            if (!Directory.Exists(_DirectoryPath))
            {
                Directory.CreateDirectory(_DirectoryPath);
            }

            using StreamWriter streamWriter = new(_FilePath);
#if UNITY_EDITOR
            streamWriter.Write(saveData.ToString(
                _prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None));
#else
            streamWriter.Write(saveData.ToString());
#endif
        }

        public override void Load()
        {
            
        }

        private JObject CharacterDataToJSON()
        {
            JObject characterDataJSON = new();
            foreach (KeyValuePair<char, CharacterData> pair in _characterData)
            {
                characterDataJSON.Add(new JProperty("" + pair.Key, pair.Value.ToJSON()));
            }

            return characterDataJSON;
        }

        private JArray ExcerciseDataToJSON()
        {
            JArray excerciseDataJSON = new();
            foreach (ExcerciseData excerciseData in _excerciseData)
            {
                excerciseDataJSON.Add(excerciseData.ToJSON());
            }

            return excerciseDataJSON;
        }
    }
}