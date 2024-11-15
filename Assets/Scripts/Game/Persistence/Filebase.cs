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
        [SerializeField] private bool _shouldPrettyPrint;
#endif


        // Methods
        private void Awake()
        {
            _DirectoryPath = $"{Application.persistentDataPath}/Filebase";
            _FilePath = $"{_DirectoryPath}/save.json";
        }

        public override void Save()
        {
            AddCorrectCharacter('a');
            AddCorrectCharacter('a');
            AddCorrectCharacter('a');
            AddIncorrectCharacter('a');

            AddCorrectCharacter('b');
            AddIncorrectCharacter('b');
            AddCorrectCharacter('b');
            AddIncorrectCharacter('b');

            AddExcerciseData(new(24.56f, 48, 7));
            AddExcerciseData(new(27.23f, 50, 5));
            AddExcerciseData(new(21.78f, 46, 9));

            JObject saveData = new()
            {
                { "characterData", CharacterDataCollectionToJSON() },
                { "excerciseData", ExcerciseDataCollectionToJSON() }
            };

            if (!Directory.Exists(_DirectoryPath))
            {
                Directory.CreateDirectory(_DirectoryPath);
            }

            using StreamWriter streamWriter = new(_FilePath);
#if UNITY_EDITOR
            streamWriter.Write(saveData.ToString(
                _shouldPrettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None));
#else
            streamWriter.Write(saveData.ToString());
#endif
        }

        public override void Load()
        {
            if (!File.Exists(_FilePath)) return;

            using StreamReader streamReader = new(_FilePath);
            JObject saveData = JObject.Parse(streamReader.ReadToEnd());

            _characterDataCollection.Clear();
            JObject characterDataCollectionJSON = saveData["characterData"].Value<JObject>();
            foreach (KeyValuePair<string, JToken> characterDataPair in characterDataCollectionJSON)
            {
                CharacterData characterData = new();
                characterData.FromJSON(characterDataPair.Value);
                _characterDataCollection.Add(characterDataPair.Key[0], characterData);
            }

            _excerciseDataCollection.Clear();
            JArray excerciseDataCollectionJSON = saveData["excerciseData"].Value<JArray>();
            foreach (JToken excerciseDataJSON in excerciseDataCollectionJSON)
            {
                ExcerciseData excerciseData = new();
                excerciseData.FromJSON(excerciseDataJSON);
                _excerciseDataCollection.Add(excerciseData);
            }
        }

        private JObject CharacterDataCollectionToJSON()
        {
            JObject characterDataCollectionJSON = new();
            foreach (KeyValuePair<char, CharacterData> characterDataPair in _characterDataCollection)
            {
                characterDataCollectionJSON.Add(
                    new JProperty("" + characterDataPair.Key, characterDataPair.Value.ToJSON()));
            }

            return characterDataCollectionJSON;
        }

        private JArray ExcerciseDataCollectionToJSON()
        {
            JArray excerciseDataCollectionJSON = new();
            foreach (ExcerciseData excerciseData in _excerciseDataCollection)
            {
                excerciseDataCollectionJSON.Add(excerciseData.ToJSON());
            }

            return excerciseDataCollectionJSON;
        }
    }
}