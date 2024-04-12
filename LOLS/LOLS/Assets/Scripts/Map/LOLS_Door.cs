using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Door : MonoBehaviour
{
    private bool isLocked = true;
    private bool isClosed = true;
    private GameObject character;
    [SerializeField] private bool isBossDoor = false;

    public void ToggleDoor()
    {
        if(isLocked)
        {
            isLocked = false;
        }

        if(isClosed)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 90f, transform.localEulerAngles.z);
            isClosed = false;
            //Invoke("AutoToggleDoor", 1.5f);
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
            isClosed = true;
        }
    }

    public bool CheckIfLocked()
    {
        return isLocked;
    }

    public bool CheckIfClosed()
    {
        return isClosed;
    }

    public void AutoCloseDoor()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
        isClosed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Character"))
        {
           character = collision.transform.gameObject;
           Invoke("AutoCloseDoor", 0.5f);
           Invoke("CharacterCollided", 1f);
        }
    }

    private void CharacterCollided()
    {
        character.transform.GetComponentInParent<LOLS_MoveCharacter>().SetCurrentCell();
        character.transform.GetComponentInParent<LOLS_MoveCharacter>().OnCellReached();
    }
}
