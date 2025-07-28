using System;

public enum DamageType { Attack, Return }

public interface IDamageable
{
    void TakeDamage(float damage, DamageType type, bool isIgnoreBlock, Action callback);
    bool IsDie { get; }
}