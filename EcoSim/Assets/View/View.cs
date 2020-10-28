using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private int _sizeGroupOne;
    private int _sizeGroupTwo;

    public AnimalGroupView GroupOne;
    public AnimalGroupView GroupTwo;

    void Start()
    {

    }

    public void Initialize(int sizeOne, int sizeTwo)
    {
        _sizeGroupOne = sizeOne;
        _sizeGroupTwo = sizeTwo;

        GroupOne = Instantiate(GroupOne);
        GroupTwo = Instantiate(GroupTwo);

        GroupOne.Initialize(_sizeGroupOne, Color.blue);
        GroupTwo.Initialize(_sizeGroupTwo, Color.red);
    }

    public void UpdatePositions(List<Vector3> positionsGrOne, List<Vector3> positionsGrTwo)
    {
        GroupOne.UpdatePositions(positionsGrOne);
        GroupTwo.UpdatePositions(positionsGrTwo);
    }
}
