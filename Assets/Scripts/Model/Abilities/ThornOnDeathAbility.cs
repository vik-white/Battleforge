public class ThornOnDeathAbility : Ability
{
    private readonly IBattle _battle;
    private readonly IGameFactory _factory;
    
    public new ThornOnDeathAbilityData Data => _data as ThornOnDeathAbilityData;
    
    public ThornOnDeathAbility(IBattle battle, IGameFactory factory) {
        _battle = battle;
        _factory = factory;
    }
    
    public override void Initialize(Character character) {
        character.OnDie += () => {
            var board = _battle.GetBattleSide(character.Side).Board;
            board.Add(_factory.CreateCharacter("Thorns", character.LocalPosition, character.Side));
        };
    }
}