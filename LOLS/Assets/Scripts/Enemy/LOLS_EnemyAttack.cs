using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_EnemyAttack : MonoBehaviour
{

    private LOLS_Character character;
    private LOLS_Stats.S_stats enemyStats;
    private float lastTime = -Mathf.Infinity;
    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponentInParent<LOLS_Character>();
        enemyStats = GetComponent<LOLS_Enemy>().Enemy.Stats;
    }

    public void TriggerSimpleAttack()
    {
        if (Time.time - lastTime >= enemyStats.AttackSpeed)
        {
            SimpleAttack();
            lastTime = Time.time;
        }
        else
        {

        }
    }

    private void SimpleAttack()
    {
        if(character != null)
        {
            LOLS_Enemy myRef = GetComponent<LOLS_Enemy>();
            if(myRef.GetCardRef() != null)
            {
                LOLS_EnnemyCard cardRef = myRef.GetCardRef();
                cardRef.EnemyAttack();
            }
            switch (Random.Range(0, 2))
            {
                case 0: LOLS_SoundManager.Instance.PlaySound("gobPunch");
                break;
                case 1: LOLS_SoundManager.Instance.PlaySound("gobPunch2");
                break;
                case 2: LOLS_SoundManager.Instance.PlaySound("gobPunch3");
                break;
            }
            character.TakeDamages(enemyStats.Damage);
        }
    }
}
