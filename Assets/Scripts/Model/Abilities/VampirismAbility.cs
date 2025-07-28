public class VampirismAbility : Ability
{
    public new VampirismAbilityData Data => _data as VampirismAbilityData;

    public override void Initialize(Character character) {
        character.OnApplyDamage += () => {
            character.SetHealth(character.Health + Data.Health);
        };
    }
}