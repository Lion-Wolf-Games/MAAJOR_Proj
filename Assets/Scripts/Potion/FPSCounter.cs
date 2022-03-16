using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI counterText;
    private int lastFrameIndex;
    [SerializeField] private float[] frameDeltaTimeArray;

    // Start is called before the first frame update
    void Awake()
    {
        counterText = GetComponentInChildren<TextMeshProUGUI>();

        frameDeltaTimeArray = new float[60];
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        counterText.text = Mathf.RoundToInt(CalculateFPS()) + "FPS";
    }

    private float CalculateFPS()
    {
        float total = 0f;

        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}
