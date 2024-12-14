using System;
using UnityEngine;

namespace SpeedTypingGame.Game {
    [AddComponentMenu("SpeedTypingGame/Game/Input manager")]
    public class InputManager : MonoBehaviour {

        public bool PauseKeyPressed() {
            return Input.GetKey(KeyCode.LeftControl) &&
                   (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.P));
        }

        public bool EscapeKeyPressed() {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        public bool NoInput() {
            return !Input.anyKeyDown;
        }

        public bool NoText() {
            return string.IsNullOrEmpty(Input.inputString);
        }
        
        public bool Restart() { // CTRL + R? 
            throw new NotImplementedException();
        }

        public bool NewExercise() {  // CTRL + N?
            return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.N);
        }
        
        public bool Play() {
            return Input.GetKey(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.P);
        }        
    }
}
