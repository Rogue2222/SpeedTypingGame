using UnityEngine;

using SpeedTypingGame.Game;
using SpeedTypingGame.GUI.Main;
using SpeedTypingGame.GUI.Settings;
using SpeedTypingGame.GUI.Statistics;
using SpeedTypingGame.GUI.Overlay;

namespace SpeedTypingGame.GUI
{
    [AddComponentMenu("SpeedTypingGame/GUI/GUI manager")]
    public class GUIManager : MonoBehaviour
    {
        // Fields
        [SerializeField] private GameManager _game;

        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private StatisticsMenu _statisticsMenu;
        [SerializeField] private OverlayMenu _overlayMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        private Menu[] _menus;


        // Properties
        public GameManager Game => _game;

        public MainMenu MainMenu => _mainMenu;
        public StatisticsMenu StatisticsMenu => _statisticsMenu;
        public OverlayMenu OverlayMenu => _overlayMenu;
        public SettingsMenu SettingsMenu => _settingsMenu;


        // Methods
        private void Awake()
        {
            _menus = new Menu[]
            {
                _mainMenu,
                _statisticsMenu,
                _overlayMenu,
                _settingsMenu
            };
        }

        private void Start()
        {
            foreach (Menu menu in _menus)
            {
                if (menu != _mainMenu)
                {
                    menu.Close();
                }
                else
                {
                    menu.Open();
                }
            }
        }
    }
}