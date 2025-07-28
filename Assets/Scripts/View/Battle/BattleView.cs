using System;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    public Action OnRemove;
    
    [SerializeField] private GameObject _rangePointsContainer;
    [SerializeField] private GameObject _meleePointsContainer;

    private void Awake() {
        HideRangePoints();
        HideMeleePoints();
    }

    public void ShowRangePoints() => _rangePointsContainer.SetActive(true);
    
    public void HideRangePoints() => _rangePointsContainer.SetActive(false);
    
    public void ShowMeleePoints() => _meleePointsContainer.SetActive(true);
    
    public void HideMeleePoints() => _meleePointsContainer.SetActive(false);
    
    private void OnDestroy() => OnRemove?.Invoke();
}