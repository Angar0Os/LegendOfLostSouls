using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LOLS_SelectableCharacter : MonoBehaviour
{
    private LOLS_Character mainCharacter;
    [Header("List of each Character stats")]
    public List<LOLS_Stats.S_stats> StatsList = new List<LOLS_Stats.S_stats>();

    [Header("List of character card images")]
    [SerializeField] private List<Sprite> images = new List<Sprite>();
    public Image changeImage;
    
    private LOLS_Stats.S_stats stats;
    private Animator animator;

    private void Awake()
    {
        mainCharacter = FindAnyObjectByType<LOLS_Character>();
        animator = GetComponent<Animator>();

        //Player guerrier first selected
        stats = StatsList[0];
        mainCharacter.Character.Stats = stats;
    }

    //--Button--//
    public void SelectCharacter_1()
    {
        mainCharacter.Character.Playerclass = LOLS_Character.E_characterClasses.Guerrier;
        stats = StatsList[0];
        mainCharacter.Character.Stats = stats;

        animator.SetTrigger("Hide");
        //Set the image to the Guerrier One
        changeImage.sprite = images[0];
    }
    public void SelectCharacter_2()
    {
        mainCharacter.Character.Playerclass = LOLS_Character.E_characterClasses.Mage;
        stats = StatsList[1];
        mainCharacter.Character.Stats = stats;

        animator.SetTrigger("Hide");
        //Set the image to the mage one
        changeImage.sprite = images[1];
    }
    public void SelectCharacter_3()
    {
        mainCharacter.Character.Playerclass = LOLS_Character.E_characterClasses.Assassin;
        stats = StatsList[2];
        mainCharacter.Character.Stats = stats;

        animator.SetTrigger("Hide");
        //set the image to the assassin one
        changeImage.sprite = images[2];
    }
}
