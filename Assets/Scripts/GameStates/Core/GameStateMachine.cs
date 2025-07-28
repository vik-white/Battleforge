using Zenject;

namespace vikwhite
{
    public interface IGameStateMachine
    {
        void SwitchState(GameState nextState);
    }

    public class GameStateMachine : ITickable, IGameStateMachine
    {
        private readonly IGameStateFactory _gameStateFactory;
        private IState _currentState;

        public GameStateMachine(IGameStateFactory gameStateFactory) {
            _gameStateFactory = gameStateFactory;
        }

        public void SwitchState(GameState nextState) {
            _currentState?.Exit();
            _currentState = _gameStateFactory.Get(nextState);
            _currentState.Enter();
        }

        public void Tick() {
            if(_currentState == null || !(_currentState is IUpdateble)) return;
            (_currentState as IUpdateble).Update();
        }
    }
}