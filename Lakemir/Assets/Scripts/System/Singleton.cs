using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T _Instance;
    //싱글톤에 해당하는 오브젝트는 하나만 존재하도록 설정

    /// 제네릭 클래스 싱글톤 사용법
    /// 선언 : public class UIManager : Singleton<UIManager>
    /// 활용 : UIManager um = UIManager.Instance;

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



