using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using UnityEngine;

using SpeedTypingGame.Game.Persistence;

// Data source: https://apiacoa.org/publications/teaching/datasets/google-10000-english.txt
namespace SpeedTypingGame.Game.Exercises
{
    /// <summary>
    /// Responsible for loading a dictionary which then can be used to randomly select words from for exercises.
    /// </summary>
    [AddComponentMenu("SpeedTypingGame/Game/Exercises/Exercise generator")]
    public class ExerciseGenerator : MonoBehaviour, IPersistable
    {
        /// <summary>
        /// The generation method of the generator. Can be either word count or character count.
        /// </summary>
        public enum Method
        {
            WordCount,
            CharacterCount
        }


        // Fields
        private const string _DebugGroup = "GENERATOR";
        public const int MinWordCount = 4;
        public const int MaxWordCount = 32;
        public const int MinCharacterCount = 16;
        public const int MaxCharacterCount = 192;
        private static readonly List<string> _Dictionary = new();

        [SerializeField] private TextAsset _dictionaryFile;
        private Method _method;
        private int _wordCount = MinWordCount + (MaxWordCount - MinWordCount) / 2;
        private int _characterCount = MinCharacterCount + (MaxCharacterCount - MinCharacterCount) / 2;


        // Properties
        public static int DictionarySize => _Dictionary.Count;
        public bool IsWordCounter => _method == Method.WordCount;
        public bool IsCharacterCounter => _method == Method.CharacterCount;
        public int WordCount
        {
            get => _wordCount;
            set => _wordCount =
                value >= MinWordCount && value <= MaxWordCount ? value : _wordCount;
        }
        public int CharacterCount
        {
            get => _characterCount;
            set => _characterCount =
                value >= MinCharacterCount && value <= MaxCharacterCount ? value : _characterCount;
        }


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
        /// Sets the generator to generate by counting words.
        /// </summary>
        public void UseWordCount()
        {
            _method = Method.WordCount;
        }

        /// <summary>
        /// Sets the generator to generate by counting characters.
        /// </summary>
        public void UseCharacterCount()
        {
            _method = Method.CharacterCount;
        }

        /// <summary>
        /// Generates a list of words by randomly selecting them from <c>_Dictionary</c> in a way that the either <br />
        /// the number of words or the sum of their lengths is roughly equal to either <c>_wordCount</c> or <br />
        /// <c>_characterCount</c> depending on the currently active method.
        /// <returns>A list of randomly selected words from <c>_Dictionary</c>.</returns>
        public List<string> Generate()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> words = new(MaxCharacterCount / 2);
            HashSet<int> usedIndexes = new(MaxCharacterCount / 2);            
            int characterCount = 0;

            if (IsWordCounter)
            {
                for (int i = 0; i < _wordCount; ++i)
                {
                    int index = SelectIndex(usedIndexes);
                    AddWord(index, words, ref characterCount);
                }
            }
            else
            {
                while (characterCount < _characterCount)
                {
                    int index = SelectIndex(usedIndexes);
                    AddWord(index, words, ref characterCount);
                }
            }

            stopwatch.Stop();
            Log($"Selected {words.Count} words with a total length of {characterCount} " +
                $"in {stopwatch.ElapsedMilliseconds} ms");

            return words;
        }

        /// <summary>
        /// Selects an index randomly from the range of [0, <c>_Dictionary.Count</c>]<br />
        /// which it is not yet included in the provided <c>HashSet</c>.
        /// </summary>
        /// <param name="usedIndexes">The provided <c>HashSet</c> which values should be avoided.</param>
        /// <returns>A new, unique, random index.</returns>
        private int SelectIndex(HashSet<int> usedIndexes)
        {
            int index;
            do
            {
                index = Random.Range(0, _Dictionary.Count);
            } while (usedIndexes.Contains(index));
            usedIndexes.Add(index);
            
            return index;
        }

        /// <summary>
        /// Adds a word to the word list currently being generated.
        /// </summary>
        /// <param name="index">The index of the word in <c>_Dictionary</c>.</param>
        /// <param name="words">The list of words to append to.</param>
        /// <param name="characterCount">The overall character count to increase.</param>
        private void AddWord(int index, List<string> words, ref int characterCount)
        {
            string word = _Dictionary[index];
            words.Add(word);
            characterCount += word.Length;
        }

        /// <summary>
        /// Converts the generator into JSON format that stores its generation method, word and character counts.
        /// </summary>
        /// <returns>The JSON format of the generator's data.</returns>
        public JToken ToJSON()
        {
            return new JObject()
            {
                { "m", _method.ToString() },
                { "w", _wordCount },
                { "c", _characterCount },
            };
        }

        /// <summary>
        /// Deconverts the generator's JSON format by setting from it its generation method, word and character counts.
        /// </summary>
        /// <param name="json">The JSON format of the generator</param>
        public void FromJSON(JToken json)
        {
            _method = json["m"].Value<string>().Equals("WordCount") ? Method.WordCount : Method.CharacterCount;
            _wordCount = json["w"].Value<int>();
            _characterCount = json["c"].Value<int>();
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