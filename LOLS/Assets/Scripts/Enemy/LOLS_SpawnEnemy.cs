using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_SpawnEnemy : MonoBehaviour
{
    private LOLS_Grid grid;
    [SerializeField] private GameObject PrefabEnemy;
    [SerializeField] private bool isBossSpawner = false;
    private List<Vector3> ListSpawnDispo;
    private List<Vector3> ListSpawnUsed = new List<Vector3>();
    private List<GameObject> ListEnemy = new List<GameObject>();
    private Dictionary<GameObject, Vector3> DictionnairePositionEnemy = new Dictionary<GameObject, Vector3>();
    private bool roomVisited = false;
    private LOLS_Dice dice;
    [SerializeField] private LOLS_TextBoxManager affichageText;

    [SerializeField] private LayerMask gridLayer;
    int res;

    public void OnRoomChanged(bool _enter)
    {
        if (_enter)
        {
            if (roomVisited == false)
            {

                if (!isBossSpawner)
                {
                    int ResultDice = dice.DiceRoll();

                    affichageText.RemoveMessages();

                    affichageText.SendMessage("Le Narrateur lance les dés pour déterminer le nombre d'ennemi dans la salle...");
                    affichageText.SendMessage("Résultat du Dé : " + ResultDice);

                    affichageText.SendMessage(" ");

                    if (ResultDice <= 2)
                    {
                        affichageText.SendMessage("Echec du Narrateur : Pas d'ennemi");
                        //Debug.Log("Echec du GameMaster : Pas d'ennemi");
                    }

                    else if (ResultDice <= 18 || CheckNbSpawnEnemy() < 2)
                    {
                        affichageText.SendMessage("Réussite du Narrateur : Un ennemi est apparu");
                        //Debug.Log("Réussite du GameMaster : un ennemi est apparu");
                        SpawnEnemy();
                    }

                    else if (CheckNbSpawnEnemy() > 1)
                    {
                        affichageText.SendMessage("Réussite totale du Narrateur : Deux ennemi sont apparu");
                        //Debug.Log("Réussite totale du GameMaster : deux ennemi sont apparu");
                        SpawnEnemy();
                        SpawnEnemy();
                    }
                }
                else
                {
                    SpawnEnemy();
                }

            }
            else
            {
                if (ListEnemy.Count > 0)
                {
                    foreach (GameObject _enemy in ListEnemy)
                    {
                        if (_enemy != null)
                        {
                            _enemy.SetActive(true);
                            DictionnairePositionEnemy.TryGetValue(_enemy, out Vector3 _position);
                            _enemy.transform.position = _position;
                            _enemy.GetComponent<LOLS_Enemy>().Enemy.Stats.Health = _enemy.GetComponent<LOLS_Enemy>().Enemy.Stats.MaxHealth;
                            _enemy.GetComponent<LOLS_Move_Enemy>().OnPooled(false);
                        }
                    }
                }

            }

        }
        else
        {
            if (ListEnemy.Count > 0)
            {
                foreach (GameObject _enemy in ListEnemy)
                {
                    if (_enemy != null)
                    {
                        _enemy.GetComponent<LOLS_Move_Enemy>().OnPooled(true);
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
            if (grid.CheckCellBusy(positionCell) == LOLS_Tile.E_TileState.Free)
            {
                ListSpawnDispo.Add(positionCell);
            }
        }
        res = Random.Range(0, ListSpawnDispo.Count);

        ListSpawnUsed.Add(ListSpawnDispo[res]);

        var positionWorld = grid.GetWorldPositionOfCell(ListSpawnDispo[res], ListSpawnDispo[res]);
        grid.SetCellBusy(ListSpawnDispo[res], LOLS_Tile.E_TileState.Ennemy);

        GameObject ennemy = Instantiate(PrefabEnemy, positionWorld, Quaternion.identity);
        ListEnemy.Add(ennemy);
        DictionnairePositionEnemy.Add(ennemy, positionWorld);
    }

    public int CheckNbSpawnEnemy()
    {
        ListSpawnDispo = new List<Vector3>();
        foreach (Transform child in transform)
        {
            var positionCell = grid.GetCurrentCellByPosition(child.position);
            if (grid.CheckCellBusy(positionCell) == LOLS_Tile.E_TileState.Free)
            {
                ListSpawnDispo.Add(positionCell);
            }
        }
        return ListSpawnDispo.Count;
    }

    public void FindNearestGrid()
    {
        RaycastHit _hit;

        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) , Color.blue, 60, false);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 10f, gridLayer))
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
