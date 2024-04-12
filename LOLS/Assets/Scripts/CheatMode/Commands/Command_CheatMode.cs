using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

public class Command_CheatMode : LOLS_ICommand
{
    GameObject CheatMode;
    public Command_CheatMode(GameObject gO)
    {
        CheatMode = gO;
    }
    public void Execute(string[] args)
    {
        LOLS_Grid[] objectsWithScript = GameObject.FindObjectsOfType<LOLS_Grid>();
        if (args[0] == "0")
        {
            foreach (LOLS_Grid grid in objectsWithScript)
            {
                grid.ToggleVisibleGridDebug(false);
            }
            GameObject.FindWithTag("NoClip").GetComponent<LOLS_NoclipCamera>().DisableNoclip();
        }
          //  CheatMode.GetComponent<LOLS_CheatMode>().CurrentGameState = LOLS_CheatMode.E_cheatModeState.CheatModeDisabled;
        else
        {
            foreach (LOLS_Grid grid in objectsWithScript)
            {
                grid.ToggleVisibleGridDebug(true);
            }
            GameObject.FindWithTag("NoClip").GetComponent<LOLS_NoclipCamera>().EnableNoclip();
        }
           // CheatMode.GetComponent<LOLS_CheatMode>().CurrentGameState = LOLS_CheatMode.E_cheatModeState.CheatModeEnabled;
        Debug.Log(CheatMode.GetComponent<LOLS_CheatMode>().CurrentGameState);
    }

    public bool IsTagValid(string s)
    {
        return s == "cheatmode" || s == "cm";
    }
}
