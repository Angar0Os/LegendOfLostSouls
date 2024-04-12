using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_UIButtons : MonoBehaviour
{
    public GameObject Player;
    public GameObject Menus;
    public Button DiceButton;
    public bool DiceInterractable = false;

    public void PauseButton()
    {
        Menus.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Lockpickbutton()
    {
        int boost = 0;
        if (Player.GetComponent<LOLS_PassiveSkills>().LockPickingBoost())
        {
            boost = 10;
        }
        if (this.gameObject.GetComponent<LOLS_Dice>().DiceRoll() >= 15 - boost) //if the dice rolls more than 15 when any class and 5 when assassin
        {
            return;
        }
        else
        {
            return;
        }
    }

    private void Update()
    {
        DiceButton.interactable = DiceInterractable;
    }
}
