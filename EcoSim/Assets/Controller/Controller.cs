using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public bool ready;
    public bool firstTime;
    //Paramenters for menu
    public GameObject menu;
        //Predators
    private double reproductionPredator;
    private double maxSpeedPredator;
    private double visionRadiusPredator;
    private int initialPopulationPredator;
    public GameObject reprodPredator;
    public GameObject speedPredator;
    public GameObject visionPredator;
    public GameObject initialPredator;
        //Preys
    private double reproductionPrey;
    private double maxSpeedPrey;
    private double visionRadiusPrey;
    private int initialPopulationPrey;
    public GameObject reprodPrey;
    public GameObject speedPrey;
    public GameObject visionPrey;
    public GameObject initialPrey;

    //PARAMETERS OF SIMULATION
    private int NUMBER_OF_SIMULATIONS = 1;
    private int ITERATIONS_PER_SIMULATION = 200;

    private double INITIAL_PLANTS = 1200;
    private double GROWTH_RATE = 1.7;
    private double THRESHOLD = 200;
    //Reproduction probability, maximum speed, visionRadius, GroupSize
    private GroupParameters _preyParameters;
    private GroupParameters _predatorParameters;
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

    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 30;
    }

    void Start()
    {
        this.ready = false;
        this.firstTime = true;

        this.reproductionPredator = 0.3;
        this.maxSpeedPredator = 0.6;
        this.visionRadiusPredator = 15;
        this.initialPopulationPredator = 12;

        this.reproductionPrey = 0.9;
        this.maxSpeedPrey = 0.55;
        this.visionRadiusPrey = 8;
        this.initialPopulationPrey = 500;
    }
    // Update is called once per frame
    void Update()
    {
        if (this.ready)
        {
            Debug.Log("Updateamos");
            if (this.firstTime) {
                this.firstTime = false;
                Debug.Log("Simulación " + this._simulationCounter);

                this._predatorParameters = new GroupParameters(this.reproductionPredator, this.maxSpeedPredator, this.visionRadiusPredator, this.initialPopulationPredator);
                this._preyParameters = new GroupParameters(this.reproductionPrey, this.maxSpeedPrey, this.visionRadiusPrey, this.initialPopulationPrey);
                //Initialize the ecosystem
                Resource plants = new Resource(INITIAL_PLANTS, GROWTH_RATE, THRESHOLD);
                this._ecosystem = new Ecosystem(this._preyParameters, this._predatorParameters, plants);

                //Initialize the view
                this._myView = Instantiate(MyView);
                this._myView.Initialize(this._preyParameters.GroupSize, this._predatorParameters.GroupSize);
                this.CalculateTotalSimulations();

                this.PATH = Application.dataPath + "/SimulationData/SimulationDataPhase2/";
                Debug.Log(this.PATH);
                this.CreateFileForSimulation();
            }

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
                Resource plants = new Resource(INITIAL_PLANTS, GROWTH_RATE, THRESHOLD);
                this._ecosystem = new Ecosystem(this._preyParameters, this._predatorParameters, plants);
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
        }
    }//End Update

    //Transforms from Vec3 to Vector3
    public List<Vector3> TransformToVector3(List<Vec3> vectors)
    {
        List<Vector3> newVectors = new List<Vector3>();
        foreach (Vec3 v in vectors)
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
            sr.WriteLine("Parameters (in next line): Iterations|PreyReproductionRate|PreyMaxSpeed|PreyVisionRadius|PredatorReproductionRate|PredatorMaxSpeed|PredatorVisionRadius|InitialPlants|GrowthRate");
            sr.WriteLine(this.ITERATIONS_PER_SIMULATION
                        + "|" + this._preyParameters.toString()
                        + "|" + this._predatorParameters.toString()
                        + "|" + INITIAL_PLANTS
                        + "|" + GROWTH_RATE
                        );

            sr.WriteLine("Iteracion|InicialPresas|InicialPredadores|SupervivientesPresas|SupervivientesPredadores|Plantas|PlantasSupervivientes");
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
                + "|" + this._ecosystem.Predators.SurvivorsNumber
                + "|" + this._ecosystem.Preys.CurrentFood
                + "|" + (this._ecosystem.Preys.CurrentFood - this._ecosystem.Preys.SurvivorsNumber)
                );
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

    public void InitiateParameters() {
        this.ready = true;

        //Predator parameters
        string strRepPred = reprodPredator.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strRepPred)) {
            this.reproductionPredator = Convert.ToDouble(strRepPred);
        }
        string strSpeedPred = speedPredator.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strSpeedPred))
        {
            this.maxSpeedPredator = Convert.ToDouble(strSpeedPred);
        }
        string strVisionPred = visionPredator.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strVisionPred))
        {
            this.visionRadiusPredator = Convert.ToDouble(strVisionPred);
        }
        string strInitPredator = initialPredator.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strInitPredator))
        {
            this.initialPopulationPredator = Convert.ToInt32(strInitPredator);
        }

        //Prey parameters
        string strRepPrey = reprodPrey.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strRepPrey))
        {
            this.reproductionPrey = Convert.ToDouble(strRepPrey);
        }
        string strSpeedPrey = speedPrey.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strSpeedPrey))
        {
            this.maxSpeedPrey = Convert.ToDouble(strSpeedPrey);
        }
        string strVisionPrey = visionPrey.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strVisionPrey))
        {
            this.visionRadiusPrey = Convert.ToDouble(strVisionPrey);
        }
        string strInitPrey = initialPrey.GetComponent<Text>().text;
        if (!string.IsNullOrEmpty(strInitPrey))
        {
            this.initialPopulationPrey = Convert.ToInt32(strInitPrey);
        }


        Destroy(menu);
    }
}

    