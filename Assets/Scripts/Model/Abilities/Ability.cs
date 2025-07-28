public class Ability
{
    protected AbilityData _data;
        
    public AbilityData Data => _data;

    public Ability Initialize(AbilityData data) {
        _data = data;
        return this;
    }
    
    public virtual void Initialize(Character character) {}
}