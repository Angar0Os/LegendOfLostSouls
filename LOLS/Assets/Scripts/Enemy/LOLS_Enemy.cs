using UnityEngine;
using UnityEngine.SceneManagement;

public class LOLS_Enemy : MonoBehaviour
{
    private LOLS_EnnemyCard cardRef;
    private LOLS_UI_StatsRuntime UI_StatsRuntime;
    public enum E_typeEnemy
    {
        Gobelin,
        Roi_Gobelins,
        Mannequin
    };

    [System.Serializable]
    public struct S_Enemy
    {
        public E_typeEnemy TypeEnemy;
        public LOLS_Stats.S_stats Stats;
    }
    public S_Enemy Enemy;

    private void Awake()
    {
        UI_StatsRuntime = GameObject.FindGameObjectWithTag("UI_Selectable").GetComponent<LOLS_UI_StatsRuntime>();
            if (gameObject.tag == "Mannequin")
            {
                cardRef = GameObject.FindGameObjectWithTag("UI_Selectable").transform.GetChild(0).transform.GetChild(0).GetComponent<LOLS_EnnemyCard>();
                SetStatsUI();
            }
    }

    public void SetStatsUI()
    {
        UI_StatsRuntime.SetCardHealthValue(Enemy.Stats.Health, cardRef.gameObject);
        UI_StatsRuntime.SetCardDamagesValue(Enemy.Stats.Damage, cardRef.gameObject);
        UI_StatsRuntime.SetCardLevel(Enemy.Stats.Level, cardRef.gameObject);
    }

    public LOLS_EnnemyCard GetCardRef()
    {
        return cardRef;
    }

    public void SetCardRef(LOLS_EnnemyCard _card)
    {
        cardRef = _card;
        SetStatsUI();
    }

    public void TakeDamages(int _damagesAmount, LOLS_Character player)
    {
        Enemy.Stats.Health -= _damagesAmount;
        UI_StatsRuntime.SetCardHealthValue(Enemy.Stats.Health, cardRef.gameObject);

        if(Enemy.Stats.Health <= 0)
        {
            LOLS_SoundManager.Instance.PlaySound("gobHitted");
            LOLS_Character.S_Character playerStats = player.Characters[player.CurrentCharacter];
            playerStats.Stats.CurrentXP += 5;
            player.Characters[player.CurrentCharacter] = playerStats;
            if(gameObject.tag == "Mannequin")
            {
                GetComponent<LOLS_Mannequin>().OnDie();
            }
            else
            {
                GetComponent<LOLS_Move_Enemy>().OnDie();
                if(gameObject.tag == "Boss")
                {
                    SceneManager.LoadScene("LOLS_VictoryMenu");
                }
            }
            Destroy(this.gameObject);
        }
    }
}
