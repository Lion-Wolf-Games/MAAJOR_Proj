using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiControlDisplay : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputMap;
    [SerializeField] private Transform controleTileParent;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject compositeTilePrefab;
    [SerializeField] private string currentcontrolScheme;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Dictionary<string,List<GameObject>> controlTileGroup;

    void Start()
    {

        currentcontrolScheme = playerInput.currentControlScheme;
        InputActionMap actionMap = inputMap.FindActionMap("Player");
        controlTileGroup = new Dictionary<string, List<GameObject>>();

        if(actionMap != null)
        {
            for (int i = 0; i < actionMap.actions.Count; i++)
            {
                SetUpActionTiles(actionMap.actions[i]);
            }
        }

        foreach (GameObject ct in controlTileGroup[currentcontrolScheme])
        {
            ct.SetActive(true);
        }

    }

    private void Update() {

        if(!playerInput.isActiveAndEnabled) return;

        if(currentcontrolScheme != playerInput.currentControlScheme)
        {
            OnControlChanged();
        }
    }

    public void OnControlChanged()
    {
        string newscheme = playerInput.currentControlScheme;

        if(controlTileGroup.ContainsKey(newscheme))
        {
            foreach (GameObject ct in controlTileGroup[currentcontrolScheme])
            {
                ct.SetActive(false);
            }

            foreach (GameObject ct in controlTileGroup[newscheme])
            {
                ct.SetActive(true);
            }

            currentcontrolScheme = newscheme;
        }
    }
    
    public void SetUpActionTiles(InputAction action)
    {
        string inputName = action.name;

        for (int f = 0; f < action.bindings.Count; f++)
        {
            string group;

            if(action.bindings[f].isComposite)
            {
                group = action.bindings[f+1].groups;
            }
            else
            {
                group = action.bindings[f].groups;
            }

            if(group.StartsWith(";"))
            {
                group = group.Remove(0,1);
            }
            
            string tileName ="Control Tile " + inputName + " " + group;


            if (action.bindings[f].isComposite)
            {
                //Create CompositeTile
                var go = CreateTile(compositeTilePrefab,tileName,group);

                //Get and set Controls info
                string keyU = action.bindings[f + 1].path;
                string keyD = action.bindings[f + 2].path;
                string keyL = action.bindings[f + 3].path;
                string keyR = action.bindings[f + 4].path;

                go.GetComponent<CompositeControlTile>().SetUpTile(inputName,keyU,keyD,keyL,keyR);
                go.SetActive(false);
                //increment f to skip Tiles
                f += 4;
                continue;
            }
            else
            {
                var go = CreateTile(tilePrefab,tileName,group);

                string key = action.bindings[f].path;
                
                go.GetComponent<ControlTile>().SetUpTile(inputName,key);
                go.SetActive(false);
            }
        }

    }

    private GameObject CreateTile(GameObject tiletype,string tileName, string tileGroup)
    {
        var go = Instantiate(tiletype,controleTileParent);

        go.name =  tileName;

        if(controlTileGroup.ContainsKey(tileGroup))
        {
            controlTileGroup[tileGroup].Add(go);
        }
        else
        {
            controlTileGroup.Add(tileGroup,new List<GameObject>());
            controlTileGroup[tileGroup].Add(go);
        }
        
        //go.SetActive(false);
        
        return go;
    }

}
