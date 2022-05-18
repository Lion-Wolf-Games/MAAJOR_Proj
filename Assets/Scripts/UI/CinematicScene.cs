using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicScene : MonoBehaviour
{
    [SerializeField] VideoPlayer player;
    [SerializeField] bool videoStarted;

    [SerializeField] int sceneIndex;

    private void Start()
    {
        if (player.targetCamera == null)
        {
            player.targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (!videoStarted && player.isPlaying)
        {
            videoStarted = true;
        }
        else if (videoStarted && !player.isPlaying)
        {
            videoStarted = false;
            EndVideo();
        }
    }

    public void EndVideo()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
