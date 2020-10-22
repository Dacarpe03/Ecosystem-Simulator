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
        Debug.Log("Coord x: " + newPosition.x + "Coord y: " + newPosition.y + "Coord z" + newPosition.z);
    }

    
}
