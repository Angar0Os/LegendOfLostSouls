using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LOLS_LevelingSysteme : MonoBehaviour
{
    private LOLS_Character mainCharacter;
    private LOLS_UI_StatsRuntime UI_StatsRuntime;

    [SerializeField] private GameObject cardRef;

    private void Awake()
    {
        mainCharacter = this.gameObject.GetComponent<LOLS_Character>();
        UI_StatsRuntime = GameObject.FindGameObjectWithTag("UI_Selectable").GetComponent<LOLS_UI_StatsRuntime>();
    }

    void Update()
    {
        CheckForLevel();
    }

    private void CheckForLevel()
    {
        foreach (LOLS_Character.S_Character character in mainCharacter.Characters.ToList())
        {
            if(character.Stats.Level == 5)
                break;
                
            if (character.Stats.Level >= 4)
            {
                if (character.Stats.CurrentXP >= 10)
                {
                    ChangeStats(character, character.Stats.Level < 4 ? 5 : 10);
                }
            }
            else
            {
                if (character.Stats.CurrentXP >= 5)
                {
                    ChangeStats(character, character.Stats.Level < 4 ? 5 : 10);
                }
            }
        }
    }

    private void ChangeStats(LOLS_Character.S_Character character, int xpNeeded)
    {
        character.Stats.Level += 1;
        character.Stats.CurrentXP -= xpNeeded;

        if (character.Playerclass == LOLS_Character.E_characterClasses.Guerrier)
        {
            character.Stats.MaxHealth += 2;
            character.Stats.Damage += 1;
        }
        else
        {
            character.Stats.MaxHealth += 1;
            character.Stats.Damage += 2;
        }

        character.Stats.Health = character.Stats.MaxHealth;
        mainCharacter.Characters[mainCharacter.CurrentCharacter] = character;

        UI_StatsRuntime.SetCardHealthValue(character.Stats.Health, cardRef.gameObject);
        UI_StatsRuntime.SetCardDamagesValue(character.Stats.Damage, cardRef.gameObject);
        UI_StatsRuntime.SetCardLevel(character.Stats.Level, cardRef.gameObject);

    }
}
