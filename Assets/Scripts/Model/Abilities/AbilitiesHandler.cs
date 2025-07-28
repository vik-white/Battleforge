using System.Collections.Generic;

public class AbilitiesHandler
{
    private List<Ability> _abilities;
    private readonly IAbilityFactory _factory;

    public int Block { get; private set; }
    public bool IsIgnoreBlock { get; private set; }
    
    public AbilitiesHandler(IAbilityFactory factory) {
        _factory = factory;
    }

    public void Initialize(Character character) {
        _abilities = new List<Ability>();
        foreach (var abilityData in character.Data.Abilities) {
            var ability = _factory.CreateAbility(abilityData);
            ability.Initialize(character);
            _abilities.Add(ability);
        }
        Recalculate();
    }
    
    public void Recalculate() {
        ResetAbilities();
        foreach (var ability in _abilities) {
            RecalculateAbility(ability);
        }
    }

    private void ResetAbilities() {
        Block = 0;
        IsIgnoreBlock = false;
    }

    private void RecalculateAbility(Ability ability) {
        switch (ability) {
            case BlockAbility a : Block += a.Data.Block; break;
            case IgnoreBlockAbility a : IsIgnoreBlock = true; break;
        }
    }
}