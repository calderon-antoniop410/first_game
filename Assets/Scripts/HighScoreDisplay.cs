using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HighScoreDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text =
            "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }
}
