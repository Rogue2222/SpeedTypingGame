using UnityEngine;

namespace SpeedTypingGame.Game {
    [AddComponentMenu("SpeedTypingGame/Game/Input manager")]
    public class InputManager : MonoBehaviour {

        public bool PauseKeyPressed() {
            return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                   (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.P));
        }

        public bool EscapeKeyPressed() {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab);
        }

        public bool NoInput() {
            return !Input.anyKeyDown;
        }

        public bool NoText() {
            return string.IsNullOrEmpty(Input.inputString);
        }
        
        public bool Restart() { // SHIFT + R? 
            return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R);
        }

        public bool NewExercise() {  // SHIFT + N?
            return (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.N);
        }
        
        public bool Play() {
            return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.P) ||
                Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1);
        }

        public bool Settings()
        {
            return Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2);
        }

        public bool Statistics()
        {
            return Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3);
        }

        public bool Exit()
        {
            return Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4);
        }
    }
}
