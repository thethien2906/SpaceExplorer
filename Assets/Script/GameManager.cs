using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] enemyRefab;
    public float ExistTime = 20f;
    public float SpawnLevel;
    public float spawnRate;
    public float spawnRadius = 20f;
    public float minSpawnRadius = 10f;
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
        InvokeRepeating("IntantianteEnemy", 1f, 2f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(true);
        }
    }
    void IntantianteEnemy()
    {
        int randomIndex = Random.Range(0, enemyRefab.Length);
        GameObject enemy = enemyRefab[randomIndex];

        Vector3 enemyPos = GetValidSpawnPosition();
        GameObject asteroid = Instantiate(enemy, enemyPos, Quaternion.Euler(0, 0, 180));
        Destroy(asteroid, ExistTime);
    }

    Vector3 GetValidSpawnPosition()
    {


        Vector3 centerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPosition;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnRadius, spawnRadius);
        Vector2 randomOffset = randomDirection * randomDistance;
        spawnPosition = centerPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

        return spawnPosition;
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
