using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LOLS_MainMenuManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("LOLS_MainScene");
    }

    public void LoadControlsMenu()
    {
        SceneManager.LoadScene("LOLS_ControlsMenu");
    }

    public void LoadRulesMenu()
    {
        SceneManager.LoadScene("LOLS_RulesMenu");
    }

    public void QuitScene()
    {
        SceneManager.LoadScene("LOLS_MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
