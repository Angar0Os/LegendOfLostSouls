using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LOLS_Move_Enemy : MonoBehaviour
{
    private LOLS_Grid grid;
    private Transform player;
    private List<Vector3> path;
    private int currentPathIndex = 0;
    private float moveSpeed = 5f;

    private void CheckCurrentGrid()
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, Vector3.down, Color.black, 20f);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 8f, (1 << 11)))
        {
            grid = hit.transform.gameObject.GetComponent<LOLS_Grid>();
        }
    }

    private void Start()
    {
        CheckCurrentGrid();
        player = GameObject.FindGameObjectWithTag("Character").transform;
        StartCoroutine(MoveAlongPath());
    }

    private void Update()
    {
        if (currentPathIndex < path.Count &&
            Vector3.Distance(transform.position, grid.GetWorldPositionOfCell(path[currentPathIndex], transform.position)) < 0.1f)
        {
            Vector3 startCell = grid.GetCurrentCellByPosition(transform.position);
            Vector3 goalCell = grid.GetCurrentCellByPosition(player.position);
            path = FindPath(startCell, goalCell);
        }
    }

    private List<Vector3> FindPath(Vector3 start, Vector3 goal)
        {
            List<Vector3> path = new List<Vector3>();
            List<Vector3> openSet = new List<Vector3> { start };
            Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
            Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> 
            { 
                { 
                    start, 0 
                } 
            };
            Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float> 
            { 
                { 
                    start, DistanceEstimation(start, goal) 
                    } 
            };

            while (openSet.Count > 0)
            {
                Vector3 currentCell = GetLowestFScoreCell(openSet, fScore);
                openSet.Remove(currentCell);

                if (currentCell == goal)
                {
                    path = ReconstructPath(cameFrom, goal);
                    break;
                }

                foreach (Vector3 neighborCell in GetNeighborCells(grid, currentCell))
                {
                    float tentativeGScore = gScore[currentCell] + Vector3.Distance(currentCell, neighborCell);

                    if (!gScore.ContainsKey(neighborCell) || tentativeGScore < gScore[neighborCell])
                    {
                        cameFrom[neighborCell] = currentCell;
                        gScore[neighborCell] = tentativeGScore;
                        fScore[neighborCell] = gScore[neighborCell] + DistanceEstimation(neighborCell, goal);

                        if (!openSet.Contains(neighborCell))
                        {
                            openSet.Add(neighborCell);
                        }
                    }
                }
            }

            return path;
        }

    private IEnumerator MoveAlongPath()
    {
        Vector3 startCell = grid.GetCurrentCellByPosition(transform.position);
        Vector3 goalCell = grid.GetCurrentCellByPosition(player.position);
        path = FindPath(startCell, goalCell);

        while (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = grid.GetWorldPositionOfCell(path[currentPathIndex], transform.position);
            float distance = Vector3.Distance(transform.position, targetPosition);
            Vector3 currentPlayerCell = grid.GetCurrentCellByPosition(player.position);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetPosition - transform.position, out hit, 8f, (1 << 10)))
            {
                Debug.DrawLine(transform.position, hit.point, Color.black);
                if (hit.collider.transform.gameObject.CompareTag("Wall"))
                {
                    Debug.Log("Wall");
                    startCell = grid.GetCurrentCellByPosition(transform.position);
                    goalCell = grid.GetCurrentCellByPosition(player.position);
                    path = FindPath(startCell, goalCell);

                    if (path.Count > 0)
                    {
                        currentPathIndex = 0;
                    }
                    else
                    {
                        Debug.Log("pas de chemin trouvé c'est bug");
                        yield break;
                    }

                    yield return null;
                    continue;
                }
            }

            if (path[currentPathIndex] != currentPlayerCell)
            {
                if (distance < 0.1f)
                {
                    currentPathIndex++;
                    yield return null;

                    if (currentPathIndex < path.Count && Vector3.Distance(transform.position, grid.GetWorldPositionOfCell(path[currentPathIndex], transform.position)) < 0.1f)
                    {
                        startCell = grid.GetCurrentCellByPosition(transform.position);
                        goalCell = grid.GetCurrentCellByPosition(player.position);
                        path = FindPath(startCell, goalCell);
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }


    private Vector3 GetLowestFScoreCell(List<Vector3> openSet, Dictionary<Vector3, float> fScore)
    {
        float lowestFScore = Mathf.Infinity;
        Vector3 lowestCell = Vector3.zero;

        foreach (Vector3 cell in openSet)
        {
            if (fScore.ContainsKey(cell) && fScore[cell] < lowestFScore)
            {
                lowestFScore = fScore[cell];
                lowestCell = cell;
            }
        }

        return lowestCell;
    }

    private float DistanceEstimation(Vector3 start, Vector3 goal)
    {
        return Vector3.Distance(start, goal);
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        List<Vector3> path = new List<Vector3> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }

        return path;
    }

    private List<Vector3> GetNeighborCells(LOLS_Grid grid, Vector3 cell)
    {
        return grid.GetNeighborCells(cell);
    }
}