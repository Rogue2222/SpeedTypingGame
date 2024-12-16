using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SpeedTypingGame.Game;
using SpeedTypingGame.Game.Exercises;

namespace SpeedTypingGame.GUI.Main
{
    [AddComponentMenu("SpeedTypingGame/GUI/Main/Main menu")]
    public class MainMenu : Menu
    {
        // Fields
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private GameObject _exitButton;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldSkip;
#endif


// Methods
        private void Start()
        {
#if UNITY_WEBGL
            _exitButton.SetActive(false);
#elif UNITY_EDITOR
            if (_shouldSkip)
            {
                PlayGame();
            }
#endif
        }

        private new void Update() {
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

        public void OpenSettingsMenu()
        {
            _gui.SettingsMenu.Open();
            Close();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}