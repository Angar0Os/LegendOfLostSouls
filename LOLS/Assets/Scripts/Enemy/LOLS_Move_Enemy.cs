using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Move_Enemy : MonoBehaviour
{
    [SerializeField] private bool isBoss = false;
    private GameObject player;
    private LOLS_Grid grid;
    private bool isMoving = false;
    private Vector3 targetCell;
    private Vector3 currentCell;
    private Vector3 lastCell;
    private Vector3 cacheCell;
    private float minJumpHeight = 5.75f;
    private bool isFollowingPlayer = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Character");

        if(isBoss)
        {
            minJumpHeight = 7.20f;
            grid = GameObject.FindGameObjectWithTag("BossRoom").GetComponent<LOLS_Grid>();
        }
        else
        {
            grid = player.GetComponentInParent<LOLS_MoveCharacter>().GetCurrentGrid();
        }

        transform.position = grid.GetWorldPositionOfCell(grid.GetCurrentCellByPosition(transform.position), grid.GetCurrentCellByPosition(transform.position));
        transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, minJumpHeight, transform.GetChild(0).transform.position.z);
        cacheCell = grid.GetCurrentCellByPosition(transform.position);
        currentCell = grid.GetCurrentCellByPosition(transform.position);
        lastCell = currentCell;
        grid.SetCellBusy(currentCell, LOLS_Tile.E_TileState.Ennemy);
        grid.SetCellObject(currentCell, this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(MoveToPlayer());
    }
    public void OnDie()
    {
        grid.SetCellBusy(lastCell, LOLS_Tile.E_TileState.Free);
        grid.SetCellObject(lastCell, null);
        currentCell = grid.GetCurrentCellByPosition(transform.position);
        grid.SetCellBusy(currentCell, LOLS_Tile.E_TileState.Free);
        grid.SetCellObject(currentCell, null);
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
    }

        private bool CanSeePlayer(Vector3 startCell, Vector3 _playerCell)
    {
        RaycastHit hit;
        // Vector3 raycastStart = grid.GetWorldPositionOfCell(startCell, startCell);
        // Vector3 go = CalculateDirection(currentCell, _playerCell);
        // Vector3 direction = _playerCell - startCell;
        

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 8f, (1 << 10)))
        {
            if(hit.transform.gameObject.tag == "Wall")
            {
                Debug.DrawLine(transform.position, transform.TransformDirection(Vector3.forward), Color.red, 15f);
                return false; 
            }
            else
            {
                return true;
            }

        }
        else
        {
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector3.forward), Color.black, 15f);
            return true;
        }
    }

    private IEnumerator MoveToPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Vector3 playerCell = grid.GetCurrentCellByPosition(player.transform.position);
            Vector3 currentCell = grid.GetCurrentCellByPosition(transform.position);

            if (isFollowingPlayer)
            {
                if (!AreCellsAdjacent(currentCell, playerCell))
                {
                    
                    isFollowingPlayer = false;
                    yield return null;
                }
            }
            else
            {
                if (AreCellsAdjacent(currentCell, playerCell))
                {
                    foreach (GameObject card in GameObject.FindGameObjectsWithTag("EnemyCard"))
                    {
                        LOLS_EnnemyCard cardReff = card.GetComponent<LOLS_EnnemyCard>();

                        if (CanSeePlayer(currentCell, playerCell))
                        {
                            if (cardReff.GetBusy())
                            {

                            }
                            else
                            {
                                cardReff.EnemyCardEnterCombat(this.gameObject);
                                isFollowingPlayer = true;
                                StartCoroutine(ToggleAttackMode());
                                break;
                            }
                        }
                        else if (!isMoving)
                        {

                                Vector3 direction = CalculateDirection(currentCell, playerCell);

                                if (CanMoveInDirection(direction))
                                {
                                    targetCell = currentCell + direction;
                                    StartCoroutine(MoveToNextCell(targetCell));
                                }
                                else
                                {
                                    yield return StartCoroutine(FindOtherPath(currentCell, playerCell));
                                }
                            
                        }
                    }
                }
                            else if (!isMoving)
                            {
                                Vector3 direction = CalculateDirection(currentCell, playerCell);

                                if (CanMoveInDirection(direction))
                                {
                                    targetCell = currentCell + direction;
                                    StartCoroutine(MoveToNextCell(targetCell));
                                }
                                else
                                {
                                    yield return StartCoroutine(FindOtherPath(currentCell, playerCell));
                                }
                            }
                        }
                    }
                }

    public void OnPooled(bool _returnToPool)
    {
        if(_returnToPool)
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
            transform.position = grid.GetWorldPositionOfCell(grid.GetCurrentCellByPosition(transform.position), grid.GetCurrentCellByPosition(transform.position));
            grid.SetCellBusy(grid.GetCurrentCellByPosition(transform.position), LOLS_Tile.E_TileState.Free);
            grid.SetCellObject(grid.GetCurrentCellByPosition(transform.position), null);

            grid.SetCellBusy(lastCell, LOLS_Tile.E_TileState.Free);
            grid.SetCellObject(lastCell, null);

            currentCell = grid.GetCurrentCellByPosition(transform.position);
            grid.SetCellBusy(targetCell, LOLS_Tile.E_TileState.Free);
            grid.SetCellObject(targetCell, null);
            isMoving = false;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Character");
            grid = player.GetComponentInParent<LOLS_MoveCharacter>().GetCurrentGrid();
            transform.position = grid.GetWorldPositionOfCell(grid.GetCurrentCellByPosition(transform.position), grid.GetCurrentCellByPosition(transform.position));
            grid.SetCellBusy(grid.GetCurrentCellByPosition(transform.position), LOLS_Tile.E_TileState.Ennemy);
            grid.SetCellObject(grid.GetCurrentCellByPosition(transform.position), this.gameObject);
            transform.GetChild(0).transform.position = new Vector3(transform.GetChild(0).transform.position.x, minJumpHeight, transform.GetChild(0).transform.position.z);
            lastCell = grid.GetCurrentCellByPosition(transform.position);
            currentCell = grid.GetCurrentCellByPosition(transform.position);
            StartCoroutine(MoveToPlayer());
        }
    }

     private IEnumerator ToggleAttackMode()
    {
        while(isFollowingPlayer)
        {
            GetComponent<LOLS_EnemyAttack>().TriggerSimpleAttack();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator FindOtherPath(Vector3 _currentCell, Vector3 _playerCell)
    {
        isMoving = true;
    
        HashSet<Vector3> visited = new HashSet<Vector3>();
        Stack<Vector3> stack = new Stack<Vector3>();

        stack.Push(_currentCell);

        while (stack.Count > 0)
        {
            Vector3 cell = stack.Pop();
            visited.Add(cell);

            List<Vector3> neighbors = GetAdjacentCells(cell);

            foreach (Vector3 neighbor in neighbors)
            {
                if (CanMoveInDirection(neighbor - _currentCell) && !AreCellsAdjacent(neighbor, _playerCell))
                {
                    targetCell = neighbor;
                    StartCoroutine(MoveToNextCell(targetCell));
                    yield break;
                }
                stack.Push(neighbor);
            }
        }

        targetCell = lastCell;
        StartCoroutine(MoveToNextCell(targetCell));
        isMoving = false;
    }
        private Vector3 CalculateDirection(Vector3 _currentCell, Vector3 _targetCell)
    {
        float x = 0;
        float z = 0;

        if (Mathf.Abs(_targetCell.x - _currentCell.x) > Mathf.Abs(_targetCell.z - _currentCell.z))
        {
            x = Mathf.Clamp(_targetCell.x - _currentCell.x, -1, 1);
        }
        else
        {
            z = Mathf.Clamp(_targetCell.z - _currentCell.z, -1, 1);
        }

        x = Mathf.Clamp(x, -grid.gridSizeX, grid.gridSizeX);
        z = Mathf.Clamp(z, -grid.gridSizeZ, grid.gridSizeZ);
        return new Vector3(x, 0, z);
    }

    private List<Vector3> GetAdjacentCells(Vector3 cell)
    {
        List<Vector3> adjacentCells = new List<Vector3>
        {
            new Vector3(cell.x + 1, cell.y, cell.z),
            new Vector3(cell.x - 1, cell.y, cell.z),
            new Vector3(cell.x, cell.y, cell.z + 1),
            new Vector3(cell.x, cell.y, cell.z - 1)
        };

        adjacentCells.Remove(lastCell);
        return adjacentCells;
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
        LootAkPlayer();
    }

        private IEnumerator MoveToNextCell(Vector3 _targetCell)
    {
        LOLS_SoundManager.Instance.PlaySoundWithDelay("move2", 0.1f);
        isMoving = true;
        grid.SetCellBusy(lastCell, LOLS_Tile.E_TileState.Free);
        grid.SetCellObject(lastCell, null);
        lastCell = _targetCell;
        grid.SetCellBusy(_targetCell, LOLS_Tile.E_TileState.Ennemy);
        grid.SetCellObject(_targetCell, this.gameObject);
        while (Vector3.Distance(transform.position, grid.GetWorldPositionOfCell(_targetCell, _targetCell)) > 0.01f)
        {
            if(grid.GetWorldPositionOfCell(_targetCell, _targetCell) == Vector3.zero)
            {
                _targetCell = cacheCell;
            }
            else
            {
                if (Vector3.Distance(transform.position, grid.GetWorldPositionOfCell(_targetCell, _targetCell)) >= 7.5f)
                {
                    transform.GetChild(0).transform.position = Vector3.Lerp(transform.GetChild(0).transform.position, new Vector3(transform.GetChild(0).transform.position.x, 18f, transform.GetChild(0).transform.position.z), Time.deltaTime * 3f);
                }
                else
                {
                    transform.GetChild(0).transform.position = Vector3.Lerp(transform.GetChild(0).transform.position, new Vector3(transform.GetChild(0).transform.position.x, minJumpHeight, transform.GetChild(0).transform.position.z), Time.deltaTime * 3f);
                }
                transform.position = Vector3.MoveTowards(transform.position, grid.GetWorldPositionOfCell(_targetCell, _targetCell), Time.deltaTime * 5f);
            }
            yield return null;
        }
        transform.position = grid.GetWorldPositionOfCell(_targetCell, _targetCell);
        currentCell = grid.GetCurrentCellByPosition(transform.position);

        isMoving = false;
    }
        private bool AreCellsAdjacent(Vector3 cell1, Vector3 cell2)
    {
        int deltaX = Mathf.Abs(Mathf.RoundToInt(cell1.x) - Mathf.RoundToInt(cell2.x));
        int deltaZ = Mathf.Abs(Mathf.RoundToInt(cell1.z) - Mathf.RoundToInt(cell2.z));

        return (deltaX == 1 && deltaZ == 0) || (deltaX == 0 && deltaZ == 1);
    }
    private bool CanMoveInDirection(Vector3 _direction)
        {
            if(grid.GetWorldPositionOfCell(grid.GetCurrentCellByPosition(transform.position) + _direction, transform.position) != Vector3.zero)
            {
                RaycastHit hit;
                Vector3 raycastStart = transform.position;
                Vector3 raycastEnd = grid.GetWorldPositionOfCell(grid.GetCurrentCellByPosition(transform.position) + _direction, transform.position);

                Debug.DrawLine(raycastStart, raycastEnd, Color.green, 1f);

                if (Physics.Raycast(raycastStart, _direction, out hit, 8f, (1 << 10)))
                {
                    Debug.DrawLine(raycastStart, raycastEnd, Color.red, 1f);
                    return false;
                }
                else if(grid.CheckCellBusy(grid.GetCurrentCellByPosition(raycastEnd)) == LOLS_Tile.E_TileState.Free)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }
}
