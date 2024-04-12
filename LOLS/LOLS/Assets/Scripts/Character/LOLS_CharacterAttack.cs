using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_CharacterAttack : MonoBehaviour
{
    private LOLS_Character character;
    private LOLS_Stats.S_stats characterStats;
    private LOLS_MoveCharacter characterMovement;
    void Awake()
    {
        character = GetComponent<LOLS_Character>();
        characterStats = character.Character.Stats;
        characterMovement = GetComponent<LOLS_MoveCharacter>();
    }

    public void TriggerSimpleAttack()
    {
        StopCoroutine("SimpleAttackCooldown");
        StartCoroutine("SimpleAttackCooldown");
    }

    private IEnumerator SimpleAttackCooldown()
    {
        SimpleAttack();
        yield return new WaitForSeconds(characterStats.AttackSpeed);
    }

    private void SimpleAttack()
    {
        
      //  character.currentGrid.CheckCellObject(characterMovement.GetFrontCellGrid()).GetComponent<>
    }
}
