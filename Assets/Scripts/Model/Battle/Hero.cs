using System;

public class Hero : IDamageable
{
    public Action<float> OnHealthChanged;
    public Action OnDie;
    
    private float _health;

    public float Health => _health;
    public bool IsDie => _health <= 0;

    public void Initialize(float health) => _health = health;
    
    public void TakeDamage(float damage, DamageType type, bool isIgnoreBlock, Action callback) {
        SetHealth(_health - damage);
        if(_health <= 0) Die();
    }

    private void SetHealth(float health) {
        if (_health != health) {
            _health = health;
            OnHealthChanged?.Invoke(_health);
        }
    }

    private void Die() {
        OnDie?.Invoke();
    }
}