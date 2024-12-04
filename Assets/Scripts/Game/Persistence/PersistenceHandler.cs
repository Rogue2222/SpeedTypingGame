using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

using SpeedTypingGame.Game.Exercises;

namespace SpeedTypingGame.Game.Persistence
{
    /// <summary>
    /// Handles persisting a player's important statistical data between sessions.
    /// </summary>
    [AddComponentMenu("SpeedTypingGame/Game/Persistence/Persistence handler")]
    public class PersistenceHandler : MonoBehaviour
    {
        // Fields
        private const string _DebugGroup = "PERSISTENCE";
        private static string _FilePath;
        private static readonly Dictionary<char, CharacterData> _CharacterDataCollection = new();
        private static JObject _CharacterDataCollectionJSON = new();
        private static readonly List<ExerciseData> _ExerciseDataCollection = new();
        private static JArray _ExerciseDataCollectionJSON = new();

        [SerializeField] private GameManager _game;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldIndent;
#endif


        // Properties
        public int CharacterDataCount => _CharacterDataCollection.Count;
        public CharacterData this[char character] => _CharacterDataCollection[character];
        public List<char> Characters => new(_CharacterDataCollection.Keys);
        public List<Tuple<char, CharacterData>> CharacterData =>
            _CharacterDataCollection.Select(characterDataPair =>
            new Tuple<char, CharacterData>(characterDataPair.Key, characterDataPair.Value)).ToList();
        public int ExerciseDataCount => _ExerciseDataCollection.Count;
        public ExerciseData this[int index] => _ExerciseDataCollection[index];
        public List<ExerciseData> ExerciseData => new(_ExerciseDataCollection);
        public List<DateTime> Timestamps =>
            _ExerciseDataCollection.Select(exerciseData => exerciseData.Timestamp).ToList();
        public List<double> Accuracy =>
            _ExerciseDataCollection.Select(exerciseData => exerciseData.Accuracy).ToList();
        public List<double> WordsPerMinute =>
            _ExerciseDataCollection.Select(exerciseData => exerciseData.WordsPerMinute).ToList();
        
        

        // Methods
        /// <summary>
        /// Sets the file path and initializes the static data containers before any other action.
        /// </summary>
        private void Awake()
        {
            _FilePath = $"{Application.persistentDataPath}/save.json";
            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();
            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = new();
    }

        /// <summary>
        /// Loads the save data every time the application starts.
        /// </summary>
        private void Start()
        {
            Load();
        }

        /// <summary>
        /// Saves the data and clears resets static variables every time the user quits the application.
        /// </summary>
        private void OnApplicationQuit()
        {
            Save();

            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();
            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = new();
        }

        /// <summary>
        /// For standalone apps: writes the save data into the file specified at <c>_FilePath</c>.<br></br>
        /// For WebGL: uses <c>PlayerPrefs</c> class to store save data in the IndexedDB of the player's browser.
        /// </summary>
        public void Save()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            JObject saveData = new()
            {
                { "version", Application.version },
                { "characterData", _CharacterDataCollectionJSON },
                { "exerciseData", _ExerciseDataCollectionJSON }
            };

