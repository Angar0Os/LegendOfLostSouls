using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Enemy : MonoBehaviour
{
    public enum E_typeEnemy
    {
        Gobelin,
        Roi_Gobelins,
        Mannequin
    };

    [System.Serializable]
    public struct S_Enemy
    {
        public E_typeEnemy TypeEnemy;
        public LOLS_Stats.S_stats Stats;
    }

    public S_Enemy Enemy;
}
