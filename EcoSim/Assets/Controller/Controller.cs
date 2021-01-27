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
    private int NUMBER_OF_SIMULATIONS = 500;
    private int ITERATIONS_PER_SIMULATION = 100;

    private double PREY_REPRODUCTION_PROB = 1;
    private double PREDATOR_REPRODUCTION_PROB = 1;

    private double PREY_MAX_SPEED = 0.5;
    private double PREDATOR_MAX_SPEED = 0.6;

    private double PREY_VISION_RADIUS = 8;
    private double PREDATOR_VISION_RADIUS = 15;

    private int PREY_GROUP_SIZE = 500 ;
    private int PREDATOR_GROUP_SIZE = 15;
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

        //TODO: Create a class that contains all the initial paremeters of a group
        //Initialize the ecosystem
        this._ecosystem = new Ecosystem(PREY_GROUP_SIZE, PREY_MAX_SPEED, PREY_VISION_RADIUS, PREY_REPRODUCTION_PROB, PREDATOR_GROUP_SIZE, PREDATOR_MAX_SPEED, PREDATOR_VISION_RADIUS, PREDATOR_REPRODUCTION_PROB);
        
        //Initialize the view
        this._myView = Instantiate(MyView);
        this._myView.Initialize(PREY_GROUP_SIZE, PREDATOR_GROUP_SIZE);

        this.PATH = Application.dataPath + "/SimulationData/";
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
                List<Vector3> preyModelPositions = this.GetModelPositions(this._ecosystem.Preys);
                List<Vector3> predatorModelPositions = this.GetModelPositions(this._ecosystem.Predators);
                this._myView.UpdatePositions(preyModelPositions, predatorModelPositions);
            }
        }
        //If the simulation has finished, we start another one if necessary
        else if (this._simulationCounter < this.NUMBER_OF_SIMULATIONS)
        {
            this._simulationCounter++;
            Debug.Log("Simulación " + this._simulationCounter);

            //Initialize the ecosystem
            this._ecosystem = new Ecosystem(PREY_GROUP_SIZE, PREY_MAX_SPEED, PREY_VISION_RADIUS, PREY_REPRODUCTION_PROB, PREDATOR_GROUP_SIZE, PREDATOR_MAX_SPEED, PREDATOR_VISION_RADIUS, PREDATOR_REPRODUCTION_PROB);
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

    
    //TODO: Change this method so it is encapsulated, (for example return two lists of positions)
    //Gets all the positions of an animal group
    public List<Vector3> GetModelPositions(AnimalGroup modelGroup)
    {
        List<Vec3> modelPositions = modelGroup.GetPositions();
        List<Vector3> viewPositions = this.TransformToVector3(modelPositions);

        return viewPositions;
    }//End GetModelPositions

    
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


    //TODO: Save all the information of an iteration in the same line
    //TODO: Change the separators to only one character
    //TODO: Change the name of the file to stage_x_sim_y
    //Creates a file to save the data from a simulation
    public void CreateFileForSimulation()
    {
        //Create the name of the file
        var dt = DateTime.Now;
        string date = dt.ToString("MM-dd-yyyy-hh-mm");
        string fileName = date + "-simulation" + this._simulationCounter + ".txt";
        this._currentFileName = this.PATH + fileName;

        //Write the first lines of the file
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
            sr.Close();
        }
    }//End CreateFileForSimulation



    //TODO: Save all the information in one line
    //Saves the data from an iteration
    public void UpdateFile()
    {
        if (File.Exists(this._currentFileName))
        {
            StreamWriter sr = File.AppendText(_currentFileName);
            sr.WriteLine(this._ecosystem.Iteration + "||" + this._ecosystem.Preys.Size + "||" + this._ecosystem.Predators.Size);
            sr.WriteLine(this._ecosystem.Iteration + "||" + this._ecosystem.Preys.SurvivorsNumber + "||" + this._ecosystem.Predators.SurvivorsNumber);
            sr.Close();
        }
    }//End UpdateFile
}

    