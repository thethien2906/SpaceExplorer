using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject enemyPrefab;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime;   
    [Header("Particle Effects")]
    public GameObject explosion;

    [Header("Panels")]
    public GameObject startMenu;
    public GameObject pauseMenu;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
        InvokeRepeating("InstaniateEnemy", 1f, 2f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(true);
        }
    }
    private void InstaniateEnemy()
    {
        Vector3 enemypos = new Vector3(Random.Range(minInstantiateValue, maxInstantiateValue), 6f);
        GameObject eneymy = Instantiate(enemyPrefab, enemypos, Quaternion.Euler(0f, 0f,180f));
        Destroy(eneymy, enemyDestroyTime);
    }
    public void StartGameButton()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else 
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }
   

}
