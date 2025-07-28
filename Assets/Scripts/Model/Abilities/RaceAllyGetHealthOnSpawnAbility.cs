public class RaceAllyGetHealthOnSpawnAbility : Ability
{
    private readonly IBattle _battle;
    
    public new RaceAllyGetHealthOnSpawnAbilityData Data => _data as RaceAllyGetHealthOnSpawnAbilityData;
    
    public RaceAllyGetHealthOnSpawnAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        foreach (var c in _battle.CurrentSide.Board.GetCharacters().FindAll(e => e.Race == Data.Race && e != character)) {
            c.SetHealth(c.Health + Data.Health);
        }
    }
}