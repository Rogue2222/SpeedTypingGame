using UnityEngine;

using SpeedTypingGame.Game.Players;
using SpeedTypingGame.Game.Excercises;
using SpeedTypingGame.GUI;

namespace SpeedTypingGame.Game
{
    [AddComponentMenu("SpeedTypingGame/Game/Game manager")]
    public class GameManager : MonoBehaviour
    {
        // Fields
        [SerializeField] private GUIManager _gui;

        [SerializeField] private Player _player;
        private Excercise _excercise;
        private bool _isRunning;
        private bool _isPaused;
        private float _elapsedTime;


        // Properties
        public Player Player => _player;
        public Excercise Excercise => _excercise;
        public bool IsRunning => _isRunning;
        public bool IsPaused => _isPaused;
        public float ElapsedTime => _elapsedTime;


        // Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isPaused)
                {
                    Pause();
                }
                else
                {
                    Resume();
                }
            }

            if (_isRunning && !_isPaused)
            {
                _elapsedTime += Time.deltaTime;
            }
        }

        public void Play()
        {
            _isRunning = true;
            _isPaused = false;
            _elapsedTime = 0f;

            StartExcercise();
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
        public void Stop()
        {
            _isRunning = false;
            _isPaused = false;
        }

        public void StartExcercise()
        {
            _excercise = new(this);
            _player.StartExcercise();
        }

        public void FinishExcercise()
        {
            _player.FinishExcercise();
            
            _elapsedTime = 0f;
            StartExcercise();
        }
    }
}