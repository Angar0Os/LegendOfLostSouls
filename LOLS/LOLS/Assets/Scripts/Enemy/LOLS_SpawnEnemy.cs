using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_SpawnEnemy : MonoBehaviour
{
    [SerializeField]
    private LOLS_Grid grid;
    public GameObject PrefabEnemy;
    private List<Vector3> ListSpawnDispo;
    private List<Vector3> ListSpawnUsed = new List<Vector3>();
    private List<GameObject> ListEnemy = new List<GameObject>();
    private Dictionary<GameObject,Vector3> DictionnairePositionEnemy = new Dictionary<GameObject, Vector3>();
    private bool roomVisited=false;
    private LOLS_Dice dice;

    [SerializeField] private LayerMask gridLayer;
    int res;

    public void OnRoomChanged(bool _enter)
    {
        if(_enter)
        {
            if(roomVisited==false)
            {
                roomVisited=true;
                int ResultDice = dice.DiceRoll();

                Debug.Log(ResultDice);

                if(ResultDice<=5)
                {
                    Debug.Log("Echec du GameMaster : Pas d'ennemi");
                }

                else if(ResultDice<=15)
                {
                    Debug.Log("Réussite du GameMaster : un ennemi est apparu");
                    SpawnEnemy();
                }

                else 
                {
                    Debug.Log("Réussite totale du GameMaster : deux ennemi sont apparu");
                    SpawnEnemy();
                    SpawnEnemy();
                }
            }
            else
            {
                if(ListEnemy.Count>0)
                {
                    foreach (GameObject _enemy in ListEnemy)
                    {
                        if(_enemy != null)
                        {
                            
                            _enemy.SetActive(true);
                            DictionnairePositionEnemy.TryGetValue(_enemy, out Vector3 _position);
                            _enemy.transform.position = _position; 
                            _enemy.GetComponent<LOLS_Enemy>().Enemy.Stats.Health=_enemy.GetComponent<LOLS_Enemy>().Enemy.Stats.MaxHealth;

                        }
                    }
                }
                
            }
            
        }
        else
        {
            if(ListEnemy.Count>0)
            {
                foreach (GameObject _enemy in ListEnemy)
                {
                    if(_enemy != null)
                    {
                        _enemy.SetActive(false);
                    }
                }
            }
            
        }
    }

    public void SpawnEnemy()
    {   
        ListSpawnDispo = new List<Vector3>();
        foreach (Transform child in transform)
        {
            var positionCell = grid.GetCurrentCellByPosition(child.position);
            if(grid.CheckCellBusy(positionCell)==LOLS_Tile.E_TileState.Free)
            {
                ListSpawnDispo.Add(positionCell);
            }
        }

        res = Random.Range(0,ListSpawnDispo.Count);
        Debug.Log(ListSpawnDispo[res]);

        ListSpawnUsed.Add(ListSpawnDispo[res]);

        var positionWorld = grid.GetWorldPositionOfCell(ListSpawnDispo[res], ListSpawnDispo[res]);
        grid.SetCellBusy(ListSpawnDispo[res], LOLS_Tile.E_TileState.Ennemy);

        GameObject ennemy = Instantiate(PrefabEnemy, positionWorld, Quaternion.identity);
        ListEnemy.Add(ennemy);
        DictionnairePositionEnemy.Add(ennemy,positionWorld);
    }

    public void GridChange()
    {
        
    }

    public void FindNearestGrid()
        {
            RaycastHit _hit;
                                         
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) , Color.blue, 60, false);
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 10f, gridLayer))
            {
                grid = _hit.transform.gameObject.GetComponent<LOLS_Grid>();
            }
        }


    void Start()
    {
        dice = GameObject.FindGameObjectWithTag("Dice").GetComponent<LOLS_Dice>();
        gridLayer = (1 << 11);
        FindNearestGrid();
        grid.SetGridSpawner(this.gameObject);
    }

    void Update()
    {

    }

}
 