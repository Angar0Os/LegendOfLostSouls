using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LOLS_SelectableCharacter : MonoBehaviour
{
    [SerializeField] private LOLS_Character mainCharacter;

    [Header("List of character card images")]
    [SerializeField] private List<Sprite> images = new List<Sprite>();
    public Image changeImage;

    [Header("List of dead character card images")]
    [SerializeField] private List<Sprite> deathImage = new List<Sprite>();

    [SerializeField] private List<bool> characterDead = new List<bool>();

    [SerializeField]
    private Button warriorButton;

    [SerializeField]
    private Button rogueButton;

    [SerializeField]
    private Button mageButton;

    private LOLS_Stats.S_stats stats;

    private Animator animator;

    public bool IsAnimating = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SelectCharacter(string characterName)
    {
        foreach (LOLS_Character.S_Character character in mainCharacter.Characters.ToList())
        {
            switch (characterName)
            {
                case "Warrior":
                    mainCharacter.CurrentCharacter = 0;
                    break;
                case "Rogue":
                    mainCharacter.CurrentCharacter = 1;
                    break;
                case "Mage":
                    mainCharacter.CurrentCharacter = 2;
                    break;
            }
        }

        mainCharacter.SetStatsUI();
        animator.SetTrigger("Hide");
    }

    public void SwitchCardImage()
    {
        changeImage.sprite = images[mainCharacter.CurrentCharacter];
    }

    public void CardDeath()
    {
        if (stats.Health <= 0)
        {
            characterDead[mainCharacter.CurrentCharacter] = true;
            ColorBlock colorBlock;
            switch (mainCharacter.CurrentCharacter)
            {
                case 0:
                    warriorButton.GetComponent<Image>().sprite = deathImage[mainCharacter.CurrentCharacter];
                    colorBlock = warriorButton.colors;
                    colorBlock.disabledColor = Color.white;
                    warriorButton.colors = colorBlock;
                    break;
                case 1:
                    rogueButton.GetComponent<Image>().sprite = deathImage[mainCharacter.CurrentCharacter];
                    colorBlock = rogueButton.colors;
                    colorBlock.disabledColor = Color.white;
                    rogueButton.colors = colorBlock;
                    break;
                case 2:
                    mageButton.GetComponent<Image>().sprite = deathImage[mainCharacter.CurrentCharacter];
                    colorBlock = mageButton.colors;
                    colorBlock.disabledColor = Color.white;
                    mageButton.colors = colorBlock;
                    break;
            }

            for (int i = 0; i < 2; i++)
            {
                mainCharacter.CurrentCharacter += 1;
                if (mainCharacter.CurrentCharacter >= 3)
                {
                    mainCharacter.CurrentCharacter = 0;
                }
                if (mainCharacter.Characters[mainCharacter.CurrentCharacter].Stats.Health > 0)
                {
                    break;
                }
            }

            if (mainCharacter.Characters[mainCharacter.CurrentCharacter].Stats.Health <= 0)
            {
                SceneManager.LoadScene("LOLS_GameOverMenu");
            }
            else
            {
                stats = mainCharacter.Characters[mainCharacter.CurrentCharacter].Stats;
                animator.SetTrigger("Hide");
            }
        }
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("SingleAttack");
    }

    private void Update()
    {
        LOLS_Character.S_Character s_Character = mainCharacter.Characters[mainCharacter.CurrentCharacter];
        stats = s_Character.Stats;

        if (IsAnimating)
        {
            warriorButton.interactable = false;
            rogueButton.interactable = false;
            mageButton.interactable = false;
        }
        else
        {
            if (!characterDead[0])
            {
                warriorButton.interactable = true;
            }
            if (!characterDead[1])
            {
                rogueButton.interactable = true;
            }
            if (!characterDead[2])
            {
                mageButton.interactable = true;
            }
        }
        switch (mainCharacter.CurrentCharacter)
        {
            case 0:
                warriorButton.interactable = false;
                break;
            case 1:
                rogueButton.interactable = false;
                break;
            case 2:
                mageButton.interactable = false;
                break;
        }
        CardDeath();
    }

}