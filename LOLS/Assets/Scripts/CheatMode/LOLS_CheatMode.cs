using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class LOLS_CheatMode : MonoBehaviour
{
    public InputField InputBox;
    public GameObject Ui_In_Game;
    LOLS_CommandRegisterer cheatConsole = new LOLS_CommandRegisterer();
    public enum E_cheatModeState
    {
        CheatModeEnabled,
        CheatModeDisabled
    }

    private void Start()
    {
        cheatConsole.RegisterCommand(new Command_CheatMode(this.gameObject));
    }

    public E_cheatModeState CurrentGameState;
    public void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            InputBox.gameObject.SetActive(true);
            InputBox.ActivateInputField();
            Ui_In_Game.SetActive(false);
            Time.timeScale = 0;
        }
        if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            InputBox.text = "";
            InputBox.gameObject.SetActive(false);
            Ui_In_Game.SetActive(true);
            Time.timeScale = 1;
        }
    }
    public void OnValueSubmit()
    {
        string textInput = InputBox.text;
        if(!cheatConsole.Parse(textInput))
            Debug.Log("Unknown command: " + textInput);

        InputBox.text = "";
    }
}


