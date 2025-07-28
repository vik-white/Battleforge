public class DamageIncreaseOnTakeDamageAbility : Ability
{
    public new DamageIncreaseOnTakeDamageAbilityData Data => _data as DamageIncreaseOnTakeDamageAbilityData;
    
    public override void Initialize(Character character) {
        character.OnTakeDamage += (type, callback) => {
            if (type == DamageType.Attack) {
                character.SetDamage(character.Damage + Data.Damage);
            }
        };
    }
}