using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnPause += PauseGame;
        pauseCanvas.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PauseGame()
    {
        //ShowPanel
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //PlayOpen Animation
    }

    public void ResumeGame()
    {
        //Play Closing animation
        //Hide Panel
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.ChangeGameState(GameState.Playing);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        GameManager.StaticLoadLevel(0);
    }


}
