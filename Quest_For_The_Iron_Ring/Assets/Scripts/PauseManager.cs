using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Manager : MonoBehaviour
{
    public GameObject pauseMenu;
    public TMP_Text marksText;
    public Button winButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);

        if (winButton != null)
            winButton.gameObject.SetActive(false);
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
        UpdateWinButton();
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
            marksText.text = MarkSaver.Instance.GetAllGradesText();
    }

    private void UpdateWinButton()
    {
        if (winButton == null)
            return;

        bool canWin = MarkSaver.Instance != null && MarkSaver.Instance.HasPassedAllLevels();
        winButton.gameObject.SetActive(canWin);
        Debug.Log("Win Button Updated: " + MarkSaver.Instance.HasPassedAllLevels());
    }
}