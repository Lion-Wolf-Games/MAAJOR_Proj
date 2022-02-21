using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompositeControlTile : MonoBehaviour
{
    [SerializeField] private ControlTile tileUp,tileDown,tileLeft,tileRight;
    [SerializeField] private TextMeshProUGUI actionName;
    //[SerializeField] private TextMeshProUGUI keyPath;
    [SerializeField] private ControlSkin skin;

    public void SetUpTile(string name,string keyU,string keyD,string keyL,string keyR)
    {
        actionName.text = name;
        //keyPath.text = key;
        tileUp.SetUpTile("Up",keyU);
        tileDown.SetUpTile("Down",keyD);
        tileLeft.SetUpTile("Left",keyL);
        tileRight.SetUpTile("Right",keyR);

    }
}
