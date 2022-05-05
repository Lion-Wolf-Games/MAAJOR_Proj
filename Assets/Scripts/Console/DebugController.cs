using System.Collections;
using System.Collections.Generic;
using UnityEngine ;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;

    string input;

    public static DebugCommand KILL_ALL;

    public List<object> commandList;

    
    public void ToggleConsole()
    {
        showConsole = !showConsole;
    }

    public void OnReturn()
    {
        if(showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void Awake() {
        KILL_ALL = new DebugCommand("kill_all","Kill all Trap", "kill_all", () => 
        {
            Enemies[] enemies = FindObjectsOfType<Enemies>(false);

            foreach (var en in enemies)
            {
                en.Kill();
            }
        });

        commandList = new List<object>
        {
            KILL_ALL
        };
    }

    private void OnGUI() {

        if (!showConsole) { return; }

        float y = 0f;
        GUI.Box(new Rect(0,y,Screen.width,30),"");  
        GUI.backgroundColor = new Color(0,0,0,0);
        input = GUI.TextField(new Rect(10f,y + 5f, Screen.width - 20f,20f), input);
    }


    private void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    //cast to this type ans inke the command
                    (commandList[i] as DebugCommand).Invoke();
                }
            }   
                
        }
    }
}
