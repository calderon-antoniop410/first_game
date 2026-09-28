using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float delay = 0.2f;

    private AudioSource audioSource;
    private bool isPaused;

    private void Awake()
    {
        Debug.Log("PauseMenu Awake called");
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            Debug.Log("Pause key pressed");
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        PlayClick();
    }

    public void GoToMainMenu()
    {
        StartCoroutine(ClickThen(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }));
    }

    public void QuitGame()
    {
        StartCoroutine(ClickThen(Application.Quit));
    }

    private void PlayClick()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private IEnumerator ClickThen(Action action)
    {
        PlayClick();
        yield return new WaitForSecondsRealtime(delay);
        action();
    }
}