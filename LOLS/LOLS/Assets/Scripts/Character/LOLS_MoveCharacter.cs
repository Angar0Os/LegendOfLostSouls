using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class LOLS_MoveCharacter : MonoBehaviour
{
    private LOLS_Grid currentGrid;
    private float moveSpeed = 5f;
    private float rotateSpeed = 5f;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private Vector3 currentCell;
    public Vector3 cellToMove = new Vector3(0,0,1);
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask obstaclesLayer;
    [SerializeField] private LayerMask gridLayer;
    private string tt;
    private LOLS_Door lastDoorOpened;
    private bool isRotating = false;
    private float targetEulerYRotation;

    void Start()
    {
        Invoke("Spawn", 1f);
    }

    private void Spawn()
    {
        FindNearestGrid();
        Vector3 cellPosition = new Vector3(0, 0, 0);
        Vector3 cellWorldPosition = currentGrid.GetWorldPositionOfCell(cellPosition, cellPosition);
        transform.position = cellWorldPosition;

        //FindOtherNearestGrid();
    }

    public Vector3 GetFrontCellGrid()
    {
        return currentCell = currentGrid.GetCurrentCellByPosition(transform.position) + cellToMove;
    }
    public void Move(string _moveOrientation)
        {
            
            if(!isMoving && !isRotating)
            {
            Debug.Log(currentGrid);
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

                

                if(CheckObstacles(targetPosition) == true)
                {
                    return;
                }
                else
                {
                    isMoving = true;
                }
            }
        }

    public void Rotate(string _rotationOrientation)
       {
            if(!isMoving && !isRotating)
            {
            int newRotationY = 0;
            switch (gameObject.transform.eulerAngles.y)
            {
                case 0:
                    newRotationY = (_rotationOrientation == "Right") ? 90 : 270; 
                    break;
                case 90:
                    newRotationY = (_rotationOrientation == "Right") ? 180 : 0; 
                    break;
                case 180:
                    newRotationY = (_rotationOrientation == "Right") ? 270 : 90; 
                    break;
                case 270:
                    newRotationY = (_rotationOrientation == "Right") ? 0 : 180; 
                    break;
            }
            targetEulerYRotation = newRotationY;
            isRotating = true;
            //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, newRotationY, gameObject.transform.eulerAngles.z);
            //UpdateDirection();
            }
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
            }
        }

    public void FindNearestGrid()
        {
            RaycastHit _hit;
        Debug.DrawLine(playerCam.transform.position, Vector3.down, Color.black, 20f);
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.down), out _hit, 8f, gridLayer))
            {
                currentGrid = _hit.transform.gameObject.GetComponent<LOLS_Grid>();
            }
        }

    public void FindOtherNearestGrid(Vector3 _direction)
        {
           // Vector3 test = _direction;
           // test -= new Vector3(0f,-0.5f,0f);
           // test.Normalize();
            RaycastHit _hit;
            Vector3 _rayDirection = playerCam.transform.TransformDirection(new Vector3(0f,-0.5f,0f) + Vector3.forward);
            if(Physics.Raycast(playerCam.transform.position, _rayDirection, out _hit, 64f, gridLayer))
            {
                Debug.DrawLine(playerCam.transform.position, _hit.point, Color.black, 20f);
                if(_hit.transform.gameObject != currentGrid)
                {
                     currentGrid = _hit.transform.gameObject.GetComponent<LOLS_Grid>();
                }
            }
        }


    public void TryToggleDoor()
         {
            RaycastHit _hit;
                if(Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out _hit, 8f, obstaclesLayer))
                {
                    Debug.DrawLine(playerCam.transform.position, _hit.transform.position, Color.magenta, 20f);
                    if(_hit.transform.tag == "Door")
                    {
                        lastDoorOpened = _hit.transform.gameObject.GetComponent<LOLS_Door>();
                        lastDoorOpened.ToggleDoor();
                        Debug.Log("TODO Crochetage");
                    }
                }
         }

    public bool CheckObstacles(Vector3 _cellToCheck)
        {
            RaycastHit _hit;
            Vector3 _rayDirection = Vector3.zero;
            switch (tt)
            {
                    case "Forward": _rayDirection = playerCam.transform.TransformDirection(Vector3.forward);
                        break;
                    case "Backward": _rayDirection = playerCam.transform.TransformDirection(Vector3.back);
                        break;
                    case "Right": _rayDirection = playerCam.transform.TransformDirection(Vector3.right);
                        break;
                    case "Left": _rayDirection = playerCam.transform.TransformDirection(Vector3.left);
                        break;
            }
            //Vector3 _rayDirection = (_cellToCheck - playerCam.transform.position).normalized;
            //Debug.DrawRay(playerCam.transform.position, playerCam.transform.TransformDirection(_rayDirection) * 8f, Color.blue, 15, false);
            Debug.DrawRay(playerCam.transform.position, _rayDirection * 8f, Color.blue, 15, false);
           // Debug.DrawLine(playerCam.transform.position,_cellToCheck, Color.blue, 20f);
           // if (Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(_rayDirection), out _hit, 8f, obstaclesLayer))
           if (Physics.Raycast(playerCam.transform.position, _rayDirection, out _hit, 8f, obstaclesLayer))
            {
                switch (_hit.transform.gameObject.tag)
                {
                    
                    default: 
                    return false;
                    case "Wall": 
                    return true;
                    case "Door":
                   // Debug.DrawLine(playerCam.transform.position, _hit.point, Color.yellow, 20f); 
                    if(_hit.transform.gameObject.GetComponent<LOLS_Door>().CheckIfClosed() == false)
                    {
                        if(currentGrid.GetCurrentEnemysSpawnerOnGrid() != null)
                        {
                            currentGrid.GetCurrentEnemysSpawnerOnGrid().OnRoomChanged(false);
                        }

                        FindOtherNearestGrid(_rayDirection);
                       // transform.position = currentGrid.GetCurrentCellByPosition(_hit.transform.position);
                        //currentCell = currentGrid.GetCurrentCellByPosition(transform.position);
                        Debug.DrawLine(playerCam.transform.position, _hit.point, Color.yellow, 20f); 
                        targetPosition = currentGrid.GetWorldPositionOfCell(currentGrid.GetCurrentCellByPosition(_hit.transform.position), currentGrid.GetCurrentCellByPosition(_hit.transform.position));
                        
                        //currentCell = currentGrid.GetCurrentCellByPosition(currentGrid.GetCurrentCellByPosition(_hit.transform.position));
                    }
                    return _hit.transform.gameObject.GetComponent<LOLS_Door>().CheckIfClosed();
                }
            }
            else
            {
                Debug.Log("pas de collision");
                return false;
            }
        }

        public void SetCurrentCell()
        {
            currentCell = currentGrid.GetCurrentCellByPosition(transform.position);
           // UpdateDirection();
        //    isMoving = false;
        }

        public void OnCellReached()
        {
            if(currentGrid.GetCurrentEnemysSpawnerOnGrid() != null)
            {
                currentGrid.GetCurrentEnemysSpawnerOnGrid().OnRoomChanged(true);
            }
        }


    void Update()
        {
          //  Debug.Log(gameObject.transform.eulerAngles.y + " " + isRotating);
            if(isMoving && !isRotating)
            {
                if(Vector3.Distance(transform.position, targetPosition) <= 0.1f)
                {              
                    isMoving = false;
                    SetCurrentCell();
                }
                else
                {
                   transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                }
            }
            if(isRotating && !isMoving)
            {
                if(Mathf.Abs(gameObject.transform.eulerAngles.y - targetEulerYRotation) < 0.1f)
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
