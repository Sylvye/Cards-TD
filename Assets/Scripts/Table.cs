using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public void PurgeStowaways()
    {
        foreach (Transform child in transform)
        {
            Transform grandchild = child.GetChild(0);
            if (grandchild != null)
            {
                Destroy(grandchild.gameObject);
            }
        }
    }
}
