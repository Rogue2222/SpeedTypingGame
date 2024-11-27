using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using UnityEngine;

using SpeedTypingGame.Game.Exercises;

namespace SpeedTypingGame.Game.Persistence
{
    /// <summary>
    /// Handles persisting statistical data important between sessions.
    /// </summary>
    [AddComponentMenu("SpeedTypingGame/Game/Persistence/Persistence handler")]
    public class PersistenceHandler : MonoBehaviour
    {
        // Fields
        private const string _DebugGroup = "PERSISTENCE";
        private static string _FilePath;
        private static readonly Dictionary<char, CharacterData> _CharacterDataCollection = new();
        private static JObject _CharacterDataCollectionJSON = new();
        private static readonly List<ExerciseData> _ExcerciseDataCollection = new();
        private static JArray _ExcerciseDataCollectionJSON = new();

        [SerializeField] private GameManager _game;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldPrettyPrint;
#endif


        // Properties
        public int CharacterDataCount => _CharacterDataCollection.Count;
        public CharacterData this[char character] => _CharacterDataCollection[character];
        public List<char> Characters => new(_CharacterDataCollection.Keys);
        public List<Tuple<char, CharacterData>> CharacterData =>
            _CharacterDataCollection.Select(characterDataPair =>
            new Tuple<char, CharacterData>(characterDataPair.Key, characterDataPair.Value)).ToList();
        public int ExcerciseDataCount => _ExcerciseDataCollection.Count;
        public ExerciseData this[int index] => _ExcerciseDataCollection[index];
        public List<ExerciseData> ExcerciseData => new(_ExcerciseDataCollection);
        public List<DateTime> Timestamps =>
            _ExcerciseDataCollection.Select(excerciseData => excerciseData.Timestamp).ToList();
        public List<float> Durations =>
            _ExcerciseDataCollection.Select(excerciseData => excerciseData.Duration).ToList();
        public List<int> Lengths =>
            _ExcerciseDataCollection.Select(excerciseData => excerciseData.Length).ToList();
        public List<int> Hits =>
            _ExcerciseDataCollection.Select(excerciseData => excerciseData.Hits).ToList();
        public List<int> Misses =>
            _ExcerciseDataCollection.Select(excerciseData => excerciseData.Misses).ToList();


        // Methods
        /// <summary>
        /// Sets the file path and initializes the static data containers.
        /// </summary>
        private void Awake()
        {
            _FilePath = $"{Application.persistentDataPath}/save.json";
            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();
            _ExcerciseDataCollection.Clear();
            _ExcerciseDataCollectionJSON = new();
    }

        /// <summary>
        /// Loads the save data every time the application starts.
        /// </summary>
        private void Start()
        {
            Load();
        }

        /// <summary>
        /// Clears data from static variables and saves the save data every time the user quits the application.
        /// </summary>
        private void OnApplicationQuit()
        {
            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();
            _ExcerciseDataCollection.Clear();
            _ExcerciseDataCollectionJSON = new();

            Save();
        }

        /// <summary>
        /// Writes the save data into the file specified at <c>_FilePath</c>.
        /// </summary>
        public void Save()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            JObject saveData = new()
            {
                { "version", Application.version },
                { "characterData", _CharacterDataCollectionJSON },
                { "excerciseData", _ExcerciseDataCollectionJSON }
            };

            using StreamWriter streamWriter = new(_FilePath);
#if UNITY_EDITOR
            streamWriter.Write(saveData.ToString(
                _shouldPrettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None));
#elif UNITY_WEBGL
            PlayerPrefs.SetString("save", saveData.ToString());
#else
            streamWriter.Write(saveData.ToString());
#endif
            stopwatch.Stop();
            Log($"Saved data (for {_CharacterDataCollection.Count} characters " +
                $"and {_ExcerciseDataCollection.Count} excercises) in {stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Loads save data from the file specified at <c>_FilePath</c>.
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

            _ExcerciseDataCollection.Clear();
            _ExcerciseDataCollectionJSON = saveData["excerciseData"].Value<JArray>();
            foreach (JToken excerciseDataJSON in _ExcerciseDataCollectionJSON)
            {
                ExerciseData excerciseData = new();
                excerciseData.FromJSON(excerciseDataJSON);
                _ExcerciseDataCollection.Add(excerciseData);
            }

            stopwatch.Stop();
            Log($"Loaded save data (for {_CharacterDataCollection.Count} characters " +
                $"and {_ExcerciseDataCollection.Count} excercises) in {stopwatch.ElapsedMilliseconds} ms");
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
        /// Designed to be used after typing a character.
        /// </summary>
        /// <param name="character">The character that was correctly typed</param>
        /// <param name="amount">The number of times the character was typed correctly, 1 by default.</param>
        public void AddCharacterHit(char character, int amount = 1)
        {
            ChangeCharacterData(character, amount);
        }

        /// <summary>
        /// Adds an  inorrect character typing (aka a miss) to the player's data.
        /// Designed to be used after typing a character
        /// but can be used for a bulk update by specifying the <c>amount</c> parameter.
        /// </summary>
        /// <param name="character">The character that was incorrectly typed</param>
        /// <param name="amount">The number of times the character was typed incorrectly, 1 by default.</param>
        public void AddCharacterMiss(char character, int amount = -1)
        {
            ChangeCharacterData(character, amount);
        }

        /// <summary>
        /// Adds all the data from excercise to the player's data.
        /// </summary>
        /// <param name="excerciseData">The data of the excercise to be added.</param>
        public void AddExcerciseData(ExerciseData excerciseData)
        {
            _ExcerciseDataCollection.Add(excerciseData);
            _ExcerciseDataCollectionJSON.Add(excerciseData.ToJSON());

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

            _ExcerciseDataCollection.Clear();
            _ExcerciseDataCollectionJSON = new();

            stopwatch.Stop();
            Log($"Cleared data in {stopwatch.ElapsedMilliseconds} ms");

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