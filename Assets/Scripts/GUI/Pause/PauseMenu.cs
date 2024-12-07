using UnityEngine;

namespace SpeedTypingGame.GUI.Pause
{
    [AddComponentMenu("SpeedTypingGame/GUI/Pause/Pause menu")]
    public class PauseMenu : Menu
    {
        // Methods
        public void ResumeGame()
        {
            Game.Resume();
            
            Close();
        }

        public void OpenMainMenu()
        {
            Game.Stop();

            _gui.OverlayMenu.Close();
            Back();
        }
    }
}