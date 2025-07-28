using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class CharacterView : MonoBehaviour
{
    public Action OnApplyDamage;

    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private Transform _image;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private SpriteRenderer _typeIcon;
    [SerializeField] private GameObject _typeContainer;
    private Vector3 _position;
    private Vector3 _target;
    private CharacterType _type;
    private Material _material;
    private Side _side;
    private IConfigs _configs;
    
    [Inject]
    public void Constract(IConfigs configs) {
        _configs = configs;
    }

    public void Initialize(Vector3 position, float health, float damage, CharacterType type) {
        _position = position;
        _type  = type;
        _health.text = health.ToString();
        _damage.text = damage.ToString();
        _side = _position.x > 2.5f ? Side.Right : Side.Left;
        _material = _image.GetComponentInChildren<MeshRenderer>().material;
        if(type != CharacterType.Wall) _typeIcon.sprite = _configs.UI.GetCharacterTypeIcons(type);
        if(_side == Side.Right) _image.localScale = new Vector3(-_image.localScale.x, _image.localScale.y, _image.localScale.z);
        _typeContainer.SetActive(type != CharacterType.Wall);
    }

    public void Hit(Vector2Int boardPosition, Action callback) {
        _target = BoardHandler.GlobalToWorldPosition(BoardHandler.BoardToGlobalPosition(boardPosition, _side == Side.Left ? Side.Right : Side.Left ));
        switch (_type) {
            case CharacterType.Melee: GoToTarget(() => MeleeAttack(callback)); break;
            case CharacterType.Range: RageAttack(callback); break;
        }
    }

    private void GoToTarget(Action callback) {
        var worldPosition = _target - (_side == Side.Right ? Vector3.left : Vector3.right);
        transform.DOMove(worldPosition, GetMoveDuration(worldPosition)).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke());
    }
    
    public void GoPlace(Action callback) {
        switch (_type) {
            case CharacterType.Melee: transform.DOMove(_position, GetMoveDuration(_position)).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke()); break;
            case CharacterType.Range: callback?.Invoke(); break;
        }
    }
    
    private void MeleeAttack(Action callback) {
        OnApplyDamage?.Invoke();
        _image.DORotate(new Vector3(_image.eulerAngles.x, 0, _side == Side.Right ? 15 : -15), 0.1f).OnComplete(() => {
            _image.DORotate(new Vector3(_image.eulerAngles.x, 0, 0), 0.1f).OnComplete(() => callback?.Invoke());
        });
    }

    private void RageAttack(Action callback) {
        var bullet = GameObject.Instantiate(_bulletPrefab);
        var shift = new Vector3(0, 0.4f, 0);
        if (_side == Side.Right) bullet.transform.localScale = new Vector3(-1, 1, 1);
        bullet.transform.position = transform.position + shift;
        bullet.transform.DOMove(_target + shift, GetRageAttackDuration(_target)).SetEase(Ease.Linear).OnComplete(() => {
            OnApplyDamage?.Invoke();
            callback?.Invoke();
            GameObject.Destroy(bullet);
        });
    }
    
    private float GetMoveDuration(Vector3 position) => (position - transform.position).magnitude / 10;
    
    private float GetRageAttackDuration(Vector3 position) => (position - transform.position).magnitude / 14;

    public void TakeDamage(DamageType type, Action callback) {
        _material.DOColor(Color.red, 0.1f).OnComplete(() => _material.DOColor(Color.white, 0.1f));
        _image.DORotate(new Vector3(_image.eulerAngles.x, 0, _side == Side.Right ? -15 : 15), 0.1f).OnComplete(() => {
            _image.DORotate(new Vector3(_image.eulerAngles.x, 0, 0), 0.1f).OnComplete(() => callback?.Invoke());
        });
    }

    public void SetHealth(float health) => _health.text = health.ToString();
    
    public void SetDamage(float damage) => _damage.text = damage.ToString();

    public void Die() {
        DOTween.Kill(_image); 
        DOTween.Kill(transform);
        _material.DOColor(Color.red, 0.1f);
        _image.DOScaleY(0, 0.5f).OnComplete(() => GameObject.Destroy(gameObject));
    }
}