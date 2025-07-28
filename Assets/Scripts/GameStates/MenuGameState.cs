using UnityEngine;
using vikwhite.View;

namespace vikwhite
{
    public interface IMenuGameState : IState { }

    public class MenuGameState : IMenuGameState
    {
        private readonly Presenter.IUIService _ui;
        private readonly ICameraService _camera;
        private readonly IUIRoot _root;

        public MenuGameState(Presenter.IUIService ui, ICameraService camera, IUIRoot root) {
            _camera = camera;
            _ui = ui;
            _root = root;
        }

        public void Enter() {
            _camera.Create();
            _root.Create();
            _ui.ShowMainMenu();
        }

        public void Exit() {
            _ui.Close<MainMenuWindowView>();
        }
    }
}