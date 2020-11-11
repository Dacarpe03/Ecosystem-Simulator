using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //PATHS FOR FILES
    private string PATH;
    private string _currentFileName;
    //END PATHS FOR FILES


    //PARAMETERS OF SIMULATION
    private int NUMBER_OF_SIMULATIONS = 3;
    private int ITERATIONS_PER_SIMULATION = 5;

    private double PREY_REPRODUCTION_PROB = 1;
    private double PREDATOR_REPRODUCTION_PROB = 1;

    private double PREY_MAX_SPEED = 0.5;
    private double PREDATOR_MAX_SPEED = 0.6;

    private double PREY_VISION_RADIUS = 8;
    private double PREDATOR_VISION_RADIUS = 15;

    private int PREY_GROUP_SIZE = 50;
    private int PREDATOR_GROUP_SIZE = 1;
    //END PARAMETERS OF SIMULATION

    //COUNTER PARAMETERS
    private int _simulationCounter = 1;
    //END COUNTER PARAMETERS

    //MAIN ATTRIBUTES
    private Ecosystem _ecosystem;

    public View MyView;
    private View _myView;
    //END MAIN ATTRIBUTES

    void Start()
    {
        Debug.Log("Simulación " + this._simulationCounter);
        this._ecosystem = new Ecosystem(PREY_GROUP_SIZE, PREY_MAX_SPEED, PREY_VISION_RADIUS, PREY_REPRODUCTION_PROB, PREDATOR_GROUP_SIZE, PREDATOR_MAX_SPEED, PREDATOR_VISION_RADIUS, PREDATOR_REPRODUCTION_PROB);
        this._myView = Instantiate(MyView);
        this._myView.Initialize(PREY_GROUP_SIZE, PREDATOR_GROUP_SIZE);

        this.PATH = Application.dataPath + "/SimulationData/";
        this.CreateFileForSimulation();
    }

    // Update is called once per frame
    void Update()
    {
        if (this._ecosystem.Iteration < ITERATIONS_PER_SIMULATION & !this._ecosystem.Extinguised)
        {

            if (this._ecosystem.Reset)
            {
                this._ecosystem.Update();
                this.ResetView();
            }
            else
            {
                this._ecosystem.Update();
                List<Vector3> preyModelPositions = this.GetModelPositions(this._ecosystem.Preys);
                List<Vector3> predatorModelPositions = this.GetModelPositions(this._ecosystem.Predators);
                this._myView.UpdatePositions(preyModelPositions, predatorModelPositions);
            }
        }
        else if (this._simulationCounter < this.NUMBER_OF_SIMULATIONS)
        {
            Debug.Log("Simulación " + this._simulationCounter);
            this._simulationCounter++;
            this._ecosystem = new Ecosystem(PREY_GROUP_SIZE, PREY_MAX_SPEED, PREY_VISION_RADIUS, PREY_REPRODUCTION_PROB, PREDATOR_GROUP_SIZE, PREDATOR_MAX_SPEED, PREDATOR_VISION_RADIUS, PREDATOR_REPRODUCTION_PROB);
            this.ResetView();
        }
        else
        {
            Debug.Log("Fin de las simulaciones");
            Destroy(this.gameObject);
        }
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

    public void ResetView()
    {
        this._myView.Reset();
        Destroy(this._myView.gameObject);
        this._myView = Instantiate(MyView);
        this._myView.Initialize(this._ecosystem.Preys.Size, this._ecosystem.Predators.Size);
    }

    public void CreateFileForSimulation()
    {
        var dt = DateTime.Now;
        string date = dt.ToString("MM-dd-yyyy-hh-mm");
        string fileName = date + "-simulation" + this._simulationCounter + ".txt";
        this._currentFileName = this.PATH + fileName;
        if (!File.Exists(this._currentFileName))
        {
            StreamWriter sr = File.CreateText(_currentFileName);
            sr.WriteLine("Parameters (in next line): Iterations|PreyReproductionRate|PreyVisionRadius|PreyMaxSpeed|PredatorReproductionRate|PredatorVisionRadius|PredatorMaxSpeed");
            sr.WriteLine(this.ITERATIONS_PER_SIMULATION
                        + "||" + this.PREY_REPRODUCTION_PROB
                        + "||" + this.PREY_VISION_RADIUS
                        + "||" + this.PREY_MAX_SPEED
                        + "||" + this.PREDATOR_REPRODUCTION_PROB
                        + "||" + this.PREDATOR_VISION_RADIUS
                        + "||" + this.PREDATOR_MAX_SPEED);

            sr.WriteLine("Iteracion|InicialPresas|InicialPredadores");
            sr.WriteLine("Iteracion|SupervivientesPresas|SupervivientesPredadores");
            sr.WriteLine(this._ecosystem.Iteration + "||" + this._ecosystem.Preys.Size + "||" + this._ecosystem.Predators.Size);
            sr.Close();
        }
    }

    public void UpdateFile()
    {
        if (File.Exists(this._currentFileName))
        {
            StreamWriter sr = File.AppendText(_currentFileName);
            sr.WriteLine(this._ecosystem.Iteration + "||" + this._ecosystem.Preys.Size + "||" + this._ecosystem.Predators.Size);
            sr.Close();
        }
    }
}

    