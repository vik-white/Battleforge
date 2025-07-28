using Zenject;

public interface IAbilityFactory
{
    Ability CreateAbility(AbilityData data);
}

public class AbilityFactory : IAbilityFactory
{
    private readonly DiContainer _container;
    
    public AbilityFactory(DiContainer container) {
        _container = container;
    }

    public Ability CreateAbility(AbilityData data) {
        return data switch {
            MultipleHitAbilityData d => _container.Resolve<MultipleHitAbility>().Initialize(d),
            MeleeDamageReturnAbilityData d => _container.Resolve<MeleeDamageReturnAbility>().Initialize(d),
            RaceAllyGetHealthOnSpawnAbilityData d => _container.Resolve<RaceAllyGetHealthOnSpawnAbility>().Initialize(d),
            SelfSacrificeHealerAbilityData d => _container.Resolve<SelfSacrificeHealerAbility>().Initialize(d),
            ThornOnDeathAbilityData d => _container.Resolve<ThornOnDeathAbility>().Initialize(d),
            RandomlySummonOnSpawnAbilityData d => _container.Resolve<RandomlySummonOnSpawnAbility>().Initialize(d),
            BlockAbilityData d => _container.Resolve<BlockAbility>().Initialize(d),
            IgnoreBlockAbilityData d => _container.Resolve<IgnoreBlockAbility>().Initialize(d),
            CounterAttackAbilityData d => _container.Resolve<CounterAttackAbility>().Initialize(d),
            RandomlyHealsAllyAbilityData d => _container.Resolve<RandomlyHealsAllyAbility>().Initialize(d),
            RandomlySummonOnTakeDamageAbilityData d => _container.Resolve<RandomlySummonOnTakeDamageAbility>().Initialize(d),
            DamageIncreaseOnTakeDamageAbilityData d => _container.Resolve<DamageIncreaseOnTakeDamageAbility>().Initialize(d),
            RaceAllyGetDamageOnSpawnAbilityData d => _container.Resolve<RaceAllyGetDamageOnSpawnAbility>().Initialize(d),
            VampirismAbilityData d => _container.Resolve<VampirismAbility>().Initialize(d),
            PhoenixRebornAbilityData d => _container.Resolve<PhoenixRebornAbility>().Initialize(d),
            _ => _container.Resolve<Ability>().Initialize(data)
        };
    }
}