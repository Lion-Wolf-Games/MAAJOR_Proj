using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI _text;
    public GameObject _textPannel;

    public TextMeshProUGUI _speakerNametext;
    public GameObject _speakerNamePanel;

    public Image _speakerPortrait;
    public GameObject _speakerPortraitPanel;

    public GameObject _optionPanel;
    public TextMeshProUGUI _optionAText;
    public TextMeshProUGUI _optionBText;

    public float _defaultTimeToChar = 0.1f; //Time between characters
    private float _timeToCharacter = 0.1f;
    private float _readtime; //time text remaing on screen if set to auto

    public string _fullText;
    private string _currentText;

    public List<Dialogue> _dialogueQueue;

    public AK.Wwise.Event OnWrite;

    public bool _isWriting = false;
    private bool stopOnDialogue = true;

    public delegate void OnDialogueEvent();

    public OnDialogueEvent _onDialogueStart;
    public OnDialogueEvent _onDialogueEnd;
    public OnDialogueEvent _onDialogueOptionA;
    public OnDialogueEvent _onDialogueOptionB;



    #region Singleton
    public static DialogueSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    //Use Instance.DialogueSystem

    private void Start()
    {
        _onDialogueStart += () => {GameManager.Instance.ChangeGameState(GameState.InDialogue);};
        _onDialogueEnd += () => {GameManager.Instance.ChangeGameState(GameState.Playing);};

        if (_dialogueQueue.Count == 0)
        {
            _textPannel.SetActive(false);
        }
        else
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (_dialogueQueue.Count != 0)
        {
            _textPannel.SetActive(true);

            if(stopOnDialogue)
            {
                _onDialogueStart?.Invoke();
            }

            Tell(_dialogueQueue[0], _defaultTimeToChar);

        }
    }

    public void AddDialogueToQueue(DialogueSO dialogues)
    {
        stopOnDialogue = dialogues.stopInput;
        _readtime = dialogues.readTime;

        for (int i = 0; i < dialogues._dialogue.Length; i++)
        {
            AddDialogueToQueue(dialogues._dialogue[i]);
        }
    }

    public void AddDialogueToQueue(Dialogue dialogue) //Add text to display, use multiple time at once to queue
    {
        _dialogueQueue.Add(dialogue);

        if(_dialogueQueue[0] == dialogue)
        {
            StartDialogue();
        }
    }

    public void ClearQueue(bool hidepanel)
    {
        for (int i = _dialogueQueue.Count - 1; i >= 1; i--)
        {
            _dialogueQueue.Remove(_dialogueQueue[i]);
        }

        if (hidepanel)
        {
            _textPannel.SetActive(false);
        }
    }

    public void SetTimeToCharacter(float time = 0.1f) //Set time between each character. Lower is faster
    {
        _defaultTimeToChar = time;
        //_audio.pitch = time * 10;
    }
    public void SkipWriting(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            SkipWriting();
        }
    }

    [ContextMenu("Skip writing")]
    public void SkipWriting() //Set time between char to 0 or skip to next dialogue in queue if not writing
    {
        if (_textPannel.activeInHierarchy)
        {
            if (_isWriting)
            {
                _timeToCharacter = 0;
            }
            else if (_currentText != _dialogueQueue[0]._text)
            {
                Tell(_dialogueQueue[0], _defaultTimeToChar);
            }
            else if (_dialogueQueue[0]._isQuestion)
            {
                return;
            }
            else if (_dialogueQueue.Count != 1 && _currentText == _dialogueQueue[0]._text)
            {
                NextDialogueInQueue();
            }
            else
            {
                NextDialogueInQueue();
                _onDialogueEnd?.Invoke();

                //GameManager.Instance._eventInProgress = false;

                _textPannel.SetActive(false);

                Time.timeScale = 1;
            }

        }    
    }

    private void Tell(Dialogue dialogue, float timeToChar) //text to display and time between characters
    {
        _isWriting = true;

        _fullText = dialogue._text;

        if(dialogue._textSpeed == -1)
        {
            _timeToCharacter = _defaultTimeToChar;
        }
        else
        {
            _timeToCharacter = dialogue._textSpeed;
        }

        //_timeToCharacter = timeToChar;

        //SetName
        if (_speakerNamePanel != null)
        {
            if (dialogue._speakerName == string.Empty)
            {
                _speakerNamePanel.SetActive(false);
            }
            else
            {
                _speakerNamePanel.SetActive(true);
                _speakerNametext.text = dialogue._speakerName;
            } 
        }

        //SetPortrait
        if(_speakerPortrait != null)
        {
            if(dialogue._speakerPortrait == null)
            {
                _speakerPortraitPanel.SetActive(false);
            }
            else
            {
                _speakerPortraitPanel.SetActive(true);
                _speakerPortrait.sprite = dialogue._speakerPortrait;
            }
        }

        if (_optionPanel != null)
        {
            _optionPanel.SetActive(false);
        }

        StartCoroutine(DialogueWriter());
    }

    [ContextMenu("Next Dialogue")]
    private void NextDialogueInQueue()
    {
        _dialogueQueue.Remove(_dialogueQueue[0]);
        if (_dialogueQueue.Count != 0)
        {
            Tell(_dialogueQueue[0], _defaultTimeToChar);
        }
    }

    IEnumerator DialogueWriter()
    {
        _currentText = "";

        for (int i = 0; i < _fullText.Length; i++)
        {
            _currentText += _fullText.Substring(i, 1);

            _text.text = _currentText;

            if (_fullText.Substring(i, 1) == " ") //delays for space reduced
            {
                yield return new WaitForSecondsRealtime(_timeToCharacter / 1.5f);
            }
            else if (_fullText.Substring(i, 1) == ".") //longer delays for dots
            {
                yield return new WaitForSecondsRealtime(_timeToCharacter * 6);
            }
            else if (_fullText.Substring(i,1) == "<") //Display tags instantaly
            {
                bool hasReachEndTag = false;
                while (!hasReachEndTag)
                {
                    i++;

                    if (i == _fullText.Length)
                    {
                        break;
                    }

                    _currentText += _fullText.Substring(i, 1);

                    if (_fullText.Substring(i, 1) == ">")
                        hasReachEndTag = true;

                }
            }
            else
            {
                OnWrite.Post(gameObject);
                yield return new WaitForSecondsRealtime(_timeToCharacter);
            }
        }

        if (_currentText == null)
        {
            SkipWriting();
        }

        _isWriting = false;

        //Show options 
        if (_dialogueQueue[0]._isQuestion)
        {
            _optionAText.text = _dialogueQueue[0]._optionA;
            _optionBText.text = _dialogueQueue[0]._optionB;
            _optionPanel.SetActive(true);
        }

        if(!stopOnDialogue)
        {
            yield return new WaitForSecondsRealtime(_readtime);
            SkipWriting();
        }

    }

    public void RespondA()
    {
        _onDialogueOptionA?.Invoke();

        //Clear the delegate to prevent errors
        _onDialogueOptionB = null;
        _onDialogueOptionA = null;

        //Hide the panel and go to next dialogue
         
        NextDialogueInQueue();
    }

    public void RespondB()
    {
        _onDialogueOptionB?.Invoke();

        //Clear the delegate to prevent errors
        _onDialogueOptionB = null;
        _onDialogueOptionA = null;

        //Hide the panel and go to next dialogue
        _optionPanel.SetActive(false);
        NextDialogueInQueue();
    }

    public void DebugAddText()
    {
        int random = UnityEngine.Random.Range(0, 11);
        AddDialogueToQueue(new Dialogue("Beep Boop","Random int: " + random));
    }
    public void DebugTextSpeed()
    {
        SetTimeToCharacter(UnityEngine.Random.Range(0f, 0.5f));
    }

    public void DebugAddDialogue(DialogueSO dialogue)
    {
        AddDialogueToQueue(dialogue);
    }

}
