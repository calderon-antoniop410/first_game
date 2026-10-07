using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Gameplay_1";
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float delay = 0.1f;

    private AudioSource audioSource;
    private GameObject controlsPanel;
    private TMP_FontAsset menuFont;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("MainMenu requires a Canvas to show the controls panel.", this);
            return;
        }

        TMP_Text existingText = canvas.GetComponentInChildren<TMP_Text>(true);
        if (existingText == null || existingText.font == null)
        {
            Debug.LogError("MainMenu requires a TextMeshPro font on the menu Canvas.", this);
            return;
        }

        menuFont = existingText.font;
        CreateControlsPanel(canvas.transform);
    }

    public void PlayGame()
    {
        StartCoroutine(ClickThen(() => SceneManager.LoadScene(gameSceneName)));
    }

    public void QuitGame()
    {
        StartCoroutine(ClickThen(Application.Quit));
    }

    public void ShowControls()
    {
        if (controlsPanel == null)
        {
            Debug.LogError("The controls panel could not be created.", this);
            return;
        }

        PlayClick();
        controlsPanel.SetActive(true);
        controlsPanel.transform.SetAsLastSibling();
    }

    private void HideControls()
    {
        PlayClick();
        controlsPanel.SetActive(false);
    }

    // Used AI here just to create a controls setting page
    private void CreateControlsPanel(Transform canvasTransform)
    {
        controlsPanel = CreateUIObject("Controls Panel", canvasTransform).gameObject;
        RectTransform overlayRect = controlsPanel.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;

        Image overlayImage = controlsPanel.AddComponent<Image>();
        overlayImage.color = new Color(0f, 0f, 0f, 0.72f);

        RectTransform cardRect = CreateUIObject("Controls Card", controlsPanel.transform);
        cardRect.sizeDelta = new Vector2(700f, 520f);
        Image cardImage = cardRect.gameObject.AddComponent<Image>();
        cardImage.color = new Color(0.12f, 0.14f, 0.18f, 1f);

        CreateText(cardRect, "Title", "CONTROLS", 44, TextAlignmentOptions.Center,
            new Vector2(600f, 70f), new Vector2(0f, 195f));
        CreateText(cardRect, "Control List",
            "MOVE\nW - Up\nA - Left\nS - Down\nD - Right\n\nATTACK\nLeft Click or Spacebar",
            32, TextAlignmentOptions.Center, new Vector2(600f, 330f), new Vector2(0f, -25f));

        RectTransform closeRect = CreateUIObject("Close Button", cardRect);
        closeRect.sizeDelta = new Vector2(220f, 64f);
        closeRect.anchoredPosition = new Vector2(0f, -215f);
        Image closeImage = closeRect.gameObject.AddComponent<Image>();
        closeImage.color = new Color(0.32f, 0.38f, 0.48f, 1f);

        Button closeButton = closeRect.gameObject.AddComponent<Button>();
        closeButton.targetGraphic = closeImage;
        closeButton.onClick.AddListener(HideControls);

        CreateText(closeRect, "Close Label", "CLOSE", 28, TextAlignmentOptions.Center,
            new Vector2(220f, 64f), Vector2.zero);

        controlsPanel.SetActive(false);
    }

    private RectTransform CreateUIObject(string objectName, Transform parent)
    {
        GameObject uiObject = new GameObject(objectName, typeof(RectTransform));
        RectTransform rectTransform = uiObject.GetComponent<RectTransform>();
        rectTransform.SetParent(parent, false);
        return rectTransform;
    }

    private void CreateText(Transform parent, string objectName, string text, float fontSize,
        TextAlignmentOptions alignment, Vector2 size, Vector2 position)
    {
        RectTransform textRect = CreateUIObject(objectName, parent);
        textRect.sizeDelta = size;
        textRect.anchoredPosition = position;

        TextMeshProUGUI label = textRect.gameObject.AddComponent<TextMeshProUGUI>();
        label.font = menuFont;
        label.fontSize = fontSize;
        label.alignment = alignment;
        label.color = Color.white;
        label.text = text;
        label.raycastTarget = false;
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