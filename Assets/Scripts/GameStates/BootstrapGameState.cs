using UnityEngine;
using vikwhite.View;

namespace vikwhite
{
    public interface IBootstrapGameState : IState { }

    public class BootstrapGameState : IBootstrapGameState
    {
        private readonly IGameStateMachine _game;
        private readonly ISceneService _scene;

        public BootstrapGameState(IGameStateMachine game, ISceneService scene) {
            _game = game;
            _scene = scene;
        }

        public void Enter() {
            _scene.UnloadCurrent();
            _game.SwitchState(GameState.Menu);
        }

        public void Exit() { }
    }
}