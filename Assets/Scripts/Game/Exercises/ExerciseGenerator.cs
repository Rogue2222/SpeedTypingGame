using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

// Data source: https://apiacoa.org/publications/teaching/datasets/google-10000-english.txt;
namespace SpeedTypingGame.Game.Exercises
{
    public class ExerciseGenerator : MonoBehaviour
    {
        // Fields
        private const string _DebugGroup = "GENERATOR";
        private static string _DictionaryPath;
        private static readonly Vector2Int _DefaultCharacterCountRange = new(32, 32);
        private static readonly List<string> _Dictionary = new(8192);

        [SerializeField] private Vector2Int _characterCountRange = _DefaultCharacterCountRange;
        [SerializeField] private TextAsset _dictionaryFile;


        // Properties
        public static int DictionarySize => _Dictionary.Count;


        // Methods
        private void Awake()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            _DictionaryPath = $"{Application.streamingAssetsPath}/dictionary.txt";
#elif UNITY_WEBGL
            _DictionaryPath = $"dictionary.txt";
#endif
        }

        private void Start()
        {
            Load();
        }

        private void Load()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _Dictionary.Clear();

#if UNITY_STANDALONE || UNITY_EDITOR
            StreamReader streamReader = new(_DictionaryPath);
            string word = streamReader.ReadLine();
            while (!string.IsNullOrEmpty(word))
            {
                _Dictionary.Add(word);
                word = streamReader.ReadLine();
            }
            streamReader.Close();
#elif UNITY_WEBGL
            _Dictionary.AddRange(_dictionaryFile.text.Split('\n'));
#endif
            stopwatch.Stop();
            Log($"Loaded {_Dictionary.Count} words from <{_DictionaryPath}> in {stopwatch.ElapsedMilliseconds} ms");
        }

        public List<string> Generate()
        {
            int targetCharacterCount = Random.Range(_characterCountRange.x, _characterCountRange.y);
            int characterCount = 0;

            List<string> words = new(targetCharacterCount / 2);
            HashSet<int> usedIndexes = new(targetCharacterCount / 4);
            
            while (characterCount < targetCharacterCount)
            {
                int index;
                do
                {
                    index = Random.Range(0, _Dictionary.Count);
                } while (usedIndexes.Contains(index));
                usedIndexes.Add(index);

                string word = _Dictionary[index];
                words.Add(word);
                characterCount += word.Length;
            }

            return words;
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