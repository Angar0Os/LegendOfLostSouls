using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LOLS_PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject RulesMenuObject;
    public GameObject ControlsMenuObject;
    public GameObject UI_In_Game;

    public void Continue()
    {
        UI_In_Game.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void RulesMenu()
    {
        RulesMenuObject.SetActive(true);
        PauseMenuObject.SetActive(false);
    }

    public void ControlsMenu()
    {
        ControlsMenuObject.SetActive(true);
        PauseMenuObject.SetActive(false);
    }

    public void BackControlsMenu()
    {
        PauseMenuObject.SetActive(true);
        ControlsMenuObject.SetActive(false);
    }

    public void BackRulesMenu()
    {
        PauseMenuObject.SetActive(true);
        RulesMenuObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("LOLS_MainMenu");
        this.gameObject.SetActive(false);
    }
}
