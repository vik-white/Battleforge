using System;
using UnityEngine;

public enum CharacterType {Melee, Range, Wall}
public enum CharacterRace {City, Nature, Hell}
public enum CharacterRarity {Common, Rare, Epic, Legendary}

public class Character: IDamageable
{
    public Action OnSpawn;
    public Action OnStartAttack;
    public Action OnDie;
    public Action<float> OnHealthChanged;
    public Action<float> OnDamageChanged;
    public Action<Vector2Int, Action> OnHit;
    public Action OnApplyDamage;
    public Action<DamageType, Action> OnTakeDamage;
    public Action<Action> OnGoPlace;
    
    private ICharacterData _data;
    private Vector2Int _position;
    private Vector2Int _localPosition;
    private float _health;
    private float _damage;
    private CommandsHandler _commands;
    private AbilitiesHandler _abilities;
    private IDamageable _target;
    private Side _side;
    private readonly IGameFactory _factory;
    
    public ICharacterData Data => _data;
    public Vector2Int Position => _position;
    public Vector2Int LocalPosition => _localPosition;
    public float Health => _health;
    public float Damage => _damage;
    public CharacterType Type => _data.Type;
    public CharacterRace Race => _data.Race;
    public bool IsDie => _health <= 0;
    public Side Side => _side;
    public CommandsHandler Commands => _commands;
    
    public Character(IGameFactory factory) {
        _factory = factory;
    }

    public void Initialize(ICharacterData data, Vector2Int localPosition, Side side) {
        _data = data;
        _side  = side;
        _position = BoardHandler.BoardToGlobalPosition(localPosition, side);
        _localPosition = localPosition;
        _health = _data.Health;
        _damage =  _data.Damage;
        _commands = _factory.CreateCommandsHandler(this);
        _abilities = _factory.CreateAbilityHandler(this);
    }
    
    public void StartAttack() {
        _commands.Clear();
        if (Type == CharacterType.Melee || Type == CharacterType.Range) {
            _commands.Add(new HitCommand());
            _commands.Add(new GoPlaceCommand());
        }
        OnStartAttack?.Invoke();
    }

    public void Hit(IDamageable target, Vector2Int position, Action callback) {
        _target = target;
        OnHit?.Invoke(position, callback);
    }

    public void ApplyDamage() {
        _target.TakeDamage(Damage, DamageType.Attack, _abilities.IsIgnoreBlock, null);
        OnApplyDamage?.Invoke();
    }

    public void TakeDamage(float damage, DamageType type, bool isIgnoreBlock, Action callback) {
        if(!isIgnoreBlock) damage -= _abilities.Block;
        if (damage < 0) damage = 0;
        SetHealth(_health - damage);
        OnTakeDamage?.Invoke(type, callback);
        if(_health <= 0) Die();
    }

    public void SetHealth(float health) {
        if (_health != health) {
            _health = health;
            OnHealthChanged?.Invoke(_health);
        }
    }
    
    public void SetDamage(float damage) {
        if (_damage != damage) {
            _damage = damage;
            OnDamageChanged?.Invoke(_damage);
        }
    }

    public void Die() {
        SetHealth(0);
        _commands.Clear();
        OnDie?.Invoke();
    }
}