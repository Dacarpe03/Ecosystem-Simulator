using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private int _sizeGroupOne;
    private int _sizeGroupTwo;

    private AnimalGroupView _groupOne;
    private AnimalGroupView _groupTwo;

    public AnimalGroupView GroupDummy;

    void Start()
    {

    }

    public void Initialize(int sizeOne, int sizeTwo)
    {
        _sizeGroupOne = sizeOne;
        _sizeGroupTwo = sizeTwo;

        _groupOne = Instantiate(GroupDummy);
        _groupTwo = Instantiate(GroupDummy);

        _groupOne.Initialize(_sizeGroupOne, Color.blue);
        _groupTwo.Initialize(_sizeGroupTwo, Color.red);
    }

    public void UpdatePositions(List<Vector3> positionsGrOne, List<Vector3> positionsGrTwo)
    {
        _groupOne.UpdatePositions(positionsGrOne);
        _groupTwo.UpdatePositions(positionsGrTwo);
    }

    public void Reset()
    {
        this._groupOne.Reset();
        this._groupTwo.Reset();
    }
}
