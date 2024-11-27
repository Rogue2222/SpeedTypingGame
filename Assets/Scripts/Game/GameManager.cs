using SpeedTypingGame.Game.Persistence;
using SpeedTypingGame.Game.Exercises;
using SpeedTypingGame.GUI;
using UnityEngine;

namespace SpeedTypingGame.Game
{
    [AddComponentMenu("SpeedTypingGame/Game/Game manager")]
    public class GameManager : MonoBehaviour
    {
        // Fields
        [SerializeField] public GUIManager _gui;

        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PersistenceHandler _persistence;
        
        private Exercise _exercise;
        private bool _isRunning;
        private bool _isPaused;
        private float _elapsedTime;


        // Properties
        public InputManager Input => _inputManager;
        public PersistenceHandler Persistence => _persistence;
        public Exercise Exercise => _exercise;
        public bool IsRunning => _isRunning;
        public bool IsPaused => _isPaused;
        public float ElapsedTime => _elapsedTime;


        // Methods
        private void Update() {
            if (_inputManager.NoInput()) return;
            
            if (_inputManager.PauseKeyPressed()) {
                if (!_isPaused)
                {
                    Pause();
                }
                else
                {
                    if (_isRunning) {
                        Resume();
                    }
                }
            }


        }

        private void FixedUpdate() {
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

            LoadNewExercise();
        }

        public void Pause() {
            _isPaused = true;
        }

        public void Resume() {
            _isPaused = false;
        }
        public void Stop()
        {
            _isRunning = false;
            _isPaused = false;
        }

        public void StartExercise() {
            _elapsedTime = 0f;
            _isRunning = true;
            Resume();

        }

        public void LoadNewExercise()
        {
            _exercise = new(this);
        }

        public void FinishExercise()
        {
            LoadNewExercise();
            Stop();
        }
    }
}