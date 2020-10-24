using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Ecosystem _ecosystem;

    public View _myView;
    // Start is called before the first frame update
    void Start()
    {
        this._ecosystem = new Ecosystem();
        _myView = Instantiate(_myView);
        _myView.Initialize(100, 6);
    }

    // Update is called once per frame
    void Update()
    {
        this._ecosystem.Update();
        List<List<Vector3>> positions = this.GetModelPositions();
        this._myView.UpdatePositions(positions.ElementAt(0), positions.ElementAt(1));
    }

    public List<List<Vector3>> GetModelPositions()
    {
        List<List<Vector3>> positions = new List<List<Vector3>>();
        List<Vec3> preys = this._ecosystem.GetPreyPositions();
        List<Vec3> predators = this._ecosystem.GetPredatorPositions();

        List<Vector3> preyPositions = this.TransformToVector3(preys);
        List<Vector3> predatorPositions = this.TransformToVector3(predators);

        positions.Add(preyPositions);
        positions.Add(predatorPositions);

        return positions;
    }

    public List<Vector3> TransformToVector3(List<Vec3> vectors)
    {
        List<Vector3> newVectors = new List<Vector3>();
        foreach(Vec3 v in vectors)
        {
            Vector3 vNew = new Vector3(v.XCoord, v.YCoord, v.ZCoord);
            newVectors.Add(vNew);
        }

        return newVectors;
    }
}

