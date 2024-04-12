using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Door : MonoBehaviour
{
    private bool isLocked = true;
    private bool isClosed = true;
    private bool isBossClosed = false;
    private GameObject character;
    [SerializeField] private bool isBossDoor = false;

    public bool CheckIfBossDoor()
    {
        return isBossDoor;
    }

    public void ToggleDoor()
    {
        if(isLocked)
        {
            if(!isBossClosed)
            {
                isLocked = false;
            }
        }

        if(isClosed && !isLocked)
        {
            transform.GetChild(0).transform.localEulerAngles = new Vector3(transform.GetChild(0).transform.localEulerAngles.x, 90f, transform.GetChild(0).transform.localEulerAngles.z);
            transform.GetChild(1).transform.localEulerAngles = new Vector3(transform.GetChild(1).transform.localEulerAngles.x, -90f, transform.GetChild(1).transform.localEulerAngles.z);
            isClosed = false;
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
        transform.GetChild(0).transform.localEulerAngles = new Vector3(transform.GetChild(0).transform.localEulerAngles.x, 0f, transform.GetChild(0).transform.localEulerAngles.z);
        transform.GetChild(1).transform.localEulerAngles = new Vector3(transform.GetChild(1).transform.localEulerAngles.x, 0f, transform.GetChild(1).transform.localEulerAngles.z);
        isClosed = true;

        if(isBossDoor)
        {
            foreach (GameObject _door in GameObject.FindGameObjectsWithTag("Door"))
            {
                if(_door.GetComponent<LOLS_Door>().CheckIfBossDoor() &&  _door != this.gameObject)
                {
                    _door.GetComponent<LOLS_Door>().isLocked = true;
                    _door.GetComponent<LOLS_Door>().isBossClosed = true;
                }
            }
            isLocked = true;
            isBossClosed = true;
        }
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
