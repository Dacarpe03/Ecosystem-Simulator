using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        this.transform.position = newPosition;
    }

    public void Initialize(Color color)
    {
        this.GetComponentInChildren<Renderer>().material.color = color;
    }
    
}
