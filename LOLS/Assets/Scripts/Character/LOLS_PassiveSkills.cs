using System.Collections;
using System.Linq;
using UnityEngine;

public class LOLS_PassiveSkills : MonoBehaviour
{
    [SerializeField] private GameObject cardRef;
    private LOLS_UI_StatsRuntime UI_StatsRuntime;

    void Start()
    {
        UI_StatsRuntime = GameObject.FindGameObjectWithTag("UI_Selectable").GetComponent<LOLS_UI_StatsRuntime>();
        Invoke("Heal", 1f);
    }
    public void Heal()
    {
        LOLS_Character mainCharacter = this.gameObject.GetComponent<LOLS_Character>();

        for (int i = 0; i < 3; i++)
        {
            LOLS_Character.S_Character mainCharacterStats;
            mainCharacterStats = mainCharacter.Characters[i];

            if(mainCharacterStats.Stats.Health > 0)
            {
                if (mainCharacterStats.Stats.Health + 0.5f > mainCharacterStats.Stats.MaxHealth)
                {
                    mainCharacterStats.Stats.Health = (float)mainCharacterStats.Stats.MaxHealth;
                }
                else
                {
                    mainCharacterStats.Stats.Health += 1f;
                }
                mainCharacter.Characters[i] = mainCharacterStats;  

                if(i == mainCharacter.CurrentCharacter)
                {
                    UI_StatsRuntime.SetCardHealthValue(mainCharacter.Characters[i].Stats.Health, cardRef.gameObject);
                    UI_StatsRuntime.SetCardDamagesValue(mainCharacter.Characters[i].Stats.Damage, cardRef.gameObject);
                    UI_StatsRuntime.SetCardLevel(mainCharacter.Characters[i].Stats.Level, cardRef.gameObject);
                }
            }
        }
        Invoke("Heal", 5f);
    } 
    

    public bool LockPickingBoost()
    {
        LOLS_Character mainCharacter = this.gameObject.GetComponent<LOLS_Character>();
        LOLS_Character.S_Character mainCharacterStats = mainCharacter.Characters[mainCharacter.CurrentCharacter];

        return mainCharacterStats.Playerclass == LOLS_Character.E_characterClasses.Rogue;
    }

}
