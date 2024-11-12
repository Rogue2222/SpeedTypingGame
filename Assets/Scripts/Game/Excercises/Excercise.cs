using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SpeedTypingGame.Game.Excercises
{
    public class Excercise
    {
        // Fields
        public const int DefaultLength = 8;

        private GameManager _game;

        private readonly string _text;
        private int _cursorPosition;
        private readonly List<ExcerciseChunk> _excerciseChunks;
        private int _correctKeyCount;
        private int _incorrectKeyCount;
        private int _progression;


        // Properties
        public string Text => _text;
        public int Length => _text.Length;
        public int CursorPosition => _cursorPosition;
        public string ToCursor => _text.Substring(0, _cursorPosition);
        public string FromCursor => _text.Substring(_cursorPosition, _text.Length - _cursorPosition);
        public int ChunkCount => _excerciseChunks.Count;
        public ExcerciseChunk this[int index] => _excerciseChunks[index];
        public int CorrectKeyCount => _correctKeyCount;
        public int IncorrectKeyCount => _incorrectKeyCount;
        public float Accuracy => _correctKeyCount / _incorrectKeyCount;


        // Methods
        public Excercise(GameManager game, int length = DefaultLength)
        {
            _game = game;

            StringBuilder stringBuilder = new(length);

            for (int i = 0; i < length; ++i)
            {
                stringBuilder.Append((char)Random.Range(97, 123));
            }

            _text = stringBuilder.ToString();

            _excerciseChunks = new()
            {
                new(this)
            };
        }

        public void TypeCharacter(char character)
        {
            if (_text[_cursorPosition] == character)
            {
                ++_correctKeyCount;
                ++_progression;

                if (_progression == Length)
                {
                    _game.FinishExcercise();
                }
            }
            else
            {
                ++_incorrectKeyCount;
                --_progression;
            }

            if (_cursorPosition < _text.Length - 1)
            {
                ++_cursorPosition;
            }
        }

        public void HitBackspace()
        {
            if (_cursorPosition > 0)
            {
                --_cursorPosition;
                --_progression;
            }
        }
    }
}