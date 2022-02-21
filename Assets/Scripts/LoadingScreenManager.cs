using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreenCanvas;
    [SerializeField] private Slider _loadingBar;

    private void Start()
    {
        _loadingScreenCanvas.SetActive(false);
    }

    public void ShowLoadingScreen(AsyncOperation levelLoading)
    {
        StartCoroutine(LoadingScreenCoroutine(levelLoading));
    }

    private IEnumerator LoadingScreenCoroutine(AsyncOperation sceneLoading)
    {
        _loadingScreenCanvas.SetActive(true);

        while (!sceneLoading.isDone)
        {
            Time.timeScale = 0f;
            _loadingBar.value = sceneLoading.progress;
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1f;
        _loadingScreenCanvas.SetActive(false);

    }
}
