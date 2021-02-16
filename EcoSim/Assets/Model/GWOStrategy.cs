using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
public class GWOStrategy : HuntingStrategy, MetaHeuristic
{
    private int METAHEURISTIC_ITERATIONS = 50;
    private int METAHEURISTIC_CANDIDATES = 6; //MUST BE >= 3
    private int SIZE_OF_SPACE = 10;
    private Vec3 _desiredPosition = new Vec3(0, 0, 0);
    private Boolean fixedPosition = false;

    private class CandidateSolution
    {
        private Vec3 _solution;
        public Vec3 Solution { get => _solution; set => _solution = value; }

        private double _fitness;
        public double Fitness{ get => _fitness; set => _fitness = value; }

        public CandidateSolution(Vec3 solution, double fitness)
        {
            this.Solution = solution;
            this.Fitness = fitness;
        }
    }

    public override void Hunt(Animal agent, Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes)
    {
        if(agent.AnimalMediator.FixedPreyId == -1)
        {
            agent.AnimalMediator.UpdateBestPreyId(foes);
            this.fixedPosition = false;
        }
        else if(!this.fixedPosition || agent.Position.SquaredDistanceTo(this._desiredPosition) < 9)
        {

            Debug.Log("Calculamos");
            //Get all the positions
            List<Vec3> predatorPositions = new List<Vec3>();

            foreach (Animal a in friendly.Values)
            {
                if (agent.Id == a.Id)
                {
                    predatorPositions.Add(a.Position);
                }
            }

            //Get data from fixedPrey
            Animal fixedPrey = foes[agent.AnimalMediator.FixedPreyId];

            //Calculate optimal position of the predator agent
            this._desiredPosition= this.GreyWolfOptimizer(agent, predatorPositions, fixedPrey);

            //Update the predator Speed
            Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(agent.Position, this._desiredPosition);
            acceleration.Expand(agent.MaxSpeed);
            agent.UpdateSpeed(acceleration);
            agent.Move();

            this.fixedPosition = true;

            Debug.Log("Lobo: " + agent.Id + " va a la posicion (" + this._desiredPosition.XCoord + "," + this._desiredPosition.ZCoord);
        }
        else
        {
            Vec3 acceleration = Vec3.CalculateVectorsBetweenPoints(agent.Position, this._desiredPosition);
            acceleration.Expand(agent.MaxSpeed);
            agent.UpdateSpeed(acceleration);
            agent.Move();
        }

    }


    public Vec3 GreyWolfOptimizer(Animal agent, List<Vec3> predatorPositions, Animal fixedPrey)
    {
        System.Random rand = new System.Random();

        //Declare the space of solutions
        double xUpperLimit = agent.Position.XCoord + this.SIZE_OF_SPACE;
        double xLowerLimit = agent.Position.XCoord - this.SIZE_OF_SPACE;
        double zUpperLimit = agent.Position.ZCoord + this.SIZE_OF_SPACE;
        double zLowerLimit = agent.Position.ZCoord - this.SIZE_OF_SPACE;

        //Initialize the candidate solutions in the space of solutions
        List<CandidateSolution> candidates = new List<CandidateSolution>();

        for(int i=0; i<=this.METAHEURISTIC_CANDIDATES; i++)
        {
            Vec3 candidateSolution = new Vec3(xUpperLimit, xLowerLimit, 0, 0, zUpperLimit, zLowerLimit, rand);
            double fitness = this.CalculateFitness(candidateSolution, predatorPositions, fixedPrey);
            candidates.Add(new CandidateSolution(candidateSolution, fitness));
        }

        //Main GWO algorithm loop
        for(int i=0; i<=this.METAHEURISTIC_ITERATIONS; i++)
        {
            //Order the candidates by fitness (the lower the better)
            candidates = candidates.OrderBy(c => c.Fitness).ToList();
            //Fittest Solutions
            Vec3 alphaSol = candidates[0].Solution.Clone();
            Vec3 betaSol = candidates[1].Solution.Clone();
            Vec3 gammaSol = candidates[2].Solution.Clone();

            Debug.Log("Mejor fitness iteracion" + i + "lobo "+ agent.Id +" : " + candidates[0].Fitness);
            //Debug.Log("Segunda fitness iteracion" + i + "lobo " + agent.Id + " : " + candidates[1].Fitness);
            //Debug.Log("Tercera fitness iteracion" + i + "lobo " + agent.Id + " : " + candidates[2].Fitness);
            for (int j = 3; j < candidates.Count; j++)
            {
                Vec3 omegaSolution = candidates[j].Solution;
                Vec3 X1 = CalculateVectorX(omegaSolution, alphaSol, rand, i);
                Vec3 X2 = CalculateVectorX(omegaSolution, betaSol, rand, i);
                Vec3 X3 = CalculateVectorX(omegaSolution, gammaSol, rand, i);

                //(X1+X2+X3)/3
                X1.Add(X2);
                X1.Add(X3);
                X1.Divide(3);

                //Update the omegaSolution
                candidates[j].Solution = X1;
                candidates[j].Fitness = this.CalculateFitness(X1, predatorPositions, fixedPrey);
            }
        }

        //Return the optimal solution found
        candidates = candidates.OrderBy(c => c.Fitness).ToList();
        return candidates[0].Solution;
    }

