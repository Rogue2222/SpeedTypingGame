using UnityEngine;

namespace SpeedTypingGame.GUI.Main
{
    [AddComponentMenu("SpeedTypingGame/GUI/Main/Main menu")]
    public class MainMenu : Menu
    {
        // Methods
        public void Play()
        {
            Close();

            Game.Play();

            _gui.OverlayMenu.Open();
        }
    }
}