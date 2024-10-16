using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<T>();

                if (_Instance == null)
                {
                    GameObject obj = new() { name = typeof(T).Name };

                    _Instance = obj.AddComponent<T>();
                }
            }

            return _Instance;
        }
    }
}



