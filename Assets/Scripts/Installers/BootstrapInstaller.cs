using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace vikwhite
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings() {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize() {
            Container.Resolve<IGameStateMachine>().SwitchState(GameState.Bootstrap);
        }
    }
}