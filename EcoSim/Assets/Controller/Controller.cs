using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /*
    private double PREY_REPRODUCTION_PROB = 1;
    private double PREDATOR_REPRODUCTION_PROB = 1;

    private double PREY_MAX_SPEED = 0.5;
    private double PREDATOR_MAX_SPEED = 0.6;

    private double PREY_VISION_RADIUS = 8;
    private double PREDATOR_VISION_RADIUS = 15;

    private int PREY_GROUP_SIZE = 500 ;
    private int PREDATOR_GROUP_SIZE = 15;
     */

    //PARAMETERS OF SIMULATION
    private int NUMBER_OF_SIMULATIONS = 500;
    private int ITERATIONS_PER_SIMULATION = 100;

                                               //Reproduction probability, maximum speed, visionRadius, GroupSize
    private GroupParameters _preyParameters = new GroupParameters(1, 0.5, 8, 500);
    private GroupParameters _predatorParameters = new GroupParameters(1, 0.6, 15, 15);
    //END PARAMETERS OF SIMULATION

    //PATHS FOR FILES
    private string PATH;
    private string _currentFileName;
    private int _totalSimulations = 1;
    //END PATHS FOR FILES

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

        //Initialize the ecosystem

        this._ecosystem = new Ecosystem(this._preyParameters, this._predatorParameters);
        
        //Initialize the view
        this._myView = Instantiate(MyView);
        this._myView.Initialize(this._preyParameters.GroupSize, this._predatorParameters.GroupSize);
        this.CalculateTotalSimulations();

        this.PATH = Application.dataPath + "/SimulationData/SimulationDataPhase2/";
        this.CreateFileForSimulation();
    }//End Start


    // Update is called once per frame
    void Update()
    {
        //If we haven't completed the number of iterations fixed or there is no animal group extinguised we keep on the loop
        if (this._ecosystem.Iteration < ITERATIONS_PER_SIMULATION & !this._ecosystem.Extinguised)
        {
            //If the ecosystem has resseted (the iteration has finished), then we reset the view and save the data in the file
            if (this._ecosystem.Reset)
            {
                this.UpdateFile();
                this._ecosystem.Update();
                this.ResetView();
            }
            //If not we update the ecosystem (the next step/frame of the iteration)
            else
            {
                //Update the model
                this._ecosystem.Update();

                //Pass the information of positions from model to view
                List<Vector3> preyModelPositions = this.TransformToVector3(this._ecosystem.GetPreyPositions());
                List<Vector3> predatorModelPositions = this.TransformToVector3(this._ecosystem.GetPredatorPositions());
                this._myView.UpdatePositions(preyModelPositions, predatorModelPositions);
            }
        }
        //If the simulation has finished, we start another one if necessary
        else if (this._simulationCounter < this.NUMBER_OF_SIMULATIONS)
        {
            this._simulationCounter++;
            this._totalSimulations += 1;
            Debug.Log("Simulación " + this._simulationCounter);

            //Initialize the ecosystem
            this._ecosystem = new Ecosystem(this._preyParameters, this._predatorParameters);
            //Reset the view
            this.ResetView();
            //Create a new file to save the data of the simulation
            this.CreateFileForSimulation();
        }
        else
        {
            Debug.Log("Fin de las simulaciones");
            Destroy(this.gameObject);
        }
    }//End Update

    //Transforms from Vec3 to Vector3
    public List<Vector3> TransformToVector3(List<Vec3> vectors)
    {
        List<Vector3> newVectors = new List<Vector3>();
        foreach(Vec3 v in vectors)
        {
            Vector3 vNew = new Vector3((float)v.XCoord, (float)v.YCoord, (float)v.ZCoord);
            newVectors.Add(vNew);
        }

        return newVectors;
    }//End TransformToVector3


    //Resets the view at the end of an iteration
    public void ResetView()
    {
        //Destroys the current one
        this._myView.Reset();
        Destroy(this._myView.gameObject);

        //Initializes another one
        this._myView = Instantiate(MyView);
        this._myView.Initialize(this._ecosystem.Preys.Size, this._ecosystem.Predators.Size);
    }//End ResetView


    //Creates a file to save the data from a simulation
    public void CreateFileForSimulation()
    {
        //Create the name of the file
        var dt = DateTime.Now;
        string date = dt.ToString("yyyy-MM-dd");
        string fileName = "Stage2-Simulation" + this._totalSimulations + ".txt";

        this._currentFileName = this.PATH + fileName;

        //Write the first lines of the file
        if (!File.Exists(this._currentFileName))
        {
            StreamWriter sr = File.CreateText(this._currentFileName);
            sr.WriteLine(date);
            sr.WriteLine("Parameters (in next line): Iterations|PreyReproductionRate|PreyMaxSpeed|PreyVisionRadius|PredatorReproductionRate|PredatorMaxSpeed|PredatorVisionRadius");
            sr.WriteLine(this.ITERATIONS_PER_SIMULATION
                        + "|" + this._preyParameters.toString()
                        + "|" + this._predatorParameters.toString());

            sr.WriteLine("Iteracion|InicialPresas|InicialPredadores|SupervivientesPresas|SupervivientesPredadores");
            sr.Close();
        }
    }//End CreateFileForSimulation


    //Saves the data from an iteration
    public void UpdateFile()
    {
        if (File.Exists(this._currentFileName))
        {
            StreamWriter sr = File.AppendText(this._currentFileName);
            sr.WriteLine(this._ecosystem.Iteration 
                + "|" + this._ecosystem.Preys.Size 
                + "|" + this._ecosystem.Predators.Size
                + "|" + this._ecosystem.Preys.SurvivorsNumber 
                + "|" + this._ecosystem.Predators.SurvivorsNumber);
            sr.Close();
        }
    }//End UpdateFile

    //TODO: No funciona bien, sobreescribe si se lanzan las simulaciones en distintas ejecuciones
    //Calculates the total number of iterations so that we dont overwrite the files of previous simulations
    private void CalculateTotalSimulations()
    {
        string fileName = "Stage2-Simulation" + this._totalSimulations + ".txt";
        this._currentFileName = this.PATH + fileName;

        while (File.Exists(this._currentFileName))
        {
            Debug.Log("Existe");
            this._totalSimulations++;
            fileName = "Stage2-Simulation" + this._totalSimulations + ".txt";
            this._currentFileName = this.PATH + fileName;

        }//End CalculateTotalSimulations
    }
}

    