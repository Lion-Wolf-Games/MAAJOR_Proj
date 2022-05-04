using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompositeControlTile : MonoBehaviour
{
    //[SerializeField] private ControlTile tileUp,tileDown,tileLeft,tileRight;
    [SerializeField] private GameObject controlTilePrefab;
    [SerializeField] private Transform controlTileParent;
    [SerializeField] private TextMeshProUGUI actionName;
    //[SerializeField] private TextMeshProUGUI keyPath;
    [SerializeField] private ControlSkin skin;

    private ControlTile[] controlsTile;

    public void SetUpTile(string name,List<string> keys)
    {
        actionName.text = name;
        //keyPath.text = key;
        // tileUp.SetUpTile("Up",keyU);
        // tileDown.SetUpTile("Down",keyD);
        // tileLeft.SetUpTile("Left",keyL);
        // tileRight.SetUpTile("Right",keyR);

        foreach (string keypath in keys)
        {
            SpawnTile("", keypath);
        }

        RectTransform rectT = gameObject.GetComponent<RectTransform>();

        rectT.sizeDelta = new Vector2(rectT.sizeDelta.x,100);

    }

    private void SpawnTile(string name, string keyPath)
    {
        ControlTile ct = Instantiate(controlTilePrefab,controlTileParent).GetComponent<ControlTile>();

        ct.SetUpTile("",keyPath);

    }
}
