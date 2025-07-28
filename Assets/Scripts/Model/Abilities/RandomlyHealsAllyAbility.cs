public class RandomlyHealsAllyAbility : Ability
{
    private readonly IBattle _battle;
    
    public new RandomlyHealsAllyAbilityData Data => _data as RandomlyHealsAllyAbilityData;
    
    public RandomlyHealsAllyAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        character.OnStartAttack += () => { 
            var board = _battle.GetBattleSide(character.Side).Board;
            var characters = board.GetCharacters().FindAll(e => e != character && e.Health < e.Data.Health);
            if (characters.Count > 0) {
                characters.Shuffle();
                characters[0].SetHealth(characters[0].Health + Data.Health);
            }
        };
    }
}