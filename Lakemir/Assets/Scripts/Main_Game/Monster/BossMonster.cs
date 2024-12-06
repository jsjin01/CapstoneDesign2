using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    void Update()
    {
        GetClosestPlayer();
    }
}
