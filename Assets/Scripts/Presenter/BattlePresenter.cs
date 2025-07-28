using vikwhite.View;

namespace vikwhite.Presenter
{
    public class BattlePresenter : IPresenter
    {
        private readonly IViewFactory _viewFactory;
        private readonly IPresenterFactory _presenter;
        private readonly IUIService _ui;
        private IBattle _model;
        private BattleView _view;
        
        public BattlePresenter(IViewFactory viewFactory, IPresenterFactory presenter, IUIService ui) {
            _viewFactory = viewFactory;
            _presenter = presenter;
            _ui = ui;
        }

        public void Bind(IBattle model, BattleView view) {
            _model = model;
            _view = view;
            model.OnAddCharacter += CreateCharacterView;
            model.OnVictory += _ui.ShowVictoryWindow;
            model.OnDefeat += _ui.ShowDefeatWindow;
            view.OnRemove += Unbind;
        }

        private void Unbind() {
            _model.OnAddCharacter -= CreateCharacterView;
            _model.OnVictory -= _ui.ShowVictoryWindow;
            _model.OnDefeat -= _ui.ShowDefeatWindow;
            _view.OnRemove -= Unbind;
        }

        private void CreateCharacterView(Character character) {
            var worldPosition = BoardHandler.GlobalToWorldPosition(character.Position);
            var view = _viewFactory.CreateCharacter(character.Data, (int)character.Damage, (int)character.Health, worldPosition);
            _presenter.Create<CharacterPresenter>().Bind(character, view);
        }
    }
}