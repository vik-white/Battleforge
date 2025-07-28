public class MultipleHitAbility : Ability
{
    public new MultipleHitAbilityData Data => _data as MultipleHitAbilityData;

    public override void Initialize(Character character) {
        character.OnStartAttack += () => {
            for (int i = 0; i < Data.Count - 1; i++)
                character.Commands.AddNext(new HitCommand());
        };
    }
}