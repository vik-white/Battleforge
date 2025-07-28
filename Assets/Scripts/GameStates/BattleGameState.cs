using vikwhite.View;
using UnityEngine;
using vikwhite.Presenter;
using IUIService = vikwhite.Presenter.IUIService;

namespace vikwhite
{
    public interface IBattleGameState : IState { }

    public class BattleGameState : IBattleGameState
    {
        private readonly IGameFactory _gameFactory;
        private readonly IUIService _ui;
        private readonly IBattle _battle;
        private readonly ICameraService _camera;
        private readonly ISceneService _scene;
        private readonly IPresenterFactory _presenter;
            
        public BattleGameState(IGameFactory  gameFactory, IUIService ui, IBattle battle, ICameraService camera, ISceneService scene, IPresenterFactory presenter) {
            _gameFactory = gameFactory;
            _ui = ui;
            _battle = battle;
            _camera = camera;
            _scene = scene;
            _presenter = presenter;
        }

        public void Enter() {
            _scene.Load("Battle", OnSceneLoaded);
        }
        
        private void OnSceneLoaded() {
            _camera.ApplyReferenceTransform();
            _presenter.Create<BattlePresenter>().Bind(_battle, GameObject.FindAnyObjectByType<BattleView>());
            _battle.Initialize(_gameFactory.CreateBattlePlayer(), _gameFactory.CreateBattlePlayer("Player2"));
            _ui.ShowBattleHUD();
            _battle.Start();
        }

        public void Exit() {
            _ui.Close<BattleHUDView>();
        }
    }
}