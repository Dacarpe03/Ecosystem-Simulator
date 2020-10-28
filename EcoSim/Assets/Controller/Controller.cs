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
        List<Vector3> preyModelPositions = this.GetModelPositions(this._ecosystem.Preys);
        List<Vector3> predatorModelPositions = this.GetModelPositions(this._ecosystem.Predators);
        this._myView.UpdatePositions(preyModelPositions, predatorModelPositions);
    }

    public List<Vector3> GetModelPositions(AnimalGroup modelGroup)
    {
        List<Vec3> modelPositions = modelGroup.GetPositions();
        List<Vector3> viewPositions = this.TransformToVector3(modelPositions);

        return viewPositions;
    }

    public List<Vector3> TransformToVector3(List<Vec3> vectors)
    {
        List<Vector3> newVectors = new List<Vector3>();
        foreach(Vec3 v in vectors)
        {
            Vector3 vNew = new Vector3((float)v.XCoord, (float)v.YCoord, (float)v.ZCoord);
            newVectors.Add(vNew);
        }

        return newVectors;
    }
}

