using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueSO : ScriptableObject
{
    public bool stopInput = true;
    public float readTime = 2f;
    public Dialogue[] _dialogue;
}

[System.Serializable]
public class Dialogue
{
    public string _speakerName;
    public Sprite _speakerPortrait;
    [TextArea(4,4)]
    public string _text;
    public bool _isQuestion;
    public string _optionA, _optionB;
    public float _textSpeed = -1f;

    public Dialogue(string speaker,string text)
    {
        _speakerName = speaker;
        _text = text;
    }
}
