using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LOLS_PauseMenuManager : MonoBehaviour
{
    public GameObject PauseMenuObject;
    public GameObject RulesMenuGameObject;
    public GameObject RulesMenuCharacterObject;
    public GameObject RulesMenuEnnemisObject;
    public GameObject RulesMenuPassiveSkillObject;
    public GameObject ControlsMenuObject;
    public GameObject UI_In_Game;

    public void Continue()
    {
        UI_In_Game.SetActive(true);
        this.gameObject.SetActive(false);
        
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Tutoo").GetComponent<Canvas>().sortingOrder = 101;
    }

    public void RulesMenuGame()
    {
        RulesMenuGameObject.SetActive(true);
        PauseMenuObject.SetActive(false);
        RulesMenuCharacterObject.SetActive(false);
    }

    public void RulesMenuCharacter()
    {
        RulesMenuCharacterObject.SetActive(true);
        RulesMenuGameObject.SetActive(false);
        RulesMenuEnnemisObject.SetActive(false);
    }

    public void RulesMenuEnnemis()
    {
        RulesMenuEnnemisObject.SetActive(true);
        RulesMenuCharacterObject.SetActive(false);
        RulesMenuPassiveSkillObject.SetActive(false);
    }

    public void RulesMenuPassiveSkill()
    {
        RulesMenuPassiveSkillObject.SetActive(true);
        RulesMenuEnnemisObject.SetActive(false);
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
        RulesMenuCharacterObject.SetActive(false);
        RulesMenuGameObject.SetActive(false);
        RulesMenuEnnemisObject.SetActive(false);
        RulesMenuPassiveSkillObject.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("LOLS_MainMenu");
        this.gameObject.SetActive(false);
    }
}
