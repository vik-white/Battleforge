using System;
using UnityEngine;

public class MeleeDamageReturnAbility : Ability
{
    private readonly IBattle _battle;
    
    public new MeleeDamageReturnAbilityData Data => _data as MeleeDamageReturnAbilityData;
    
    public MeleeDamageReturnAbility(IBattle battle) {
        _battle = battle;
    }
    
    public override void Initialize(Character character) {
        character.OnTakeDamage += (type, callback) => {
            if (type == DamageType.Attack) {
                var target = _battle.CurrentSide.CurrentCharacter;
                if(target.Type == CharacterType.Melee) 
                    target.Commands.AddNext(new TakeDamageCommand(Data.Damage, DamageType.Return));
            }
        };
    }
}