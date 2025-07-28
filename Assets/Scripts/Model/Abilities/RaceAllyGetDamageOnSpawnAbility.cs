public class RaceAllyGetDamageOnSpawnAbility : Ability
{
    private readonly IBattle _battle;
    
    public new RaceAllyGetDamageOnSpawnAbilityData Data => _data as RaceAllyGetDamageOnSpawnAbilityData;
    
    public RaceAllyGetDamageOnSpawnAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        foreach (var c in _battle.CurrentSide.Board.GetCharacters().FindAll(e => e.Race == Data.Race && e != character)) {
            c.SetDamage(c.Damage + Data.Damage);
        }
    }
}