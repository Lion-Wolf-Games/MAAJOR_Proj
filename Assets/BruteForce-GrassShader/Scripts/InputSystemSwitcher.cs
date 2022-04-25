using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

public class InputSystemSwitcher : MonoBehaviour
{
    public GameObject eventS;
    // Start is called before the first frame update
    void Awake()
    {

#if ENABLE_INPUT_SYSTEM
        if (eventS.GetComponent<InputSystemUIInputModule>() == null)
        {
            if(eventS.GetComponent<StandaloneInputModule>() != null)
            {
                Destroy(eventS.GetComponent<StandaloneInputModule>());
            }
            InputSystemUIInputModule inputSystem = eventS.AddComponent<InputSystemUIInputModule>();
            inputSystem.enabled = false;
            inputSystem.enabled = true;
            inputSystem.UpdateModule();
        }
#endif
    }
}
