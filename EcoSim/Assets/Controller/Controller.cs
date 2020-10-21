using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Ecosystem _ecosystem;
    private View _myView;
    // Start is called before the first frame update
    void Start()
    {
        this._ecosystem = new Ecosystem();
        this._myView = Instantiate(_myView);
    }

    // Update is called once per frame
    void Update()
    {
        this._ecosystem.Update();
    }
}
