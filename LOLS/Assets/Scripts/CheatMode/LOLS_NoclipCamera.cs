using UnityEngine;

public class LOLS_NoclipCamera : MonoBehaviour
{
    private float movementSpeed = 25f;
    private float rotationSpeed = 2f;

    private Camera noclipCamera;
    private Camera mainCamera;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (noclipCamera != null)
        {
            MoveCamera();
            RotateCamera();
        }
    }

    public void EnableNoclip()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = Camera.main;
        gameObject.transform.position = mainCamera.transform.parent.transform.position;
        noclipCamera = GetComponent<Camera>();
        noclipCamera.enabled = true;
        rotationX = 0f;
        rotationY = 0f;
    }

    public void DisableNoclip()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (mainCamera != null)
        {
            noclipCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    void MoveCamera()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float moveSpeed = movementSpeed * Time.deltaTime;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVector = transform.TransformDirection(moveDirection) * moveSpeed;

        transform.position += moveVector;
    }

    void RotateCamera()
    {
        rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        noclipCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
