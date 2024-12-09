using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkillID03 : Skill
{
    public string prefabAddress1 = "Assets/Prefabs/Skill/SKillID03.prefab";
    GameObject prefabSkill;
    public SkillID03() : base("부식한 안개", "어둠", "몬스터들에게 약화 상태를 부여한다.", 6) { }

    public override void SkillEffect()
    {
        Addressables.LoadAssetAsync<GameObject>(prefabAddress1).Completed += handle1 =>
        {
            if(handle1.Status == AsyncOperationStatus.Succeeded)
            {
                prefabSkill = Object.Instantiate(handle1.Result, playerPos - new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
        };
    }
}
