using vikwhite.View;
using IUIService = vikwhite.Presenter.IUIService;

namespace vikwhite
{
    public interface ILobbyGameState : IState { }
    
    public class LobbyGameState : ILobbyGameState
    {
        private readonly IUIService _ui;
        private readonly ISceneService _scene;
        
        public LobbyGameState(IUIService ui, ISceneService scene) {
            _ui = ui;
            _scene = scene;
        }

        public void Enter() {
            _scene.Load("Lobby", OnSceneLoaded);
        }
        
        private void OnSceneLoaded() {
            _ui.ShowLobbyHUD();
        }

        public void Exit() {
            _ui.Close<LobbyHUDView>();
        }
    }
}