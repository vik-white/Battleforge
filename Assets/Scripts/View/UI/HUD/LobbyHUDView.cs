using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace vikwhite.View
{
    public class LobbyHUDView : UIElement
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _squadButton;
        private IGameStateMachine _game;
        
        
        [Inject]
        public void Constarct(IGameStateMachine game) {
            _game = game;
        }
        
        public void Initialize(Action ShowSquadWindow) {
            _fightButton.onClick.AddListener(() => _game.SwitchState(GameState.Battle));
            _squadButton.onClick.AddListener(() => ShowSquadWindow?.Invoke());
        }
    }
}