using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Character : MonoBehaviour
{
    public enum E_characterClasses
    {
        Guerrier,
        Mage,
        Assassin
    };


    [System.Serializable]
    public struct S_Character
    {
        public E_characterClasses Playerclass;
        public LOLS_Stats.S_stats Stats;

    }

    public S_Character Character;
}
