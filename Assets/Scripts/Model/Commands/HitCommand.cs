using UnityEngine;

public class HitCommand : Command
{
    public override void Execute(IBattle battle, Character character) {
        var targetCharacter = battle.WaitingSide.Board.GetFirstCharacterInRow(character.LocalPosition.y);
        var targetPosition = new Vector2Int(targetCharacter?.LocalPosition.x ?? 3, character.LocalPosition.y);
        var target = (IDamageable)targetCharacter ?? battle.WaitingSide.Hero;
        character.Hit(target, targetPosition, OnCompleted);
    } 
}