using UnityEngine;
using TMPro;

public class Pause_Manager : MonoBehaviour
{
    public GameObject pauseMenu;
    public TMP_Text marksText;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        UpdateMarksUI();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void UpdateMarksUI()
    {
        if (marksText != null && MarkSaver.Instance != null)
        {
            marksText.text = MarkSaver.Instance.GetAllGradesText();
        }
        else if (marksText != null)
        {
            marksText.text = "No marks available.";
        }
    }
}