    //To calculate X1, X2, X3 form the gwo equations
    private Vec3 CalculateVectorX(Vec3 candidate, Vec3 optimalSolution, System.Random rand, int iteration)
    {
        //Random vectors (coords between 0 and 1)
        Vec3 r1 = new Vec3(1, 0, 0, 0, 1, 0, rand);

        //C = 2r2, in this line we calculate r2 and C at the same time
        Vec3 C = new Vec3(1, 0, 0, 0, 1, 0, rand);
        C.Multiply(2);

        //Scalar a (coords go from 2 to 0 linearly during the iterations)
        double coord = 2 - (iteration * (2 / this.METAHEURISTIC_ITERATIONS));
        Vec3 a = new Vec3(coord, 0, coord);

        //A vector A = 2*a*r1 - a
        Vec3 A = Vec3.WolfProduct(a, r1);
        A.Multiply(2);
        A.Substract(a);

        //Calculate distance (C-Xalpha or C-Xbeta or C-Xamma)
        //optDistance = Module(C*optimalSolution - candidate)
        Vec3 dVector = Vec3.WolfProduct(C, optimalSolution);
        dVector.Substract(candidate);
        double optDistance = dVector.Module;

        //Calculate XVector=optimalSolution - optDistance*A
        A.Multiply(optDistance);
        Vec3 vectorX = Vec3.Substract(optimalSolution, A);

        return vectorX;
    }

    private Vec3 PredictPreyPosition(Vec3 candidatePosition, Vec3 preyPosition, double maxSquaredPreySpeed, double preyVisionRadius)
    {
        //Calculate where the prey would move if the agent moves to the possible position 
        Vec3 preyDirection = Vec3.CalculateVectorsBetweenPoints(candidatePosition, preyPosition);
        //double distance = preyDirection.Module;
        preyDirection.Trim(maxSquaredPreySpeed);

        //double force = preyVisionRadius / distance;
        //preyDirection.Multiply(force);

        return Vec3.Add(preyDirection, preyPosition);
    }


    public double ObjectiveFunction(List<Vec3> predatorPositions, Vec3 preyPosition)
    {
        //We want to minimize the minimun distance of all the predators to the prey
        double maxDistance = 0;
        double minDistance = double.MaxValue;

        //Calculate the distance
        foreach(Vec3 position in predatorPositions)
        {
            double sqrdDistance = position.SquaredDistanceTo(preyPosition);
            if (sqrdDistance < minDistance)
            {
                minDistance = sqrdDistance;
            }
        }

        return minDistance;

    }

    public double ObjFunc(Vec3 pos, Vec3 preyPosition)
    {
        return Vec3.CalculateVectorsBetweenPoints(pos, preyPosition).Module;
    }


    private double CalculateFitness(Vec3 candidateSolution, List<Vec3> predatorPositions, Animal prey)
    {
        Vec3 predictedPreyPosition = this.PredictPreyPosition(candidateSolution, prey.Position, prey.MaxSquaredSpeed, prey.VisionRadius);
        return ObjFunc(candidateSolution, prey.Position);
    }
}
