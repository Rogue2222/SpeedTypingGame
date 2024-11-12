using UnityEngine;

using SpeedTypingGame.Game;
using SpeedTypingGame.GUI.Main;
using SpeedTypingGame.GUI.Overlay;
using SpeedTypingGame.GUI.Pause;
using SpeedTypingGame.GUI.Statistics;

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
        [SerializeField] private PauseMenu _pauseMenu;
        private Menu[] _menus;

#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _skipMainMenu;
#endif


        // Properties
        public GameManager Game => _game;

        public MainMenu MainMenu => _mainMenu;
        public StatisticsMenu StatisticsMenu => _statisticsMenu;
        public OverlayMenu OverlayMenu => _overlayMenu;
        public PauseMenu PauseMenu => _pauseMenu;


        // Methods
        private void Awake()
        {
            _menus = new Menu[]
            {
                _mainMenu,
                _statisticsMenu,
                _overlayMenu,
                _pauseMenu
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

#if UNITY_EDITOR
            if (_skipMainMenu)
            {
                _mainMenu.PlayGame();
            }
#endif
        }
    }
}