using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkillID05 : Skill
{
    public string prefabAddress1 = "Assets/Prefabs/Skill/SKillID05.prefab";
    GameObject prefabSkill;
    public SkillID05() : base("�ڿ��� ��ȣ", "���", "�÷��̾��� ü���� ȸ����Ų��.", 15) { }

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
