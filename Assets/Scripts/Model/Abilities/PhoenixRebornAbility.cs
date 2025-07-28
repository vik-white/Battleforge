using UnityEngine;

public class PhoenixRebornAbility : Ability
{
    private readonly IBattle _battle;
    private readonly IGameFactory _factory;
    
    public new PhoenixRebornAbilityData Data => _data as PhoenixRebornAbilityData;
    
    public PhoenixRebornAbility(IBattle battle, IGameFactory factory) {
        _battle = battle;
        _factory = factory;
    }

    public override void Initialize(Character character) {
        character.OnDie += () => {
            var board = _battle.GetBattleSide(character.Side).Board;
            character = _factory.CreateCharacter("Phoenix Reborn", character.LocalPosition, character.Side);
            character.SetHealth(character.Health / 2);
            character.SetDamage(character.Damage * 1.5f);
            board.Add(character);
        };
    }
}