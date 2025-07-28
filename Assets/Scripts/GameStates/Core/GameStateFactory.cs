using System;
using Zenject;

namespace vikwhite
{
    public enum GameState { Bootstrap, Menu, Load, Battle, Lobby }

    public interface IGameStateFactory
    {
        IState Get(GameState gameState);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private DiContainer _container;

        public GameStateFactory(DiContainer container) {
            _container = container;
        }

        public IState Get(GameState gameState) {
            return gameState switch {
                GameState.Bootstrap => _container.Resolve<IBootstrapGameState>(),
                GameState.Menu => _container.Resolve<IMenuGameState>(),
                GameState.Load => _container.Resolve<ILoadGameState>(),
                GameState.Battle => _container.Resolve<IBattleGameState>(),
                GameState.Lobby => _container.Resolve<ILobbyGameState>(),
                _ => throw new Exception("Unknown GameState")
            };
        }
    }
}