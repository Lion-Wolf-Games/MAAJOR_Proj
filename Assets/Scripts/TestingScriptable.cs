using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Value", menuName = "DebugScriptable", order = 0)]



public class TestingScriptable : ScriptableObject
{
    [SerializeField] public int value;
}
