public class TakeDamageCommand : Command
{
    private float _damage;
    private DamageType _type;
    
    public TakeDamageCommand(float damage,  DamageType type) {
        _damage = damage;
        _type = type;
    }

    public override void Execute(IBattle battle, Character character) {
        character.TakeDamage(_damage, _type, false, OnCompleted);
    } 
}