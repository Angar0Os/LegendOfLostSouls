using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Dice : MonoBehaviour
{
    public int DiceRoll()
    {
        return Random.Range(1, 21);
    }
}
