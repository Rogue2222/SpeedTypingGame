using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SpeedTypingGame.Game {
    public class InputManager : MonoBehaviour {

        public bool PauseKeyPressed() {
            if (Input.GetKeyDown(KeyCode.Escape)) return true;
            return Input.GetKey(KeyCode.LeftControl) &&
                   (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.P));
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
            throw new NotImplementedException();
        }        
    }
}
