using UnityEngine;

public class SelfSacrificeHealerAbility : Ability
{
    private readonly IBattle _battle;
    
    public new SelfSacrificeHealerAbilityData Data => _data as SelfSacrificeHealerAbilityData;
    
    public SelfSacrificeHealerAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        character.OnStartAttack += () => {
            bool isHeal = false;
            foreach (var c in _battle.CurrentSide.Board.GetCharacters()) {
                if (c != character && c.Health < c.Data.Health) {
                    c.SetHealth(c.Health + Data.Health);
                    isHeal = true;
                }
            }
            if(isHeal && character.Health > 1) character.SetHealth(character.Health - Data.Health);
        };
    }
}