using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_PlayerController : MonoBehaviour
{
    private LOLS_MoveCharacter character;
    private LOLS_CharacterAttack characterAttack;
    void Awake()
    {
        character = GetComponent<LOLS_MoveCharacter>();
        characterAttack = GetComponent<LOLS_CharacterAttack>();
    }
    public void Move(string _moveOrientation)
    {
        character.Move(_moveOrientation);
    }

    public void Rotate(string _rotationOrientation)
    {
        character.Rotate(_rotationOrientation);
    }

    public void SimpleAttack()
    {
        characterAttack.TriggerSimpleAttack();
    }

    public void Crochetage()
    {
        character.TryToggleDoor();
    }
}