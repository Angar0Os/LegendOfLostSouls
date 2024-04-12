using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class LOLS_MoveCharacter : MonoBehaviour
{
    //[SerializeField] private LOLS_UIButtons UI_Button;
    private LOLS_Grid currentGrid;
    private float moveSpeed = 5f;
    private float rotateSpeed = 5f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 currentCell;
    public Vector3 cellToMove = new Vector3(0, 0, 1);
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask obstaclesLayer;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject tutorial;
    private string tt;
    private LOLS_Door lastDoorOpened;
    private bool isRotating = false;
    private float targetEulerYRotation;
    private Vector3 currentCellCheck;
    private Vector3 lastCell;
    private LOLS_Tile.E_TileState lastCellState;

    void Start()
    {
        Invoke("Spawn", 0.15f);
    }

    private void Spawn()
    {
        FindNearestGrid();

        Vector3 cellPosition = currentGrid.GetCurrentCellByPosition(transform.position);
        Vector3 cellWorldPosition = currentGrid.GetWorldPositionOfCell(cellPosition, cellPosition);
        transform.position = cellWorldPosition;
        currentCell = currentGrid.GetCurrentCellByPosition(transform.position);
        lastCell = currentCell;
        lastCellState = currentGrid.CheckCellBusy(currentCell);
        currentGrid.SetCellBusy(currentCell, LOLS_Tile.E_TileState.Player);
    }

    public Vector3 GetFrontCellGrid()
    {
        return currentCell = currentGrid.GetCurrentCellByPosition(transform.position) + cellToMove;
    }
    public void Move(string _moveOrientation)
    {
        if (!isMoving && !isRotating)
        {
            currentCell = currentGrid.GetCurrentCellByPosition(transform.position);
            switch (_moveOrientation)
            {
                case "Forward":
                    targetPosition = currentGrid.GetWorldPositionOfCell(currentCell + cellToMove, currentCell);
                    tt = "Forward";
                    break;
                case "Backward":
                    targetPosition = currentGrid.GetWorldPositionOfCell(currentCell - cellToMove, currentCell);
                    tt = "Backward";
                    break;
                case "Right":
                    targetPosition = currentGrid.GetWorldPositionOfCell(currentCell + new Vector3(cellToMove.z, cellToMove.y, -cellToMove.x), currentCell);
                    tt = "Right";
                    break;
                case "Left":
                    targetPosition = currentGrid.GetWorldPositionOfCell(currentCell + new Vector3(-cellToMove.z, cellToMove.y, cellToMove.x), currentCell);
                    tt = "Left";
                    break;
            }

            if (CheckObstacles(targetPosition) == true || currentGrid.CheckCellBusy(currentGrid.GetCurrentCellByPosition(targetPosition)) != LOLS_Tile.E_TileState.Free)
            {
                return;
            }
            else
            {
                isMoving = true;
                LOLS_SoundManager.Instance.PlaySoundWithDelay("move2", 0.1f);
            }
        }
    }

    public LOLS_Grid GetCurrentGrid()
    {
        return currentGrid;
    }

    public void Rotate(string _rotationOrientation)
    {
        if (!isMoving && !isRotating)
        {
            int newRotationY = 0;
            switch (gameObject.transform.eulerAngles.y)
            {
                case 0:
                    newRotationY = (_rotationOrientation == "Right") ? 90 : 270;
                    if (newRotationY == 270)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 359, gameObject.transform.eulerAngles.z);
                    }
                    break;
                case 90:
                    newRotationY = (_rotationOrientation == "Right") ? 180 : 0;
                    break;
                case 180:
                    newRotationY = (_rotationOrientation == "Right") ? 270 : 90;
                    break;
                case 270:
                    newRotationY = (_rotationOrientation == "Right") ? 359 : 180;
                    break;
                case 359:
                    newRotationY = (_rotationOrientation == "Right") ? 90 : 270;
                    if (newRotationY == 90)
                    {
                        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 0, gameObject.transform.eulerAngles.z);
                    }
                    break;
            }
            targetEulerYRotation = newRotationY;
            isRotating = true;
            LOLS_SoundManager.Instance.PlaySound("rotate");
        }
    }

    public Vector3 GetFrontCell()
    {
        return currentGrid.GetCurrentCellByPosition(transform.position) + cellToMove;
    }

    private void UpdateDirection()
    {
        switch (gameObject.transform.eulerAngles.y)
        {
            case 0:
                cellToMove = new Vector3(0, 0, 1);
                break;
            case 90:
                cellToMove = new Vector3(1, 0, 0);
                break;
            case 180:
                cellToMove = new Vector3(0, 0, -1);
                break;
            case 270:
                cellToMove = new Vector3(-1, 0, 0);
                break;
            case 359:
                cellToMove = new Vector3(0, 0, 1);
                break;
        }
        checkDoorUI();
    }

    public void FindNearestGrid()
    {
        RaycastHit _hit;
        Debug.DrawLine(transform.position, Vector3.down, Color.black, 20f);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit, 8f, gridLayer))
        {
            currentGrid = _hit.transform.gameObject.GetComponent<LOLS_Grid>();
        }
    }

    public void FindOtherNearestGrid(Vector3 _direction)
    {
        RaycastHit _hit;
        Vector3 _rayDirection = transform.TransformDirection(new Vector3(0f, -0.5f, 0f) + Vector3.forward);
        if (Physics.Raycast(transform.position, _rayDirection, out _hit, 64f, gridLayer))
        {
            Debug.DrawLine(transform.position, _hit.point, Color.black, 20f);
            if (_hit.transform.gameObject != currentGrid)
            {
                currentGrid = _hit.transform.gameObject.GetComponent<LOLS_Grid>();
            }
        }
    }


    public void TryToggleDoor()
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, 8f, obstaclesLayer))
        {
            Debug.DrawLine(transform.position, _hit.transform.position, Color.magenta, 20f);
            if (_hit.transform.tag == "Door")
            {
                lastDoorOpened = _hit.transform.gameObject.GetComponent<LOLS_Door>();
                ui.GetComponent<LOLS_UIButtons>().Lockpickbutton(lastDoorOpened);
                //lastDoorOpened.ToggleDoor();
            }
        }
    }

    public bool CheckObstacles(Vector3 _cellToCheck)
    {
        RaycastHit _hit;
        Vector3 _rayDirection = Vector3.zero;
        switch (tt)
        {
            case "Forward":
                _rayDirection = transform.TransformDirection(Vector3.forward);
                break;
            case "Backward":
                _rayDirection = transform.TransformDirection(Vector3.back);
                break;
            case "Right":
                _rayDirection = transform.TransformDirection(Vector3.right);
                break;
            case "Left":
                _rayDirection = transform.TransformDirection(Vector3.left);
                break;
        }
        Debug.DrawRay(transform.position, _rayDirection * 8f, Color.blue, 15, false);
        if (Physics.Raycast(transform.position, _rayDirection, out _hit, 8f, obstaclesLayer))
        {
            switch (_hit.transform.gameObject.tag)
            {
                default:
                    return false;
                case "Wall":
                    return true;
                case "Door":
                    if (_hit.transform.gameObject.GetComponent<LOLS_Door>().CheckIfClosed() == false)
                    {
                        if (currentGrid.GetCurrentEnemysSpawnerOnGrid() != null)
                        {
                            currentGrid.GetCurrentEnemysSpawnerOnGrid().OnRoomChanged(false);
                        }
                        currentGrid.SetCellBusy(currentGrid.GetCurrentCellByPosition(transform.position), LOLS_Tile.E_TileState.Free);
                        FindOtherNearestGrid(_rayDirection);
                        Debug.DrawLine(transform.position, _hit.point, Color.yellow, 20f);
                        targetPosition = currentGrid.GetWorldPositionOfCell(currentGrid.GetCurrentCellByPosition(_hit.transform.position), currentGrid.GetCurrentCellByPosition(_hit.transform.position));
                    }
                    return _hit.transform.gameObject.GetComponent<LOLS_Door>().CheckIfClosed();
            }
        }
        else
        {
            return false;
        }
    }

    public void SetCurrentCell()
    {
        currentCell = currentGrid.GetCurrentCellByPosition(transform.position);
    }

    public void OnCellReached()
    {
        if (currentGrid.GetCurrentEnemysSpawnerOnGrid() != null)
        {
            currentGrid.GetCurrentEnemysSpawnerOnGrid().OnRoomChanged(true);
           // GetComponent<LOLS_PassiveSkills>().Heal();
        }
    }

    public bool CheckFrontObject()
    {
        Vector3 faceCell = GetFrontCell();
        bool enemyFront;
        if (currentGrid.CheckCellBusy(faceCell) == LOLS_Tile.E_TileState.Ennemy)
        {
            enemyFront = true;
        }
        else
        {
            enemyFront = false;
        }

        return enemyFront;
    }

    private void checkDoorUI()
    {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, 8f, obstaclesLayer))
        {
            if (_hit.transform.gameObject.tag == "Door")
            {
                ui.GetComponent<LOLS_UIButtons>().ToggleDiceButton();
                tutorial.GetComponent<LOLS_Tutorial>().DoorFront();

            }
            else
            {
                ui.GetComponent<LOLS_UIButtons>().DisableDiceButton();
            }
        }
        else
        {
            ui.GetComponent<LOLS_UIButtons>().DisableDiceButton();
        }
    }



    void Update()
    {
        if (isMoving && !isRotating)
        {
            if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
            {
                isMoving = false;
                SetCurrentCell();
                checkDoorUI();

                currentGrid.SetCellBusy(lastCell, lastCellState);
                currentCellCheck = currentGrid.GetCurrentCellByPosition(transform.position);
                lastCell = currentCellCheck;
                lastCellState = currentGrid.CheckCellBusy(currentCellCheck);
                currentGrid.SetCellBusy(currentCellCheck, LOLS_Tile.E_TileState.Player);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPosition) >= 7.5f)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 20f, transform.position.z), moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 7f, transform.position.z), moveSpeed * Time.deltaTime);
                }
            }
        }
        if (isRotating && !isMoving)
        {
            if (Mathf.Abs(gameObject.transform.eulerAngles.y - targetEulerYRotation) < 0.1f)
            {
                isRotating = false;
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, targetEulerYRotation, gameObject.transform.eulerAngles.z);
                UpdateDirection();
            }
            else
            {
                Vector3 targetEulerRotation = new Vector3(gameObject.transform.eulerAngles.x, targetEulerYRotation, gameObject.transform.eulerAngles.z);
                gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, targetEulerRotation, rotateSpeed * Time.deltaTime);
            }
        }
    }
}
