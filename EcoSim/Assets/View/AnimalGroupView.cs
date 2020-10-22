using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGroupView : MonoBehaviour
{
    private int _size;
    public AnimalView Agent;
    public List<Animal> Animals;
    void Start()
    {

    }

    public void Initialize(int size)
    {
        this._size = size;

        for (int i=0; i<this._size; i++) 
        { 
            Agent = Instantiate(Agent);
            Animals.Add(Agent);
        }

    }
}
