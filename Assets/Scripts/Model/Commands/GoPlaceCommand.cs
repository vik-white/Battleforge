public class GoPlaceCommand : Command
{
    public override void Execute(IBattle battle, Character character) {
        character.OnGoPlace?.Invoke(OnCompleted);
    } 
}