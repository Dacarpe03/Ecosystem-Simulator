using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private int _sizeGroupOne;
    private int _sizeGroupTwo

    void Start()
    {
        
    }

    private void Initialize(int sizeOne, int sizeTwo)
    {
        _sizeGroupOne = sizeOne;
        _sizeGroupTwo = sizeTwo;
    }
}
