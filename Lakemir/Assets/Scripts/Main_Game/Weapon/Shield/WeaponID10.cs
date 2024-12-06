using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponID10 : Shield
{
    public string prefabAddress = "Assets/Prefabs/Weapon Effect/WeaponID10/HealingOra.prefab";
    GameObject shieldOra;
    public WeaponID10()
    {
        weaponName = "빛나는 새벽의 방패";
        description = "무거운 철제 철퇴로, 그 표면은 마치 눈물을 흘리는 듯한 패턴으로 장식되어 있다." +
            "이 철퇴는 사용할 때마다 주변을 침울하게 만든다.";
        ability = "퍼져 나가는 빛: 패링을 성공할 시 현재 체력의 10%에 해당하는 체력을 회복시키는 오라를 발생시킨다.\n " +
            "반격의 빛: 패링을 성공할 시 더 큰 데미지(150%)를 준다.";
        w_type = WeaponEnum.WEAPON_TYPE.SHIELD;
        reflect = 2.5f * (1 + 0.1f * weaponGrade);
        hitEffect = EFFECT.NONE;
    }

    public override void selfEffects()
    {
        Addressables.LoadAssetAsync<GameObject>(prefabAddress).Completed += handle1 =>
        {
            if(handle1.Status == AsyncOperationStatus.Succeeded)
            {
                shieldOra = Object.Instantiate(handle1.Result, playerHitPoint, Quaternion.identity);
            }
        };
    }

    public override void SetGrade(int grade)
    {
        base.SetGrade(grade);
        reflect = 2.5f * (1 + 0.1f * weaponGrade);
    }
}
