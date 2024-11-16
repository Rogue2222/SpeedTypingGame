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
        private int _totalCorrectKeyCount;
        private int _hits;
        private int _totalIncorrectKeyCount;
        private int _misses;


        // Properties
        public string Text => _text;
        public int Length => _text.Length;
        public int CursorPosition => _cursorPosition;
        public string ToCursor => _text.Substring(0, _cursorPosition);
        public string FromCursor => _text.Substring(_cursorPosition, _text.Length - _cursorPosition);
        public int ChunkCount => _excerciseChunks.Count;
        public ExcerciseChunk this[int index] => _excerciseChunks[index];
        public int TotalCorrectKeyCount => _totalCorrectKeyCount;
        public int Hits => _hits;
        public int TotalIncorrectKeyCount => _totalIncorrectKeyCount;
        public int Misses => _misses;
        public float Accuracy => _totalCorrectKeyCount / _totalIncorrectKeyCount;


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
                ++_totalCorrectKeyCount;
                ++_hits;

                if (_hits == Length)
                {
                    _game.FinishExcercise();
                }
            }
            else
            {
                ++_totalIncorrectKeyCount;
                ++_misses;
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
                if (_misses > 0)
                {
                    --_misses;
                }
                else
                {
                    --_hits;
                }
            }
        }
    }
}