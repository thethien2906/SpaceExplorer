using TMPro; // Import TextMeshPro namespace
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    public static int enemyPoint = 10;
    public static int timePoint = 10;
    private TMP_Text score; // Sử dụng TMP_Text thay vì Text

    void Start()
    {
        score = GetComponent<TMP_Text>(); // Lấy component TextMeshPro
        if (score == null)
        {
            Debug.LogError("ScoreText is missing a TMP_Text component!");
        }
    }

    void Update()
    {

        if (score != null)
        {
            score.text = "Score: " + scoreValue;
        }
    }
}
