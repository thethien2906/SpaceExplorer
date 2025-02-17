using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    private TextMeshProUGUI livesText;

    void Start()
    {
        livesText = GetComponent<TextMeshProUGUI>();
        UpdateLivesDisplay();
    }

    void Update()
    {
        UpdateLivesDisplay();
    }

    void UpdateLivesDisplay()
    {
        if (GameManager.instance != null)
        {
            livesText.text = "Lives: " + GameManager.instance.GetCurrentLives();
        }
    }
}