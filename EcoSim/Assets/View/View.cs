using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public AnimalGroupView _group1;
    public AnimalGroupView _group2;

    public View()
    {
        this._group1 = Instantiate(_group1);
        this._group2 = Instantiate(_group2);
    }
}
