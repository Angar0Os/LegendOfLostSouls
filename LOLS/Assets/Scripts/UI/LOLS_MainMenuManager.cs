using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LOLS_MainMenuManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LOLS_MainScene");
    }

    public void LoadControlsMenu()
    {
        SceneManager.LoadScene("LOLS_ControlsMenu");
    }

    public void LoadRulesMenuCharacter()
    {
        SceneManager.LoadScene("LOLS_RulesMenuCharacter");
    }

    public void LoadRulesMenuGame()
    {
        SceneManager.LoadScene("LOLS_RulesMenuGame");
    }

    public void LoadRulesMenuEnnemis()
    {
        SceneManager.LoadScene("LOLS_RulesMenuEnnemis");
    }

    public void LoadRulesMenuPassiveSkill()
    {
        SceneManager.LoadScene("LOLS_RulesMenuPassiveSkill");
    }

    public void LoadCreditsMenu()
    {
        SceneManager.LoadScene("LOLS_CreditScene");
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
