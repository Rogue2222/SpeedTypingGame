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
        private static readonly List<ExerciseData> _ExerciseDataCollection = new();
        private static JArray _ExerciseDataCollectionJSON = new();

        [SerializeField] private GameManager _game;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldIndent;
#endif


        // Properties
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

            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = new();
        }

        /// <summary>
        /// For standalone apps: writes the save data into the file specified at <c>_FilePath</c>.<br />
        /// For WebGL: uses <c>PlayerPrefs</c> class to store save data in the IndexedDB of the player's browser.
        /// </summary>
        public void Save()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            JObject saveData = new()
            {
                { "version", Application.version },
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
            Log($"Saved data for {_ExerciseDataCollection.Count} exercises in {stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// For standalone apps: loads save data from the file specified at <c>_FilePath</c>.<br />
        /// For WebGL: loads save data using the <c>PlayerPrefs</c> class from the IndexedDB of the player's browser.
        /// </summary>
        public void Load()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
#if UNITY_STANDALONE || UNITY_EDITOR
            if (!File.Exists(_FilePath))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            using StreamReader streamReader = new(_FilePath);
            JObject saveData = JObject.Parse(streamReader.ReadToEnd());
#elif UNITY_WEBGL
            if (!PlayerPrefs.HasKey("save"))
            {
                Log("No save data was found, nothing got loaded");
                return;
            }

            JObject saveData = JObject.Parse(PlayerPrefs.GetString("save"));            
#endif
            string version = saveData["version"].Value<string>();

            _ExerciseDataCollection.Clear();
            _ExerciseDataCollectionJSON = saveData["exerciseData"].Value<JArray>();
            foreach (JToken exerciseDataJSON in _ExerciseDataCollectionJSON)
            {
                ExerciseData exerciseData = new();
                exerciseData.FromJSON(exerciseDataJSON);
                _ExerciseDataCollection.Add(exerciseData);
            }

            stopwatch.Stop();
            Log($"Loaded save data for {_ExerciseDataCollection.Count} exercises in {stopwatch.ElapsedMilliseconds} ms");
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
        private void GenerateDummyData(int exerciseCount = 100_000)
        {
            for (int i = 0; i < exerciseCount; ++i)
            {
                AddExerciseDataSimply(new ExerciseData(
                    Random.Range(50, 90),
                    Random.Range(30, 100)));
            }

            Save();
        }

        /// <summary>
        /// Logs the given, persistence related message to the console using the
        /// [<c>_DebugGroup</c>] message format.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        private void Log(object message)
        {
            Debug2.Log(message, _DebugGroup);
        }
    }
}