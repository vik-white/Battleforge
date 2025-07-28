using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Utils
{
    
}


public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}

[Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    [HorizontalGroup("Pair")][HideLabel]
    public TKey Key;
    [HorizontalGroup("Pair")][HideLabel]
    public TValue Value;

    public SerializableKeyValuePair() { }
    public SerializableKeyValuePair(TKey key, TValue value) {
        Key = key;
        Value = value;
    }
}

public static class ColorUtils
{
    public static Color FromHex(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            Debug.LogError("Hex string is null or empty.");
            return Color.white;
        }

        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length != 6 && hex.Length != 8)
        {
            Debug.LogError("Hex string must be 6 or 8 characters long.");
            return Color.white;
        }

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = 255;

        if (hex.Length == 8)
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color32(r, g, b, a);
    }
}