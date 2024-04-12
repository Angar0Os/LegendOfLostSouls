using UnityEngine;

public class LOLS_Mannequin : MonoBehaviour
{
    private GameObject player;
    private LOLS_Grid grid;
    void Start()
    {
        Invoke("OnStarting", 0.3f);
    }

    private void OnStarting()
    {
        player = GameObject.FindGameObjectWithTag("Character");
        grid = player.GetComponentInParent<LOLS_MoveCharacter>().GetCurrentGrid();
        Vector3 currentCell = grid.GetCurrentCellByPosition(transform.position);
        transform.position = grid.GetWorldPositionOfCell(currentCell, currentCell);
        grid.SetCellBusy(currentCell, LOLS_Tile.E_TileState.Ennemy);
        grid.SetCellObject(currentCell, this.gameObject);
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, 5.75f, transform.GetChild(0).transform.position.z);

        foreach (GameObject card in GameObject.FindGameObjectsWithTag("EnemyCard"))
        {
            LOLS_EnnemyCard cardReff = card.GetComponent<LOLS_EnnemyCard>();

            if (cardReff.GetBusy())
            {

            }
            else
            {
                cardReff.EnemyCardEnterCombat(this.gameObject);
                break;
            }

        }
    }

    private void LootAkPlayer()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDirection.y = 0f;
        Quaternion rotationToPlayer = Quaternion.LookRotation(playerDirection, Vector3.up);
        float offsetAngle = 90f;
        Quaternion adjustedRotation = Quaternion.Euler(0f, rotationToPlayer.eulerAngles.y + offsetAngle, 0f);
        transform.GetChild(0).rotation = adjustedRotation;
    }

    void Update()
    {
        if(player != null)
        {
            LootAkPlayer();
        }
    }

        public void OnDie()
    {
        foreach (GameObject card in GameObject.FindGameObjectsWithTag("EnemyCard"))
        {
            LOLS_EnnemyCard cardReff = card.GetComponent<LOLS_EnnemyCard>();
            
            if (cardReff.GetBusy())
            {
                cardReff.EnemyCardExitCombat();
                break;
            }
            else
            {
               
            }

        }
        Vector3 currentCell = grid.GetCurrentCellByPosition(transform.position);
        grid.SetCellBusy(currentCell, LOLS_Tile.E_TileState.Free);
        grid.SetCellObject(currentCell, null);
    }
}
