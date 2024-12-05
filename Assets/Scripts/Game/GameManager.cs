using UnityEngine;

using SpeedTypingGame.Game.Exercises;
using SpeedTypingGame.Game.Persistence;
using SpeedTypingGame.GUI;

namespace SpeedTypingGame.Game
{
    [AddComponentMenu("SpeedTypingGame/Game/Game manager")]
    public class GameManager : MonoBehaviour
    {
        // Fields
        [SerializeField] private GUIManager _gui;

        [SerializeField] private InputManager _input;
        [SerializeField] private PersistenceHandler _persistence;
        [SerializeField] private ExerciseGenerator _generator;
        
        private Exercise _exercise;
        private bool _isRunning;
        private bool _isPaused;
        private float _elapsedTime;


        // Properties
        public GUIManager GUI => _gui;
        public InputManager Input => _input;
        public PersistenceHandler Persistence => _persistence;
        public ExerciseGenerator Generator => _generator;
        public Exercise Exercise => _exercise;
        public bool IsRunning => _isRunning;
        public bool IsPaused => _isPaused;
        public float ElapsedTime => _elapsedTime;


        // Methods
        private void Update()
        {
            if (_isRunning && !_isPaused) {
                _elapsedTime += Time.deltaTime;
            }
            if (_input.NoInput()) return;
            if (_exercise != null) Debug.Log($"Accuracy: {_exercise.Accuracy:f2}%");
            
            if (_input.PauseKeyPressed()) {
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

            if (_input.NewExercise())
            {
                NewExercise();
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

        private void LoadNewExercise()
        {
            if (_exercise != null && _persistence && _exercise.IsFinished) _persistence.AddExerciseData(new ExerciseData(_exercise));
            _exercise = new(this);
        }

        public void FinishExercise()
        {
            Stop();
            if (_exercise != null) 
                _gui.OverlayMenu.UpdateWordsPerMinute();
            LoadNewExercise();
        }

        public void NewExercise() {
            FinishExercise();
            _gui.OverlayMenu.ClearInputField();
            _gui.OverlayMenu.UpdateText();
        }
    }
}