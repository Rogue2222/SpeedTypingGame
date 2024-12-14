using TMPro;
using UnityEngine;

namespace SpeedTypingGame.GUI {
    public class SettingsMenu : Menu {
        
        [SerializeField] private TMP_InputField inputField;
        
        public string CustomText => inputField.text;
        
    }
}