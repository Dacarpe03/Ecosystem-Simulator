using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private int num;

    void Start()
    {
        
    }

    private void Initialize(int num)
    {
        this.num = num;
        Debug.Log("Este es el num: " + num);
    }
}
