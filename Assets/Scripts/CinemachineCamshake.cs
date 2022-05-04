using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamshake : MonoBehaviour
{
    public static CinemachineCamshake Instance;
    private CinemachineVirtualCamera virtualcam;

    private float shaketime;
    private bool isShaking;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start() {
        virtualcam = GetComponent<CinemachineVirtualCamera>();

        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

    public void ShakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        shaketime = Time.time + time;

        isShaking = true;
    }

    private void Update() {
        if(Time.time > shaketime && isShaking)
        {
             CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            isShaking = false;
        }
    }
}
