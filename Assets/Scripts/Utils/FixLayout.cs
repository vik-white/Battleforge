using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixLayout : MonoBehaviour
{
    void Update() => LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
}
