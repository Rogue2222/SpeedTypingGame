using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SpeedTypingGame.Game.Exercises;

namespace SpeedTypingGame.GUI.Settings {
    [AddComponentMenu("SpeedTypingGame/GUI/Settings/Settings menu")]
    public class SettingsMenu : Menu {
        
        // Fields
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Dropdown _generatorMethodDropdown;
        [SerializeField] private Slider _generatorCountSlider;
        [SerializeField] private TextMeshProUGUI _generatorCountSliderValueLabel;
        [SerializeField] private TextMeshProUGUI _warningLabel;
        private bool _canUpdateCount = true;
        private string _warningLabelText;
        
        
        // Properties
        public string CustomText => _inputField.text;


        // Methods
        private void Start()
        {
            _inputField.text = Game.Generator.CustomText;

            UpdateGeneratorMethodDropdown();
            UpdateGeneratorCountSlider();

            _warningLabel.gameObject.SetActive(false);
            _warningLabelText = _warningLabel.text;
        }

        public void SetCustomText()
        {
            Game.Generator.CustomText = _inputField.text;
            Game.Persistence.Save();
        }

        public void UpdateGeneratorMethodDropdown()
        {
            _generatorMethodDropdown.SetValueWithoutNotify(Game.Generator.IsWordCounter ? 0 : 1);
        }

        public void SelectGeneratorMethod()
        {
            _canUpdateCount = false;

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

            _canUpdateCount = true;
        }

        public void UpdateGeneratorCountSlider()
        {
            if (Game.Generator.IsWordCounter)
            {
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxWordCount;
                _generatorCountSlider.minValue = ExerciseGenerator.MinWordCount;
                _generatorCountSlider.SetValueWithoutNotify(Game.Generator.WordCount);
                _generatorCountSliderValueLabel.text = $"{_generatorCountSlider.value}";
            }
            else
            {
                _generatorCountSlider.maxValue = ExerciseGenerator.MaxCharacterCount;
                _generatorCountSlider.minValue = ExerciseGenerator.MinCharacterCount;
                _generatorCountSlider.SetValueWithoutNotify(Game.Generator.CharacterCount);
                _generatorCountSliderValueLabel.text = $"~{_generatorCountSlider.value}";
            }
        }

        public void SetGeneratorCount()
        {
            if (!_canUpdateCount) return;
            
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

        public void HoverOnClearButton()
        {
            _warningLabel.gameObject.SetActive(true);
        }

        public void HoverOffClearButton()
        {
            _warningLabel.gameObject.SetActive(false);
            _warningLabel.text = _warningLabelText;
        }

        public void ClearData()
        {
            Game.Persistence.Clear();

            _warningLabel.text = "Cleared data :(";
        }
    }
}