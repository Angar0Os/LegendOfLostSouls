using UnityEngine;

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
        public int CurrentXP;
        public int Level;
    }
}
