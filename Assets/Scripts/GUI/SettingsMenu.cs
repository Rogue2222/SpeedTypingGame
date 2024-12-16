using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SpeedTypingGame.Game.Exercises;

namespace SpeedTypingGame.GUI.Settings {
    [AddComponentMenu("SpeedTypingGame/GUI/Settings/Settings menu")]
    public class SettingsMenu : Menu {
        
        // Fields
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TMP_Dropdown _generatorMethodDropdown;
        [SerializeField] private Slider _generatorCountSlider;
        [SerializeField] private TextMeshProUGUI _generatorCountSliderValueLabel;
        [SerializeField] private TextMeshProUGUI _warningLabel;
        
        
        // Properties
        public string CustomText => inputField.text;


        // Methods
        private void Start()
        {
            UpdateGeneratorMethodDropdown();
            UpdateGeneratorCountSlider();

            _warningLabel.gameObject.SetActive(false);
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
            int wordCount = Game.Generator.WordCount;
            int characterCount = Game.Generator.CharacterCount;

            if (Game.Generator.IsWordCounter)
            {
                _generatorCountSlider.minValue = ExerciseGenerator.MinWordCount;
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxWordCount;
                _generatorCountSlider.SetValueWithoutNotify(wordCount);
                _generatorCountSliderValueLabel.text = $"{_generatorCountSlider.value}";
            }
            else
            {
                _generatorCountSlider.minValue = ExerciseGenerator.MinCharacterCount;
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxCharacterCount;
                _generatorCountSlider.SetValueWithoutNotify(characterCount);
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

        public void ClearData()
        {
            Game.Persistence.Clear();
        }

        public void HoverOnClearButton()
        {
            _warningLabel.gameObject.SetActive(true);
        }

        public void HoverOffClearButton()
        {
            _warningLabel.gameObject.SetActive(false);
        }
    }
}