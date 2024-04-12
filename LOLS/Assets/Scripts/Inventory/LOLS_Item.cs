using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Item : MonoBehaviour
{
    public enum E_ItemType
    {
        LockPick,
        ManaPotion,
        HealthPotion
    }

    public struct S_Item
    {
        public E_ItemType ItemType;
        private Sprite itemSprite;
    }

    public S_Item Item;
}
