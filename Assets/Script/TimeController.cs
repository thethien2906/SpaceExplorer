using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static float timeValue = 0f;
    private TMP_Text time;

    void Start()
    {
        time = GetComponent<TMP_Text>();
        if (time == null)
        {
            Debug.LogError("TextMeshPro component is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (time != null)
        {
            int minutes = Mathf.FloorToInt(timeValue / 60f);
            int seconds = Mathf.FloorToInt(timeValue % 60f);

            time.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }
}
