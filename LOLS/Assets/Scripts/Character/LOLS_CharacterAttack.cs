using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_CharacterAttack : MonoBehaviour
{
    [SerializeField] private LOLS_SelectableCharacter selectableCharacter;
    private LOLS_Character mainCharacter;
    private LOLS_Stats.S_stats characterStats;
    private LOLS_MoveCharacter characterMovement;
    private float lastTime = -Mathf.Infinity;
    void Awake()
    {
        mainCharacter = GetComponent<LOLS_Character>();
        characterMovement = GetComponent<LOLS_MoveCharacter>();
    }

    public void TriggerSimpleAttack()
    {
        LOLS_Character.S_Character currentCharacter = mainCharacter.Characters[mainCharacter.CurrentCharacter];
        characterStats = currentCharacter.Stats;

        if (Time.time - lastTime >= characterStats.AttackSpeed)
        {
            SimpleAttack();
            lastTime = Time.time;
        }
    }

    private void SimpleAttack()
    {
        LOLS_Grid currentCharacterGrid = characterMovement.GetCurrentGrid();
        Vector3 frontCell = characterMovement.GetFrontCell();
        LOLS_Grid grid = characterMovement.GetCurrentGrid();
        bool noWall;
        RaycastHit hit;
        // Vector3 raycastStart = transform.position;
        // Vector3 _direction = grid.GetWorldPositionOfCell(characterMovement.GetFrontCell(), characterMovement.GetFrontCell());
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector3.forward), Color.magenta, 15f);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 8f, (1 << 10)))
            {
                if(hit.transform.gameObject.tag == "Wall")
                {
                    noWall = false;
                }
                else
                {   
                    noWall = true;
                }
            }
            else
            {
                noWall = true;
            }

        if(noWall)
        {
            if(currentCharacterGrid.CheckCellBusy(frontCell) == LOLS_Tile.E_TileState.Ennemy)
            {
                if(currentCharacterGrid.CheckCellObject(frontCell) != null)
                {
                    switch (mainCharacter.CurrentCharacter)
                    {
                        case 0: LOLS_SoundManager.Instance.PlaySound("sword");
                        break;
                        case 1: LOLS_SoundManager.Instance.PlaySound("katana");
                        break;
                        case 2: LOLS_SoundManager.Instance.PlaySound("mageattack");
                        break;
                    }


                    selectableCharacter.AttackAnimation();
                    currentCharacterGrid.CheckCellObject(frontCell).GetComponent<LOLS_Enemy>().TakeDamages(characterStats.Damage, GetComponent<LOLS_Character>());
                }
            }
        }

        
    }
}
