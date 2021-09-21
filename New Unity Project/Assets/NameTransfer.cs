using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameTransfer : MonoBehaviour
{
    // Start is called before the first frame update
    public string theName;
    public string theOption;
    public GameObject inputField;
    public GameObject Scroll;
    public GameObject canvas;

    public void StoreName()
    {
        theName = inputField.GetComponent<Text>().text;
        theOption = Scroll.GetComponent<Text>().text;
        Destroy(canvas);
    }

    public void Update()
    {
        Debug.Log("Welcome " + theName + "to the game." + theOption);
    }
}
