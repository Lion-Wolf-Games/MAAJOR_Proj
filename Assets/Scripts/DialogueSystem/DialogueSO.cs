using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    public Dialogue[] _dialogue;
}

[System.Serializable]
public class Dialogue
{
    public string _speakerName;
    [TextArea(4,4)]
    public string _text;
    public bool _isQuestion;
    public string _optionA, _optionB;

    public Dialogue(string speaker,string text)
    {
        _speakerName = speaker;
        _text = text;
    }
}
