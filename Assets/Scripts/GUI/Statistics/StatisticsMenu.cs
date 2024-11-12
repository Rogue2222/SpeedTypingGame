using UnityEngine;

namespace SpeedTypingGame.GUI.Statistics
{
    [AddComponentMenu("SpeedTypingGame/GUI/Statistics/Statistics menu")]
    public class StatisticsMenu : Menu
    {
        // Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }

        public override void Back()
        {
            _gui.MainMenu.Open();

            Close();
        }
    }
}