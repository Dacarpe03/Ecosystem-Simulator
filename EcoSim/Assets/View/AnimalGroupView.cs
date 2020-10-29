using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalGroupView : MonoBehaviour
{
    private int _size;
    public AnimalView Agent;
    public List<AnimalView> Animals;
    void Start()
    {

    }

    public void Initialize(int size, Color color)
    {
        this._size = size;

        for (int i=0; i<this._size; i++) 
        {
            Agent = Instantiate(Agent);
            Agent.Initialize(color);
            Animals.Add(Agent);
        }

    }

    public void UpdatePositions(List<Vector3> newPositions)
    {
        for (int i=0; i<this._size; i++)
        {
            this.Animals.ElementAt(i).UpdatePosition(newPositions.ElementAt(i));
        }
    }

    public void Reset()
    {
        foreach(AnimalView av in Animals)
        {
            Destroy(av);
        }
    }
}
