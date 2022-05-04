using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ControlTile : MonoBehaviour
{
    [SerializeField] private Image keyImage;
    [SerializeField] private TextMeshProUGUI actionName;
    [SerializeField] private TextMeshProUGUI keyPath;
    [SerializeField] private ControlSkin skin;
    private string keyPathString;

    public void SetUpTile(string name,string key)
    {
        actionName.text = name;

        if(key.Contains("Keyboard"))
        {
            //Get Corresponding Key
            string inputkey = key.Split('/')[1];
            if (Keyboard.current.FindKeyOnCurrentKeyboardLayout(inputkey) != null)
            {
                string keyOnLayout = Keyboard.current.FindKeyOnCurrentKeyboardLayout(inputkey).ToString();
                string newKey = keyOnLayout.Split('/')[2];
            
                key = key.Remove(key.Length - inputkey.Length);
                key += newKey;
            }
            
            //Debug.Log(inputkey +"\t"+ newKey);
        }

        keyPath.text = key;
        keyImage.sprite = skin.GetKeyIcon(key);
    }
}
