using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] string signText;
    [SerializeField] Sprite signImage;
    

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            SignUI.Instance?.SetSign(signText,signImage);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            SignUI.Instance?.HideSign();
        }
    }
}
