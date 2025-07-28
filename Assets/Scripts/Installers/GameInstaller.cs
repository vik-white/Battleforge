using UnityEngine;
using vikwhite.Presenter;
using vikwhite.View;
using Zenject;

namespace vikwhite
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "VikWhite/Installer/GameInstaller", order = 1)]
    public class GameInstaller : ScriptableObjectInstaller 
    {
        public Configs Configs;

        public override void InstallBindings() {
            Container.BindInterfacesTo<Configs>().FromInstance(Configs).AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            
            Container.BindInterfacesTo<GameStateFactory>().AsSingle();
            Container.BindInterfacesTo<AbilityFactory>().AsSingle();
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<BootstrapGameState>().AsSingle();
            Container.BindInterfacesTo<LoadGameState>().AsSingle();
            Container.BindInterfacesTo<MenuGameState>().AsSingle();
            Container.BindInterfacesTo<BattleGameState>().AsSingle();
            Container.BindInterfacesTo<LobbyGameState>().AsSingle();
            Container.BindInterfacesTo<Battle>().AsSingle();
            Container.BindInterfacesTo<PlayerService>().AsSingle();
            
            Container.BindInterfacesTo<CameraService>().AsSingle();
            Container.BindInterfacesTo<MouseService>().AsSingle();
            Container.BindInterfacesTo<SceneService>().AsSingle();
            
            Container.BindInterfacesTo<PresenterFactory>().AsSingle();
            Container.Bind<BattlePresenter>().AsTransient();
            Container.Bind<CharacterPresenter>().AsTransient();
            Container.Bind<BattleHUDPresenter>().AsTransient();
            
            Container.BindInterfacesTo<ViewFactory>().AsSingle();
            
            Container.BindInterfacesTo<UIRoot>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.BindInterfacesTo<Presenter.UIService>().AsSingle();
            Container.Bind<SquadWindowView>().AsTransient();
            Container.Bind<CharacterWindowView>().AsTransient();
            Container.Bind<BattleCardView>().AsTransient();
            Container.Bind<CardView>().AsTransient();
            Container.Bind<CharacterView>().AsTransient();
            
            Container.Bind<BattleSide>().AsTransient();
            Container.Bind<BattleAI>().AsTransient();
            Container.Bind<Board>().AsTransient();
            Container.Bind<BattlePlayer>().AsTransient();
            Container.Bind<Hero>().AsTransient();
            Container.Bind<Character>().AsTransient();
            Container.Bind<AbilitiesHandler>().AsTransient();
            Container.Bind<CommandsHandler>().AsTransient();
            
            Container.Bind<MultipleHitAbility>().AsTransient();
            Container.Bind<MeleeDamageReturnAbility>().AsTransient();
            Container.Bind<RaceAllyGetHealthOnSpawnAbility>().AsTransient();
            Container.Bind<SelfSacrificeHealerAbility>().AsTransient();
            Container.Bind<ThornOnDeathAbility>().AsTransient();
            Container.Bind<RandomlySummonOnSpawnAbility>().AsTransient();
            Container.Bind<BlockAbility>().AsTransient();
            Container.Bind<IgnoreBlockAbility>().AsTransient();
            Container.Bind<CounterAttackAbility>().AsTransient();
            Container.Bind<RandomlyHealsAllyAbility>().AsTransient();
            Container.Bind<RandomlySummonOnTakeDamageAbility>().AsTransient();
            Container.Bind<DamageIncreaseOnTakeDamageAbility>().AsTransient();
            Container.Bind<RaceAllyGetDamageOnSpawnAbility>().AsTransient();
            Container.Bind<VampirismAbility>().AsTransient();
            Container.Bind<PhoenixRebornAbility>().AsTransient();
        }
    }
}