using System;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Character : MonoBehaviour
{
    [SerializeField] private GameObject cardRef;
    
    private LOLS_UI_StatsRuntime UI_StatsRuntime;
    public enum E_characterClasses
    {
        Guerrier,
        Mage,
        Rogue
    };

    
    public struct S_Character
    {
        public E_characterClasses Playerclass;
        public LOLS_Stats.S_stats Stats;
    }

    public List<S_Character> Characters = new List<S_Character>();

    [SerializeField]
    private List<LOLS_Stats.S_stats> charactersStats;

    public int  CurrentCharacter = 0;

    private void Awake()
    {
        CurrentCharacter = 0;

        for (int i = 0; i < 3; ++i)
        {
            S_Character s_Character = new();
            E_characterClasses Enum_Class_inst = new();
            Enum_Class_inst = (E_characterClasses)i;
            s_Character.Playerclass = Enum_Class_inst;
            s_Character.Stats = charactersStats[i];
            Characters.Add(s_Character);
        }

        S_Character s_character = Characters[CurrentCharacter];
        UI_StatsRuntime = GameObject.FindGameObjectWithTag("UI_Selectable").GetComponent<LOLS_UI_StatsRuntime>();
        UI_StatsRuntime.SetCardHealthValue(s_character.Stats.Health, cardRef.gameObject);
        UI_StatsRuntime.SetCardDamagesValue(s_character.Stats.Damage, cardRef.gameObject);
        UI_StatsRuntime.SetCardLevel(s_character.Stats.Level, cardRef.gameObject);
    }

    public void SetStatsUI()
    {
        S_Character s_character = Characters[CurrentCharacter];
        UI_StatsRuntime.SetCardHealthValue(s_character.Stats.Health, cardRef.gameObject);
        UI_StatsRuntime.SetCardDamagesValue(s_character.Stats.Damage, cardRef.gameObject);
        UI_StatsRuntime.SetCardLevel(s_character.Stats.Level, cardRef.gameObject);
    }
    public void TakeDamages(int _damagesAmount)
    {
        S_Character s_character = Characters[CurrentCharacter];
        s_character.Stats.Health -= _damagesAmount;
        UI_StatsRuntime.SetCardHealthValue(s_character.Stats.Health, cardRef.gameObject);
        Characters[CurrentCharacter] = s_character;     

        if (Characters[CurrentCharacter].Stats.Health <= 0)
        {
            LOLS_SoundManager.Instance.PlaySound("heroHitted");
        }



        foreach (GameObject card in GameObject.FindGameObjectsWithTag("EnemyCard"))
        {
            LOLS_EnnemyCard cardRef = card.GetComponent<LOLS_EnnemyCard>();
            if (cardRef.GetBusy())
            {
                cardRef.EnemyAttack();
                break;
            }
            else
            {
            }
        }
    }
}
