using System;
using System.Text;
using UnityEngine;
using TMPro;

using SpeedTypingGame.Game;

namespace SpeedTypingGame.GUI.Overlay
{
    [AddComponentMenu("SpeedTypingGame/GUI/Overlay/Overlay menu")]
    public class OverlayMenu : Menu
    {
        // Fields
        private const string _NeutralTextColor = "white";
        private const string _CorrectTextColor = "green";
        private const string _WrongTextColor = "red";

        [SerializeField] private TextMeshProUGUI _timerLabel;
        [SerializeField] private TextMeshProUGUI _exerciseLabel;
        [SerializeField] private TextMeshProUGUI _accuracyLabel;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private InputManager _inputManager;
        
        private void Start() {
            _inputField.onValueChanged.AddListener(s => Game.Exercise.HandleInputChange(s));
            _inputField.onValueChanged.AddListener(s => _exerciseLabel.text = FormatExerciseText());

            _inputField.restoreOriginalTextOnEscape = false;
            _inputField.onFocusSelectAll = false;
            _exerciseLabel.text = FormatExerciseText();
        }

        // Methods
        protected override void Update()
        {
            base.Update();
            // _timerLabel.text = $"{(int)(Game.ElapsedTime * 100 + 0.5f) / 100f} s";
            if (Game.ElapsedTime > 0 && Game.IsRunning && !Game.IsPaused) {
                UpdateStatistics();
            }

            // So the input box always in focus
            _inputField.ActivateInputField();
        }

        public void UpdateStatistics() {
            _timerLabel.text =
                $"{Game.Exercise.WordsPerMinute:F0} : WPM";
            _accuracyLabel.text =
                $"Accuracy: {Game.Exercise.Accuracy:F0}%";
        }

        private string FormatExerciseText()
        {
            // Get the current input and current word
            string[] words = Game.Exercise.ExerciseWords.ToArray();
            if (words.Length == 0) return string.Empty;
            string input = Game.Exercise.CurrentInput;
            int currentWordIndex = Game.Exercise.CurrentWordIndex;
            string currentWord = words[currentWordIndex];

            // StringBuilder to construct the formatted exercise text
            StringBuilder formattedText = new();
            
            // Words that are done already
            if (currentWordIndex > 0 && currentWordIndex < words.Length) {
                formattedText.Append(FormatExerciseChunkText(string.Join(" ", words[..currentWordIndex]), true));
                formattedText.Append(" ");
            }
                
            // Format the current word
            if (string.IsNullOrEmpty(input))
            {
                // Neutral for an empty input
                formattedText.Append($"<color={_NeutralTextColor}>{currentWord}</color>");
            }
            else
            {
                // bool isCorrect = currentWord.StartsWith(input);
                // formattedText.Append(FormatExerciseChunkText(currentWord.Substring(0, Math.Min(input.Length, currentWord.Length)), isCorrect));
                formattedText.Append(FormatWord(currentWord, input));
                
                // natural part cuz it's not written yet
                if (input.Length < currentWord.Length) 
                    formattedText.Append($"<color={_NeutralTextColor}>" +
                                         $"{currentWord.Substring(input.Length, currentWord.Length - input.Length)}</color>");
            }
            formattedText.Append(" ");

            // Text after the current word: all neutral
            formattedText.Append($"<color={_NeutralTextColor}>{string.Join(" ", words[(currentWordIndex + 1)../*words.Length*/])}</color>");

            return formattedText.ToString();
        }

        private string FormatWord(string word, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return $"<color={_NeutralTextColor}>{word}</color>";
            }

            StringBuilder textBuilder = new();

            bool isChunkCorrect = input[0] == word[0];
            StringBuilder chunkTextBuilder = new("" + word[0]);
            
            for (int i = 1; i < Math.Min(input.Length, word.Length); ++i)
            {
                if (input[i] == word[i] == isChunkCorrect)
                {
                    chunkTextBuilder.Append(word[i]);
                }
                else
                {
                    textBuilder.Append(FormatExerciseChunkText(
                        chunkTextBuilder.ToString(), isChunkCorrect));

                    isChunkCorrect = !isChunkCorrect;
                    chunkTextBuilder.Clear();
                    chunkTextBuilder.Append(word[i]);
                }
            }

            textBuilder.Append(FormatExerciseChunkText(
                chunkTextBuilder.ToString(), isChunkCorrect));

            return textBuilder.ToString();
        }

        private string FormatExerciseChunkText(string chunkText, bool isCorrect)
        {
            return $"<color={(isCorrect ? _CorrectTextColor : _WrongTextColor)}>{chunkText}</color>";
        }

        public void ClearInputField() {
            _inputField.text = "";
            FormatExerciseText();
        }

        public void UpdateText() {
            _exerciseLabel.text = FormatExerciseText();
        }
    }
}