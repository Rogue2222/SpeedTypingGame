using System.Text;
using UnityEngine;

using SpeedTypingGame.Game.Excercises;

namespace SpeedTypingGame.Game.Players
{
    [AddComponentMenu("SpeedTypingGame/Players/Player")]
    public class Player : MonoBehaviour
    {
        // Fields
        [SerializeField] private GameManager _game;

        private readonly StringBuilder _stringBuilder = new(Excercise.DefaultLength);


        // Properties
        public string Text => _stringBuilder.ToString();


        // Methods
        private void Update()
        {
            if (!_game.IsRunning || _game.IsPaused) return;

            if (!Input.anyKeyDown || string.IsNullOrEmpty(Input.inputString)) return;

            int keyCode = Input.inputString[^1];
            // The backspace key was hit
            if (keyCode == (int)KeyCode.Backspace)
            {
                HitBackspace();
                _game.Excercise.HitBackspace();
            }
            // Some other key than backspace was hit
            else
            {
                char character = (char)keyCode;

                TypeCharacter(character);
                _game.Excercise.TypeCharacter(character);
            }
        }

        public void TypeCharacter(char character)
        {
            if (_stringBuilder.Length < _game.Excercise.Length)
            {
                _stringBuilder.Append(character);
            }
        }

        public void HitBackspace()
        {
            if (_stringBuilder.Length > 0)
            {
                _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
            }
        }

        public void StartExcercise()
        {
            _stringBuilder.Clear();
        }

        public void FinishExcercise()
        {

        }
    }
}