using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Data source: https://apiacoa.org/publications/teaching/datasets/google-10000-english.txt
namespace SpeedTypingGame.Game.Exercises
{
    /// <summary>
    /// Responsible for loading a dictionary which then can be used to randomly select words from for exercises.
    /// </summary>
    [AddComponentMenu("SpeedTypingGame/Game/Exercises/Exercise generator")]
    public class ExerciseGenerator : MonoBehaviour
    {
        // Fields
        private const string _DebugGroup = "GENERATOR";
        private const int _MinCharacterCount = 32;
        private const int _MaxCharacterCount = 32;
        private static readonly List<string> _Dictionary = new();

        [SerializeField] private Vector2Int _characterCountRange = new(_MinCharacterCount, _MaxCharacterCount);
        [SerializeField] private TextAsset _dictionaryFile;


        // Properties
        /// <summary>
        /// The number of words in the dictionary.
        /// </summary>
        public static int DictionarySize => _Dictionary.Count;


        // Methods
        /// <summary>
        /// Loads the dictionary.
        /// </summary>
        private void Start()
        {
            Load();
        }

        /// <summary>
        /// Loads words from the provided dictionary file into <c>_Dictionary</c> where each row corresponds to one.
        /// </summary>
        private void Load()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            _Dictionary.Clear();

            string cleanedText = _dictionaryFile.text.Replace("\r", "");
            string[] words = cleanedText.Split('\n');
            _Dictionary.Capacity = Mathf.NextPowerOfTwo(words.Length);
            _Dictionary.AddRange(words);

            stopwatch.Stop();
            Log($"Loaded {_Dictionary.Count} words from <{_dictionaryFile.name}> in {stopwatch.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Generates a list of words by randomly selecting them from <c>_Dictionary</c> in a way that the sum of their<br />
        /// lengths is roughly in the range of [<c>minCharacterCount</c>, <c>maxCharacterCount</c>].
        /// </summary>
        /// <param name="minCharacterCount">
        /// The lower limit for the total length of words (<c>_MinCharacterCount</c> by default).</param>
        /// <param name="maxCharacterCount">
        /// The rough upper limit for the total length of words (<c>_MaxCharacterCount</c> by default).</param>
        /// <returns>A list of randomly selected words from <c>_Dictionary</c>.</returns>
        public List<string> Generate(int minCharacterCount = _MinCharacterCount, int maxCharacterCount = _MaxCharacterCount)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int targetCharacterCount = Random.Range(minCharacterCount, maxCharacterCount) - 2;
            int characterCount = 0;

            List<string> words = new(targetCharacterCount / 2);
            HashSet<int> usedIndexes = new(targetCharacterCount / 2);
            
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

            stopwatch.Stop();
            Log($"Selected {words.Count} words with a total length of {characterCount} " +
                $"in {stopwatch.ElapsedMilliseconds} ms");

            return words;
        }

        /// <summary>
        /// Generates a list of words by randomly selecting them from <c>_Dictionary</c> in a way that the sum of their<br />
        /// lengths is roughly in the range of [<c>_characterCountRange.x</c>, <c>maxCharacterCount</c>].
        /// </summary>
        /// <param name="maxCharacterCount">
        /// The rough upper limit for the total length of words. (<c>_MaxCharacterCount</c> by default)</param>
        /// <returns>A list of randomly selected words from <c>_Dictionary.</c></returns>
        public List<string> Generate(int maxCharacterCount = _MaxCharacterCount)
        {
            return Generate(_characterCountRange.x, maxCharacterCount);
        }

        /// <summary>
        /// Generates a list of words by randomly selecting them from <c>_Dictionary</c> in a way that the sum of their<br />
        /// lengths is roughly in the range of [<c>_characterCountRange.x</c>, <c>_characterCountRange.y</c>].
        /// </summary>
        /// <returns>A list of randomly selected words from <c>_Dictionary.</c></returns>
        public List<string> Generate()
        {
            return Generate(_characterCountRange.x, _characterCountRange.y);
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