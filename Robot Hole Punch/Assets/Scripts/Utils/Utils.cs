using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool TryGetComponent<T>(this Component comp, out T component)
    {
        component = comp.GetComponent<T>();
        return component != null;
    }
}
