using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Ecosystem ecosystem;
    // Start is called before the first frame update
    void Start()
    {
        this.ecosystem = new Ecosystem();
    }

    // Update is called once per frame
    void Update()
    {
        this.ecosystem.Update();
    }
}
