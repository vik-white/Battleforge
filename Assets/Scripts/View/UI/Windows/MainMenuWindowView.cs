using System;
using UnityEngine;
using UnityEngine.UI;

namespace vikwhite.View
{
    public class MainMenuWindowView : UIElement {
        
        [SerializeField] 
        private Button _startGameButton;

        public event Action OnStartGame;

        private void Start() {
            _startGameButton.onClick.AddListener(() => OnStartGame?.Invoke());
        }
    }
}