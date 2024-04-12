using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.IO;
using System;

public class LOLS_CheatMode : MonoBehaviour
{
    public InputField InputBox;

    private Dictionary<string, int> commandList;
    List<string> commandsLines = new List<string>();

    private string path = "Assets/Ressources/Commands.txt";
    private E_cheatModeState CurrentGameState = E_cheatModeState.CheatModeDisabled;
    public enum E_cheatModeState
    {
        CheatModeActivated,
        CheatModeDisabled   
    };

    public void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            InputBox.gameObject.SetActive(true);
        }
        if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            InputBox.gameObject.SetActive(false);
        }
    }
    public void OnValueSubmit()
    {
        int index = ReturnCommandIndex();
        switch (index)
        {
            case 0:           
                SetCheatMode0();
                InputBox.gameObject.SetActive(false);
                break;
            case 1:
                SetCheatMode1();
                InputBox.gameObject.SetActive(false);
                break;
            default:
                InputBox.text = "";
                InputBox.gameObject.SetActive(false);
                break;

        }
    }

    public int ReturnCommandIndex()
    {
        StreamReader reader = new StreamReader(path);

        while (!reader.EndOfStream)
        {
            string inp_ln = reader.ReadLine();
            commandsLines.Add(inp_ln);
            string[] inp_ln_tb = inp_ln.Split('-');

            if(InputBox.text == inp_ln_tb[0])
            {
                return Int32.Parse(inp_ln_tb[1]);
            }
        }
        reader.Close();
        return -1;
    }

    public void SetCheatMode0()
    {
        CurrentGameState = E_cheatModeState.CheatModeDisabled;
        InputBox.text = "";
        Debug.Log("CheatMode is now Disabled");
        //Text Box print gamemode change 
    }
    
    public void SetCheatMode1()
    {
        CurrentGameState = E_cheatModeState.CheatModeActivated;
        InputBox.text = "";
        Debug.Log("CheatMode is now Enabled");
        //Text Box print gamemode change
    }
}
