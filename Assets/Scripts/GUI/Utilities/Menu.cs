using UnityEngine;

using SpeedTypingGame.Game;

namespace SpeedTypingGame.GUI
{
    public abstract class Menu : MonoBehaviour
    {
        // Fields
        [SerializeField] protected GUIManager _gui;


        // Properties
        public GameManager Game => _gui.Game;

        public bool IsOpen => gameObject.activeSelf;
        public bool IsClosed => !IsOpen;


        // Methods
        public void Open()
        {
            gameObject.SetActive(true);

            OnOpen();
        }

        protected virtual void OnOpen() { }

        public void Close()
        {
            gameObject.SetActive(false);

            OnClose();
        }

        protected virtual void OnClose() { }

        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public virtual void Back()
        {
            _gui.MainMenu.Open();

            Close();
        }
    }
}