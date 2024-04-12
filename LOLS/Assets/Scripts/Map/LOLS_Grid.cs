using System;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Grid : MonoBehaviour
{
    private Dictionary<Vector3, LOLS_Tile.E_TileState> cellsStates = new Dictionary<Vector3, LOLS_Tile.E_TileState>();
    private Dictionary<Vector3, Vector3> cellsPositions = new Dictionary<Vector3, Vector3>();
    private Dictionary<Vector3, GameObject> cellsObjects = new Dictionary<Vector3, GameObject>();
    public int gridSizeX;
    public int gridSizeY;
    public int gridSizeZ;
    private Vector3 offset;
    private const float cellSize = 15.0f;
    [SerializeField] private Material material_Wireframe;
    private List<Renderer> debugCellsRenderer = new List<Renderer>();
    private GameObject enemysSpawner;

    public Vector3 CheckIfCellExist(Vector3 _cellToCheck, Vector3 _cellFrom)
    {
        Vector3 cellToReturn = Vector3.zero;
        cellsPositions.TryGetValue(_cellToCheck, out cellToReturn);
        if(cellToReturn == Vector3.zero)
        {
        return _cellFrom;
        }
        else
        {
        return cellToReturn;
        }
    }

    public LOLS_SpawnEnemy GetCurrentEnemysSpawnerOnGrid()
    {
        if (enemysSpawner == null)
        {
            return null;
        }
        else
        {
            return enemysSpawner.GetComponent<LOLS_SpawnEnemy>();
        }
    }

    private void Awake()
    {
        GridInitialization();
    }

    public void SetGridSpawner(GameObject _enemysSpawnerToAdd)
    {
        enemysSpawner = _enemysSpawnerToAdd;
    }

    private void GridInitialization()
    {
        Vector3 gridSize = SetGridSize();
        gridSizeX = Mathf.CeilToInt(gridSize.x / cellSize);
        gridSizeY = Mathf.CeilToInt(gridSize.y / cellSize);
        gridSizeZ = Mathf.CeilToInt(gridSize.z / cellSize);

        offset = new Vector3(-gridSize.x / 2f, -gridSize.y / 2f, -gridSize.z / 2f);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize, z * cellSize) + offset;
                    Vector3 cellCenterWorldPosition = GetCurrentPositionByCell(cellPosition);
                    GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cell.transform.position = cellCenterWorldPosition;
                    cell.transform.localScale = Vector3.one * cellSize;

                    if (material_Wireframe != null)
                    {
                        cell.GetComponent<Renderer>().material = material_Wireframe;
                    }

                    cell.transform.parent = transform;
                    debugCellsRenderer.Add(cell.GetComponent<Renderer>());
                    Vector3 cellKey = new Vector3(x, y, z);
                    cellsStates.Add(cellKey, LOLS_Tile.E_TileState.Free);
                    cellsPositions.Add(cellKey, cellCenterWorldPosition);
                }
            }
        }

        ToggleVisibleGridDebug(false);
    }

    public void ToggleDoor(GameObject _door)
    {
        if (_door.transform.eulerAngles.y == 0)
        {
            _door.transform.eulerAngles = new Vector3(_door.transform.eulerAngles.x, 90f, _door.transform.eulerAngles.z);
        }
        else
        {
            _door.transform.eulerAngles = new Vector3(_door.transform.eulerAngles.x, 0f, _door.transform.eulerAngles.z);
        }
    }

    private Vector3 SetGridSize()
    {
        Renderer _renderer = GetComponent<Renderer>();
        return _renderer.bounds.size;
    }

    public void ToggleVisibleGridDebug(bool _visible)
    {
        foreach (Renderer _debugCellRenderer in debugCellsRenderer)
        {
            _debugCellRenderer.enabled = _visible;
        }
    }

    public void SetCellBusy(Vector3 _cellPosition, LOLS_Tile.E_TileState _state)
    {
        Vector3 cellKey = _cellPosition;

        if (cellsStates.ContainsKey(cellKey))
        {
            cellsStates[cellKey] = _state;
        }
        else
        {
            cellsStates.Add(cellKey, _state);
        }
    }

    public LOLS_Tile.E_TileState CheckCellBusy(Vector3 _cellKey)
    {
        Vector3 cellKey = _cellKey;
        cellsStates.TryGetValue(cellKey, out LOLS_Tile.E_TileState cellState);
        return cellState;
    }

    public Vector3 GetCurrentCellByPosition(Vector3 _worldPosition)
    {
        float nearestCellDistance = Mathf.Infinity;
        Vector3 currentCellPosition = Vector3.zero;

        foreach (Vector3 _key in cellsPositions.Keys)
        {
            cellsPositions.TryGetValue(_key, out Vector3 _result);
            if (Vector3.Distance(_worldPosition, _result) < nearestCellDistance)
            {
                nearestCellDistance = Vector3.Distance(_worldPosition, _result);
                currentCellPosition = _key;
            }
        }
        return currentCellPosition;
    }

    private Vector3 GetCurrentPositionByCell(Vector3 _currentCell)
    {
        float posX = _currentCell.x + cellSize / 2.0f;
        float posY = _currentCell.y + cellSize / 2.0f;
        float posZ = _currentCell.z + cellSize / 2.0f;

        return new Vector3(posX, posY, posZ) + transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize, z * cellSize) + offset;
                    Vector3 cellCenter = GetCurrentPositionByCell(cellPosition);
                    Gizmos.DrawWireCube(cellCenter, Vector3.one * cellSize);
                }
            }
        }
    }

    public GameObject CheckCellObject(Vector3 _cellToCheck)
    {
        cellsObjects.TryGetValue(_cellToCheck, out GameObject _outObject);
        return _outObject;
    }

    public void SetCellObject(Vector3 _cellToAdd, GameObject _gameObjectToAdd)
    {
        cellsObjects[_cellToAdd] = _gameObjectToAdd;
    }

    public Vector3 GetWorldPositionOfCell(Vector3 _cellToCheck, Vector3 _cellFrom)
    {
        if (cellsPositions.ContainsKey(_cellToCheck))
        {
            cellsPositions.TryGetValue(_cellToCheck, out Vector3 outvector);
            return outvector;
        }
        else
        {
            cellsPositions.TryGetValue(_cellFrom, out Vector3 _cellFromWorld);
            return _cellFromWorld;
        }
    }
}
