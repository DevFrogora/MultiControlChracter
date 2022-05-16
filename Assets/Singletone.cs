using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : Component
{

    public static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                obj.hideFlags = HideFlags.HideAndDontSave;
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }
}
