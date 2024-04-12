using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_ItemContainer : MonoBehaviour
{
    public enum E_ContainerState
    {
        None,
        Busy
    }

    public struct S_ItemContainer
    {
        public E_ContainerState ContainerState;
        public LOLS_Item.E_ItemType ItemContainerType;
        private Sprite itemContainerSprite;
    }

    public S_ItemContainer ItemContainer;
}
