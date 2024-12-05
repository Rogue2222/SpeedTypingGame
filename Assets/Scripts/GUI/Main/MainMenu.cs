using SpeedTypingGame.Game;
using UnityEngine;

namespace SpeedTypingGame.GUI.Main
{
    [AddComponentMenu("SpeedTypingGame/GUI/Main/Main menu")]
    public class MainMenu : Menu
    {
        // Fields
        [SerializeField] private InputManager _inputManager;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldSkip;
#endif       


        // Methods
#if UNITY_EDITOR
        private void Start()
        {
            if (_shouldSkip)
            {
                PlayGame();
            }
        }
#endif

        private void Update() {
            if (_inputManager.Play()) {
                PlayGame();
            }
        }

        public void PlayGame()
        {
            Game.NewExercise();

            _gui.OverlayMenu.Open();
            Close();
        }

        public void OpenStatisticsMenu()
        {
            _gui.StatisticsMenu.Open();
            Close();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}