using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new control Skin",menuName ="ControlSkin")]
public class ControlSkin : ScriptableObject
{
    public Key[] keys;

    public Sprite GetKeyIcon(string KeyName)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if(keys[i].KeyName == KeyName)
            {
                return keys[i].KeyIcon;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Key
{
    public string KeyName;
    public Sprite KeyIcon;

}
