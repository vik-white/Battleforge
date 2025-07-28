namespace vikwhite
{
    public interface ILoadGameState : IState { }

    public class LoadGameState : ILoadGameState
    {
        private readonly IGameStateMachine _game;
        private readonly IPlayerService _player;
        
        public LoadGameState(IGameStateMachine game, IPlayerService player) {
            _game = game;
            _player = player;
        }

        public void Enter() {
            _player.Initialize("Player1");
            _game.SwitchState(GameState.Lobby);
        }

        public void Exit() { }
    }
}