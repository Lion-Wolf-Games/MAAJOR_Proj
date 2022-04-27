using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DebugScript : MonoBehaviour
{
    [SerializeField] private TestingScriptable scriptable;
    public TextMeshProUGUI text;

    private void Start()
    {
        text.text = scriptable.value.ToString();

        
    }

    public void ChangeScriptableValue(int addedValue)
    {
        scriptable.value += addedValue;

        text.text = scriptable.value.ToString();
    }
}
