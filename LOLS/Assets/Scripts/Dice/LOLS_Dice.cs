using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Dice : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public int DiceRollLockpick()
    {
        LOLS_Character mainCharacter = player.GetComponent<LOLS_Character>();
        LOLS_Character.S_Character mainCharacterStats = mainCharacter.Characters[mainCharacter.CurrentCharacter];

        int resultDice = UnityEngine.Random.Range(1, 21) + mainCharacterStats.Stats.Level;
        if (player.GetComponent<LOLS_PassiveSkills>().LockPickingBoost())
        {
            resultDice = (int) (1.5f * (float) resultDice);
        }
        if (resultDice < 1)
        {
            resultDice = 1;
        }
        else if (resultDice > 20) 
        {
            resultDice = 20;
        }
        return resultDice;
    }

    public int DiceRoll()
    {
        LOLS_Character mainCharacter = player.GetComponent<LOLS_Character>();
        LOLS_Character.S_Character mainCharacterStats = mainCharacter.Characters[mainCharacter.CurrentCharacter];

        int resultDice = UnityEngine.Random.Range(1, 21) + mainCharacterStats.Stats.Level;
        if (resultDice < 1)
        {
            resultDice = 1;
        }
        else if (resultDice > 20)
        {
            resultDice = 20;
        }
        return resultDice;
    }
}
