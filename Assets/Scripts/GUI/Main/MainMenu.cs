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
        [SerializeField] private TMP_Dropdown _generatorMethodDropdown;
        [SerializeField] private Slider _generatorCountSlider;
        [SerializeField] private TextMeshProUGUI _generatorCountSliderValueLabel;
#if UNITY_EDITOR
        [Header("Development options")]
        [SerializeField] private bool _shouldSkip;
#endif


// Methods
        private void Start()
        {
            UpdateGeneratorMethodDropdown();
            UpdateGeneratorCountSlider();

#if UNITY_WEBGL
            _exitButton.SetActive(false);
#elif UNITY_EDITOR
            if (_shouldSkip)
            {
                PlayGame();
            }
#endif
        }

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

        public void UpdateGeneratorMethodDropdown()
        {
            _generatorMethodDropdown.SetValueWithoutNotify(Game.Generator.IsWordCounter ? 0 : 1);
        }

        public void SelectGeneratorMethod()
        {
            if (_generatorMethodDropdown.value == 0)
            {
                Game.Generator.UseWordCount();
            }
            else
            {
                Game.Generator.UseCharacterCount();
            }

            UpdateGeneratorCountSlider();
            Game.Persistence.Save();
        }

        public void UpdateGeneratorCountSlider()
        {
            if (Game.Generator.IsWordCounter)
            {
                _generatorCountSlider.SetValueWithoutNotify(Game.Generator.WordCount);
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxWordCount;
                _generatorCountSlider.minValue = ExerciseGenerator.MinWordCount;
                _generatorCountSliderValueLabel.text = $"{_generatorCountSlider.value}";
            }
            else
            {
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxCharacterCount;
                _generatorCountSlider.SetValueWithoutNotify(Game.Generator.CharacterCount);
                _generatorCountSlider.minValue = ExerciseGenerator.MinCharacterCount;
                _generatorCountSliderValueLabel.text = $"~{_generatorCountSlider.value}";
            }
        }

        public void SetGeneratorCount()
        {
            if (Game.Generator.IsWordCounter)
            {
                Game.Generator.WordCount = (int)_generatorCountSlider.value;
                _generatorCountSliderValueLabel.text = $"{_generatorCountSlider.value}";
            }
            else
            {
                Game.Generator.CharacterCount = (int)_generatorCountSlider.value;
                _generatorCountSliderValueLabel.text = $"~{_generatorCountSlider.value}";
            }

            Game.Persistence.Save();
        }
    }
}