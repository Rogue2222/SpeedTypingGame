using System.Text;
using UnityEngine;
using TMPro;

using SpeedTypingGame.Game.Excercises;

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
        [SerializeField] private TextMeshProUGUI _excerciseLabel;


        // Methods
        private void Update()
        {
            _timerLabel.text = $"{((int)(Game.ElapsedTime * 100 + 0.5f)) / 100f} s";

            _excerciseLabel.text = FormatExcerciseText();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _gui.PauseMenu.Toggle();
            }
        }

        private string FormatExcerciseText()
        {
            string playerText = Game.Player.Text;
            string excerciseText = Game.Excercise.Text;

            if (string.IsNullOrEmpty(playerText))
            {
                return $"<color={_NeutralTextColor}>{excerciseText}</color>";
            }

            StringBuilder excerciseTextBuilder = new(Excercise.DefaultLength * 2);

            bool isExcerciseChunkCorrect = playerText[0] == excerciseText[0];
            StringBuilder excerciseChunkTextBuilder = new("" + excerciseText[0], Excercise.DefaultLength * 2);
            
            for (int i = 1; i < playerText.Length; ++i)
            {
                if (playerText[i] == excerciseText[i] == isExcerciseChunkCorrect)
                {
                    excerciseChunkTextBuilder.Append(excerciseText[i]);
                }
                else
                {
                    excerciseTextBuilder.Append(FormatExcerciseChunkText(
                        excerciseChunkTextBuilder.ToString(), isExcerciseChunkCorrect));

                    isExcerciseChunkCorrect = !isExcerciseChunkCorrect;
                    excerciseChunkTextBuilder.Clear();
                    excerciseChunkTextBuilder.Append(excerciseText[i]);
                }
            }

            excerciseTextBuilder.Append(FormatExcerciseChunkText(
                excerciseChunkTextBuilder.ToString(), isExcerciseChunkCorrect));

            excerciseTextBuilder
                .Append($"<color={_NeutralTextColor}>")
                .Append(excerciseText.Substring(playerText.Length, excerciseText.Length - playerText.Length))
                .Append("</color>");

            return excerciseTextBuilder.ToString();
        }

        private string FormatExcerciseChunkText(string chunkText, bool isCorrect)
        {
            return $"<color={(isCorrect ? _CorrectTextColor : _WrongTextColor)}>{chunkText}</color>";
        }
    }
}