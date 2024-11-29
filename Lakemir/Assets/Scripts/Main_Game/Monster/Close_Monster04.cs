using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Monster04 : Monster
{
    [Header("ºÐ¿­Ã¼ prefabs")]
    [SerializeField] GameObject miniMonster;
    [SerializeField] Vector3 right;
    [SerializeField] Vector3 left;


    protected override void ToDestoy()
    {
        Quaternion rotation = Quaternion.identity;
        Instantiate(miniMonster, transform.position + right ,rotation);
        Instantiate(miniMonster, transform.position + left, rotation);
        base.ToDestoy();
    }
}
