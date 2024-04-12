using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Rendering;

public class LOLS_Stats : MonoBehaviour
{
    [System.Serializable]
    public struct S_stats
    {
        public int AttackSpeed;
        public float Health;
        public int MaxHealth;
        public int Damage;
        public int Defense;
        public int Level;
    }

    private void Update()
    {
        if (this.gameObject.GetComponent<LOLS_Character>())
        {
            return;
        }
    }
}
