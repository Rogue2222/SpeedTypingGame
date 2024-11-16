using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using UnityEngine;

using SpeedTypingGame.Game.Excercises;

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
        private static readonly List<ExcerciseData> _ExcerciseDataCollection = new();
        private static JArray _ExcerciseDataCollectionJSON = new();

        [SerializeField] private GameManager _game;
#if UNITY_EDITOR
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
        public ExcerciseData this[int index] => _ExcerciseDataCollection[index];
        public List<ExcerciseData> ExcerciseData => new(_ExcerciseDataCollection);
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
        private void Awake()
        {
            _FilePath = $"{Application.persistentDataPath}/save.json";
            _CharacterDataCollection.Clear();
            _CharacterDataCollectionJSON = new();
            _ExcerciseDataCollection.Clear();
            _ExcerciseDataCollectionJSON = new();
    }

        private void Start()
        {
            Load();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            JObject saveData = new()
            {
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

        public void Load()
        {
#if UNITY_WEBGL
            if (!PlayerPrefs.HasKey("save"))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            JObject saveData = JObject.Parse(PlayerPrefs.GetString("save"));
#else
            if (!File.Exists(_FilePath))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            using StreamReader streamReader = new(_FilePath);
            JObject saveData = JObject.Parse(streamReader.ReadToEnd());
#endif
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
                ExcerciseData excerciseData = new();
                excerciseData.FromJSON(excerciseDataJSON);
                _ExcerciseDataCollection.Add(excerciseData);
            }

            stopwatch.Stop();
            Log($"Loaded save data (for {_CharacterDataCollection.Count} characters " +
                $"and {_ExcerciseDataCollection.Count} excercises) in {stopwatch.ElapsedMilliseconds} ms");
        }

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

        public void AddCharacterHit(char character)
        {
            ChangeCharacterData(character, 1);
        }

        public void AddCharacterMiss(char character)
        {
            ChangeCharacterData(character, -1);
        }

        public void AddExcerciseData(ExcerciseData excerciseData)
        {
            _ExcerciseDataCollection.Add(excerciseData);
            _ExcerciseDataCollectionJSON.Add(excerciseData.ToJSON());

            Save();
        }

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

        private void Log(string message)
        {
            Debug2.Log(message, _DebugGroup);
        }
    }
}