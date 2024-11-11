using UnityEngine;

namespace SpeedTypingGame.GUI.Pause
{
    [AddComponentMenu("SpeedTypingGame/GUI/Pause/Pause menu")]
    public class PauseMenu : Menu
    {
        // Methods
        public void ResumeGame()
        {
            Close();

            Game.Resume();
        }

        public void OpenMainMenu()
        {
            Close();

            _gui.OverlayMenu.Close();

            Game.Stop();

            _gui.MainMenu.Open();
        }
    }
}