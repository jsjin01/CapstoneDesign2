using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponID03 : CloseRangeWeapon
{
    float dmgIncrease = 0.1f;
    int killedMonster = 0;
    int maxMonster = 10;
    int comboNum = 26;

    public string prefabAddress1 = "Assets/Prefabs/Weapon Effect/WeaponID03/WeaponID03.prefab";                         // 첫 번째 프리팹 주소
    public string prefabAddress2 = "Assets/Prefabs/Weapon Effect/WeaponID03/WeaponID03 Strong Atk.prefab";             // 두 번째 프리팹 주소
    GameObject[] tripleParticle = new GameObject[2];
    public WeaponID03()
    {
        weaponName = "흑요석 망치";
        description = "어무거운 흑요석으로 만들어진 망치로, 강력한 광역 타격을 제공합니다.";
        ability = "지진 발동: 3타마다 큰 충격파를 만들어 다수의 몬스터를 공격\n\n" +
            "암흑의 폭발: 27타마다 충격파보다 훨씬 큰 폭발을 만들어 공격(데미지 150%)";
        w_type = WeaponEnum.WEAPON_TYPE.CLOSE_RANGE_WEAPON;
        comboNumber = 3;
        currentcomboNumber = 0;

        //tripleParticle = Prefabs.Load<GameObject>

        hitEffect = EFFECT.WEAKENING;
        comboDamage[0] = 0.7f * (1 + 0.1f * weaponGrade);
        comboDamage[1] = 0.8f * (1 + 0.1f * weaponGrade);
        comboDamage[2] = 1f * (1 + 0.1f * weaponGrade);

    }

    public override void selfEffects()
    {
        comboNum++;     //콤보넘++
        Debug.Log(comboNum);
        if(comboNum % 3 == 0 && comboNum % 27 != 0)
        {
            Debug.Log("3타 강화");
            Addressables.LoadAssetAsync<GameObject>(prefabAddress1).Completed += handle1 =>
            {
                if(handle1.Status == AsyncOperationStatus.Succeeded)
                {
                    tripleParticle[0] = Object.Instantiate(handle1.Result, playerHitPoint, Quaternion.identity);
                }
            };
        }
        else if(comboNum % 3 == 0 && comboNum % 27 == 0)
        {
            Debug.Log("27타 강화");
            Addressables.LoadAssetAsync<GameObject>(prefabAddress2).Completed += handle2 =>
            {
                if(handle2.Status == AsyncOperationStatus.Succeeded)
                {
                    tripleParticle[1] = Object.Instantiate(handle2.Result, playerHitPoint, Quaternion.identity);
                }
            };
        }
    }

   
}
