using UnityEngine;

namespace SpeedTypingGame.GUI.Main
{
    [AddComponentMenu("SpeedTypingGame/GUI/Main/Main menu")]
    public class MainMenu : Menu
    {
        // Methods
        public void PlayGame()
        {
            Game.Play();

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