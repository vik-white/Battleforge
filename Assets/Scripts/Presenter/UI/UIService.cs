using UnityEngine;
using vikwhite.Data;
using vikwhite.View;

namespace vikwhite.Presenter
{
    public interface IUIService : View.IUIService
    {
        MainMenuWindowView ShowMainMenu();
        BattleHUDView ShowBattleHUD();
        LobbyHUDView ShowLobbyHUD();
        void ShowVictoryWindow();
        void ShowDefeatWindow();
        SquadWindowView ShowSquadWindow();
        CharacterWindowView ShowCharacterWindow(ICharacterData data);
    }

    public class UIService : View.UIService, IUIService
    {
        private readonly IConfigs _configs;
        private readonly IGameStateMachine _game;
        private readonly IPresenterFactory _presenter;
        private readonly IBattle _battle;
        private readonly IPlayerService _player;

        public UIService(IUIRoot uiRoot, IConfigs configs, IMouseService mouse, IViewFactory viewFactory, IGameStateMachine game, IPresenterFactory presenter, IBattle battle, IPlayerService player) : base(uiRoot, mouse, viewFactory) {
            _configs = configs;
            _game = game;
            _presenter = presenter;
            _battle = battle;
            _player = player;
        }

        public MainMenuWindowView ShowMainMenu() {
            MainMenuWindowView prefab = _configs.UI.Get<MainMenuWindowView>(UIKey.MainMenuWindow);
            MainMenuWindowView view = CreateWindow(prefab);
            view.OnStartGame += () => _game.SwitchState(GameState.Load);
            _mouse.ShowCursor();
            return view;
        }
        
        public BattleHUDView ShowBattleHUD() {
            BattleHUDView prefab = _configs.UI.Get<BattleHUDView>(UIKey.BattleHUD);
            BattleHUDView view = CreateWindow(prefab);
            _presenter.Create<BattleHUDPresenter>().Bind(_battle, view);
            return view;
        }

        public LobbyHUDView ShowLobbyHUD() {
            LobbyHUDView prefab = _configs.UI.Get<LobbyHUDView>(UIKey.LobbyHUD);
            LobbyHUDView view = CreateWindow(prefab);
            view.Initialize(() => ShowSquadWindow());
            return view;
        }
        
        public void ShowVictoryWindow() {
            VictoryWindowView prefab = _configs.UI.Get<VictoryWindowView>(UIKey.VictoryWindow);
            VictoryWindowView view = CreateWindow(prefab);
            view.Initialize(() => {
                Close<VictoryWindowView>();
                _game.SwitchState(GameState.Lobby);
            });
        }
        
        public void ShowDefeatWindow() {
            DefeatWindowView prefab = _configs.UI.Get<DefeatWindowView>(UIKey.DefeatWindow);
            DefeatWindowView view = CreateWindow(prefab);
            view.Initialize(() =>
            {
                Close<DefeatWindowView>();
                _game.SwitchState(GameState.Lobby);
            });
        }

        public SquadWindowView ShowSquadWindow() {
            SquadWindowView prefab = _configs.UI.Get<SquadWindowView>(UIKey.SquadWindow);
            SquadWindowView view = CreateWindow(prefab);
            view.OnRemoveDeckCard = _player.RemoveDeckCard;
            view.OnSetDeckCard = _player.SetDeckCard;
            view.OnCardSelected = (d) => ShowCharacterWindow(d);
            view.Initialize(_player.GetCards(), _player.GetDeck());
            return view;
        }
        
        public CharacterWindowView ShowCharacterWindow(ICharacterData data) {
            CharacterWindowView prefab = _configs.UI.Get<CharacterWindowView>(UIKey.CharacterWindow);
            CharacterWindowView view = CreateWindow(prefab);
            view.Initialize(data);
            return view;
        }
    }
}