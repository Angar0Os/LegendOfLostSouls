using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_UIButtons : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayer;
    public GameObject Player;
    public GameObject Menus;
    public Button DiceButton;
    public LOLS_TextBoxManager affichageText;
    private int resultDice;

    public void PauseButton()
    {
        GameObject.FindGameObjectWithTag("Tutoo").GetComponent<Canvas>().sortingOrder = -101;
        Menus.SetActive(true);
        this.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Lockpickbutton(LOLS_Door _door)
    {
        if(_door == null)
        {
            Debug.Log("door null");
        }
        if (_door.CheckIfLocked())
        {
            affichageText.RemoveMessages();

            affichageText.SendMessage("Le joueur essaie de déverouiller la porte...");

            resultDice = this.gameObject.GetComponent<LOLS_Dice>().DiceRollLockpick();
            affichageText.SendMessage("Vous lancez le dé et obtenez : " + resultDice);
            affichageText.SendMessage(" ");

            if (resultDice > 10)
            {
                affichageText.SendMessage("Réussite ! La porte s'ouvre.");
                _door.ToggleDoor();
               // Player.GetComponent<LOLS_PlayerController>().Crochetage();
            }
            else
            {
                affichageText.SendMessage("Echec... La chance n'est pas avec vous.");
            }
        }
        else
        {
            _door.ToggleDoor();
          //  Player.GetComponent<LOLS_PlayerController>().Crochetage();
        }
    }
    
    public void ToggleDiceButton()
    {
        DiceButton.interactable = true;
    }

    public void DisableDiceButton()
    {
        DiceButton.interactable = false;
    }
}
