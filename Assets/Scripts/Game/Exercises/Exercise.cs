using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpeedTypingGame.Game.Exercises
{
    public class Exercise
    {
        // Fields
        public const int DEFAULT_LENGTH = 8;
        public const int WORD_COUNT = 4;

        private GameManager _game;

        private readonly string _text;
        private int _cursorPosition;
        private readonly List<string> _exerciseWords;
        private int _hits;
        private int _misses;
        private int _currentWordIndex = 0;
        private bool _inputIsCorrect;

        // Properties
        public string Text => string.Join(" ", _exerciseWords); //_exerciseWords.Aggregate((x, n) => x + " " + n);
        public int Length => _text.Length;
        public int CursorPosition => _cursorPosition;
        public string ToCursor => _text.Substring(0, _cursorPosition);
        public string FromCursor => _text.Substring(_cursorPosition, _text.Length - _cursorPosition);
        public int WordCount => _exerciseWords.Count;
        public string this[int index] => _exerciseWords[index];
        public string CurrentWord => this[_currentWordIndex];
        public List<string> ExerciseWords => _exerciseWords;
        public int CurrentWordIndex => _currentWordIndex;
        public int Hits => _hits;
        public int Misses => _misses;
        public double Accuracy => Math.Round(Math.Max(1 - _misses / (float)Text.Length, 0) * 100);

        public string CurrentInput { get; private set; }
        public string PreviousInput { get; private set; } = "";
        
        /// <summary>
        /// The amount of the right characters written in the input. 
        /// </summary>
        public int RightCharactersFromCurrentInput => CurrentWord.StartsWith(CurrentInput) ? CurrentInput.Length : 0;

        // Methods
        public Exercise(GameManager game, int length = DEFAULT_LENGTH, int wordLength = WORD_COUNT)
        {
            _game = game;
            
            _exerciseWords = new();
            
            StringBuilder stringBuilder = new(length);
            for (int j = 0; j < wordLength; j++) {
                for (int i = 0; i < length; ++i) {
                    stringBuilder.Append((char)Random.Range(97, 123));
                }
                _exerciseWords.Add(stringBuilder.ToString());
                stringBuilder.Append(" ");
                stringBuilder.Clear();
            }
            Debug.Log("Whole text: " + Text);
        }
        
        // with the event we don't need to check for multiple keys written in the same frame
        public void HandleInputChange(string input) { // it can only grow with one character at a time
            CurrentInput = input;
            
            // Count misses only the first time the user missed at a time 
            if (_inputIsCorrect && !CurrentWord.StartsWith(input.TrimEnd())) {
                ++_misses;
            }
            _inputIsCorrect = CurrentWord.StartsWith(input);
            
            //First character typed
            if (_currentWordIndex == 0 && input.Length == 1 && PreviousInput == string.Empty) {
                _misses = 0;
                _game.StartExercise();
            }
            if (input.Length > 0) _game.gui.PauseMenu.ResumeGame();
            
            //Last word don't need space *pain*
            Debug.Log(WordCount + "/" + _currentWordIndex + " input: " + input + "|");
            if (_currentWordIndex + 1 == WordCount && string.Equals(CurrentWord, input)) {
                Debug.Log("EXERCISE FINISHED");
                _game.FinishExercise();
                _game.gui.OverlayMenu.ClearInputField();
                return;
            }


            if (input.EndsWith(" ") && input.Length > 2 && string.Equals(CurrentWord, input[..^1])) {
                _currentWordIndex++;
                _game.gui.OverlayMenu.ClearInputField();
            }

            PreviousInput = input;
        }

        public int GetWrittenRightCharacters() {
            return GetWordsLenghtTillIndex(_currentWordIndex) + RightCharactersFromCurrentInput;
        }

        public int GetWordsLenghtTillIndex(int index) {
            return ExerciseWords.Take(index).Select(s => s.Length).Sum() + index; // + index is for the spaces
        }
        
    }
}