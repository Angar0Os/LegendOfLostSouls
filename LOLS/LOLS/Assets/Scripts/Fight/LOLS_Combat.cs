using UnityEngine;

public class LOLS_Combat : MonoBehaviour
{
    //Get the Player character and statList of the character selection
    private LOLS_Character mainCharacter;
    private LOLS_SelectableCharacter characterSelection;

    private void Awake()
    {
        mainCharacter = FindAnyObjectByType<LOLS_Character>();
        characterSelection = FindAnyObjectByType<LOLS_SelectableCharacter>();
    }

    //Debug
    public void buttonDamage()
    {
        TakeDamage(2);
    }
    //

    private void TakeDamage(int damage)
    {
        //Check the character class and apply damage
        switch (mainCharacter.Character.Playerclass)
        {
            case LOLS_Character.E_characterClasses.Guerrier:
                {
                    LOLS_Stats.S_stats tempList = characterSelection.StatsList[0];
                    tempList.Health -= damage;

                    characterSelection.StatsList[0] = tempList;

                    mainCharacter.Character.Stats.Health = characterSelection.StatsList[0].Health;
                }
                break;
            case LOLS_Character.E_characterClasses.Mage:
                {
                    LOLS_Stats.S_stats tempList = characterSelection.StatsList[1];
                    tempList.Health -= damage;

                    characterSelection.StatsList[1] = tempList;

                    mainCharacter.Character.Stats.Health = characterSelection.StatsList[1].Health;
                }
                break;
            case LOLS_Character.E_characterClasses.Assassin:
                {
                    LOLS_Stats.S_stats tempList = characterSelection.StatsList[2];
                    tempList.Health -= damage;

                    characterSelection.StatsList[2] = tempList;

                    mainCharacter.Character.Stats.Health = characterSelection.StatsList[2].Health;
                }
                break;
        }
    }
}
