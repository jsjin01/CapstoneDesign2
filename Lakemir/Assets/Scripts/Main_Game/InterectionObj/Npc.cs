using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interection;

public class Npc : MonoBehaviour
{
    public NPC npcType;
    public Sprite[] npcImg;
    [SerializeField] SpriteRenderer sr;

    private void OnEnable()
    {
        sr.sprite = npcImg[(int)npcType];
    }

}
