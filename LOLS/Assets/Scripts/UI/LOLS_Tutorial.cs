using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_Tutorial : MonoBehaviour
{
    public Image BlackScreen;
    public Text IntroductionSentence;

    public GameObject TutorialMovements;
    public GameObject TutorialDoor;
    public GameObject TutorialEnemy;
    public GameObject TutorialChangeCharacter;
    public GameObject Hud_Selectable;

    public bool IntroEnded = false;
    public bool TutorialEnded = false;
    private bool DoorInFront = false;
    private bool EnemyInFront = false;
    private bool DoorTuto = false;
    private bool EnemyTuto = false;
    private bool ChangeCharacterTuto = false;
    private bool letsGo = false;
    private bool TutoChangementPerso = false;
    [SerializeField] private LOLS_MoveCharacter moveC;


    void Awake()
    {
        Invoke("Starting", 0.5f);
    }

    private void Starting()
    {
        letsGo = true;
    }

    private void Tuto2()
    {
        TutoChangementPerso = true;
    }
    private void Update()
    {
        if (letsGo)
        {
            EnemyInFront = moveC.CheckFrontObject();

            if (IntroEnded && !TutorialEnded)
            {
                BlackScreen.gameObject.SetActive(false);
                IntroductionSentence.gameObject.SetActive(false);
                TutorialMovements.SetActive(true);
                Hud_Selectable.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    TutorialMovements.SetActive(false);
                    TutorialEnded = true;
                }
            }

            if (ChangeCharacterTuto == false && TutorialEnded == true)
            {
                if(TutoChangementPerso==false){
                    Invoke("Tuto2", 0.5f);
                }
    
                TutorialChangeCharacter.SetActive(true);
                if (Input.GetMouseButtonDown(0) && TutoChangementPerso)
                {
                    TutorialChangeCharacter.SetActive(false);
                    ChangeCharacterTuto = true;
                }
            }

            if (DoorInFront == true && DoorTuto == false)
            {
                TutorialDoor.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    TutorialDoor.SetActive(false);
                    DoorTuto = true;
                }
            }

            if (EnemyInFront && EnemyTuto == false)
            {
                TutorialEnemy.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    TutorialEnemy.SetActive(false);
                    EnemyTuto = true;
                }
            }


        }

    }

    public void DoorFront()
    {
        DoorInFront = true;
    }

    public void EnemyFront()
    {
        EnemyInFront = true;
    }
}