            using StreamWriter streamWriter = new(_FilePath);
#if UNITY_EDITOR
            streamWriter.Write(saveData.ToString(
                _shouldIndent ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None));
#elif UNITY_WEBGL
            PlayerPrefs.SetString("save", saveData.ToString());
#else
            streamWriter.Write(saveData.ToString());
#endif
            stopwatch.Stop();
            Log($"Saved data (for {_CharacterDataCollection.Count} characters " +
                $"and {_ExerciseDataCollection.Count} exercises) in {stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// For standalone apps: loads save data from the file specified at <c>_FilePath</c>.<br></br>
        /// For WebGL: loads save data using the <c>PlayerPrefs</c> class from the IndexedDB of the player's browser.
        /// </summary>
        public void Load()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
#if UNITY_WEBGL
            if (!PlayerPrefs.HasKey("save"))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            JObject saveData = JObject.Parse(PlayerPrefs.GetString("save"));
#else
            if (!File.Exists(_FilePath))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            using StreamReader streamReader = new(_FilePath);
            JObject saveData = JObject.Parse(streamReader.ReadToEnd());
#endif
            string version = saveData["version"].Value<string>();

            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = saveData["characterData"].Value<JObject>();
            foreach (KeyValuePair<string, JToken> characterDataPair in _CharacterDataCollectionJSON)
            {
                CharacterData characterData = new();
                characterData.FromJSON(characterDataPair.Value);
                _CharacterDataCollection.Add(characterDataPair.Key[0], characterData);
            }

            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = saveData["exerciseData"].Value<JArray>();
            foreach (JToken exerciseDataJSON in _ExerciseDataCollectionJSON)
            {
                ExerciseData exerciseData = new();
                exerciseData.FromJSON(exerciseDataJSON);
                _ExerciseDataCollection.Add(exerciseData);
            }

            stopwatch.Stop();
            Log($"Loaded save data (for {_CharacterDataCollection.Count} characters " +
                $"and {_ExerciseDataCollection.Count} exercises) in {stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Changes how many times a character was typed correctly or incorrectly by a given amount.
        /// </summary>
        /// <param name="character">The character to change data for.</param>
        /// <param name="amount">The amount to change the character's data with.</param>
        private void ChangeCharacterData(char character, int amount)
        {
            if (_CharacterDataCollection.ContainsKey(character))
            {
                _CharacterDataCollection[character] += amount;
                _CharacterDataCollectionJSON[$"{character}"] = _CharacterDataCollection[character].ToJSON();
            }
            else
            {
                CharacterData characterData = amount >= 0 ? new(amount, 0) : new(0, Mathf.Abs(amount));
                _CharacterDataCollection.Add(character, characterData);
                _CharacterDataCollectionJSON.Add(new JProperty($"{character}", characterData.ToJSON()));
            }
        }

        /// <summary>
        /// Adds a correct character typing (aka a hit) to the player's data.
        /// Designed to be used after typing a character so data is not immediately saved.
        /// Can be used for a bulk update by specifying the <c>amount</c> parameter.
        /// </summary>
        /// <param name="character">The character that was correctly typed.</param>
        /// <param name="amount">The number of times the character was typed correctly, 1 by default.</param>
        public void AddCharacterHit(char character, int amount = 1)
        {
            ChangeCharacterData(character, amount);
        }

        /// <summary>
        /// Adds an  incorrect character typing (aka a miss) to the player's data.
        /// Designed to be used after typing a character so data is not immediately saved.
        /// Can be used for a bulk update by specifying the <c>amount</c> parameter.
        /// </summary>
        /// <param name="character">The character that was incorrectly typed.</param>
        /// <param name="amount">The number of times the character was typed incorrectly, 1 by default.</param>
        public void AddCharacterMiss(char character, int amount = -1)
        {
            ChangeCharacterData(character, amount);
        }

        /// <summary>
        /// Adds all the data from an exercise to the player's data.
        /// </summary>
        /// <param name="exerciseData">The data of the exercise to be added.</param>
        public void AddExerciseDataSimply(ExerciseData exerciseData)
        {
            _ExerciseDataCollection.Add(exerciseData);
            _ExerciseDataCollectionJSON.Add(exerciseData.ToJSON());
        }

        /// <summary>
        /// Adds all the data from an exercise to the player's data then saves.
        /// </summary>
        /// <param name="exerciseData">The data of the exercise to be added.</param>
        public void AddExerciseData(ExerciseData exerciseData)
        {
            AddExerciseDataSimply(exerciseData);

            Save();
        }

        /// <summary>
        /// Clears all the player data and also saves the shocking emptiness that remains.
        /// Mainly to be used for debugging purposes or as an additional progress reset feature.
        /// </summary>
        public void Clear()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();

            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = new();

            stopwatch.Stop();
            Log($"Cleared data in {stopwatch.ElapsedMilliseconds} ms");

            Save();
        }

        /// <summary>
        /// Generates and saves dummy data for testing purposes.
        /// </summary>
        /// <param name="exerciseCount">How many random exercises generate data for, 100 000 by default</param>
        /// <param name="firstCharacter">The first character in the range to generate data for, 'a' by default.</param>
        /// <param name="lastCharacter">The last character in the range to generate data for, 'z' by default.</param>
        private void GenerateDummyData(int exerciseCount = 100_000, char firstCharacter = 'a', char lastCharacter = 'z')
        {
            for (int i = firstCharacter; i <= lastCharacter; ++i)
            {
                AddCharacterHit((char)i, Random.Range(500, 5000));
                AddCharacterMiss((char)i, Random.Range(500, 5000));
            }

            for (int i = 0; i < exerciseCount; ++i)
            {
                AddExerciseDataSimply(new ExerciseData(
                    Random.Range(10, 70),
                    Random.Range(50, 70)));
            }

            Save();
        }

        /// <summary>
        /// Logs the given, persistence related message to the console using the
        /// [<c>_DebugGroup</c>] message format.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void Log(string message)
        {
            Debug2.Log(message, _DebugGroup);
        }
    }
}