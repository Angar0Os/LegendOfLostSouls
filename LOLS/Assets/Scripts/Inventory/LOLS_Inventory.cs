using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOLS_Inventory : MonoBehaviour
{
    public LOLS_ItemContainer[] Inventory;

    private void Start()
    {
        Inventory = new LOLS_ItemContainer[9];
    }
}
