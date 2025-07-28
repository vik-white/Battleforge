public class CounterAttackAbility : Ability
{
    private readonly IBattle _battle;
    
    public new CounterAttackAbilityData Data => _data as CounterAttackAbilityData;
    
    public CounterAttackAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        character.OnTakeDamage += (type, callback) => {
            if (type == DamageType.Attack) {
                var target = _battle.CurrentSide.CurrentCharacter;
                character.Hit(target, target.LocalPosition, null);
            }
        };
    }
}