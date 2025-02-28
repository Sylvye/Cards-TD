using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public void PurgeStowaways()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Transform grandchild = child.GetChild(0);
                Destroy(grandchild.gameObject);
            }
        }
    }
}
