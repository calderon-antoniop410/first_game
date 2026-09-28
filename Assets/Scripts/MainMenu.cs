using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Gameplay_1";
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float delay = 0.1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        StartCoroutine(ClickThen(() => SceneManager.LoadScene(gameSceneName)));
    }

    public void QuitGame()
    {
        StartCoroutine(ClickThen(Application.Quit));
    }

    private IEnumerator ClickThen(Action action)
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSecondsRealtime(delay);
        action();
    }
}