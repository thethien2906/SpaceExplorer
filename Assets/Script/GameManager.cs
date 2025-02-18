using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] enemyRefab;
    public float ExistTime = 10f;
    private float originalSpawnRate = 0.95f;
    public float spawnRate;
    private static float minSpawnRate = 0f;  // Minimum spawn rate
    public float spawnRadius = 20f;
    public float minSpawnRadius = 10f;
    private int enemiesPerSpawn = 1;

    [Header("Game Settings")]
    public int originalLive = 3;
    private int currentLives;
    public float invincibilityDuration = 2f;
    private bool isPlayerInvincible = false;
    private bool isEnemyInvincible = false;
    public bool isGameOver = false;

    [Header("Particle Effects")]
    public GameObject explosion;

    [Header("Panels")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject gameOverPanel;

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;

    [Header("Player")]
    private GameObject player;
    public GameObject playerPrefab;
    public float playerSpeed = 5f;

    [Header("Player firerate")]
    public float originalFireRate = 1f;
    public float fireRate = 1f;

    public GameObject TrackingPosition;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activeMissiles = new List<GameObject>();
    private List<GameObject> activeOrds = new List<GameObject>();

    private float elapsedTime = 0f;  // Track the elapsed time

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            initialPlayerPosition = player.transform.position;
            initialPlayerRotation = player.transform.rotation;
        }
        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(true);
        }

        if (Time.timeScale != 0f)
        {
            TimeController.timeValue += Time.deltaTime;

            elapsedTime += Time.deltaTime;  // Increase elapsed time

            UpdateSpawnRate(); // Update spawn rate gradually
        }
    }

    void InitializeGame()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
        fireRate = originalFireRate;
        currentLives = originalLive;
        spawnRate = originalSpawnRate;
        CancelInvoke("IntantianteEnemy");
        InvokeRepeating("IntantianteEnemy", 0.1f, spawnRate);
        AudioManager.instance.PlayBGM(0);
    }

    void UpdateSpawnRate()
    {
        if (elapsedTime % 10f < Time.deltaTime) // Cứ 10 giây tăng số lượng spawn
        {
            enemiesPerSpawn = Mathf.Min(enemiesPerSpawn + 1, 5); // Tối đa 5 enemy mỗi lần
        }
        if (spawnRate > minSpawnRate + 0.025f)
        {
            if (elapsedTime % 3f < Time.deltaTime && spawnRate > minSpawnRate)
            {
                if (elapsedTime <= 24f)
                {
                    ScoreScript.scoreValue += 5;
                    spawnRate -= 0.05f;
                }
                else if (elapsedTime > 24f)
                {
                    ScoreScript.scoreValue += 7;
                    spawnRate -= 0.025f;
                }
            }
        }
        else
        {
            if (elapsedTime % 10f < Time.deltaTime)  // Check if 10 seconds have passed
            {
                ScoreScript.scoreValue += 30;
            }
        }
    }

    void IntantianteEnemy()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)  // Spawn nhiều enemy
        {
            int randomIndex = Random.Range(0, enemyRefab.Length);
            GameObject enemy = enemyRefab[randomIndex];
            Vector3 enemyPos = GetEdgeSpawnPosition();
            GameObject asteroid = Instantiate(enemy, enemyPos, Quaternion.Euler(0, 0, 180));
            activeEnemies.Add(asteroid);
            Destroy(asteroid, ExistTime);
        }
    }

    
    Vector3 GetEdgeSpawnPosition()
    {
        Vector3 topEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0));
        float minSpawnX = -10f;
        float maxSpawnX = 10f;
        float offsetX = Random.Range(minSpawnX, maxSpawnX);
        return new Vector3(topEdge.x + offsetX, topEdge.y, 0);
    }

    public void RegisterMissile(GameObject missile)
    {
        activeMissiles.Add(missile);
    }

    public void RegisterOrd(GameObject ord)
    {
        activeOrds.Add(ord);
    }

    void ResetGameState()
    {
        isGameOver = false;

        if (player == null)
        {
            player = Instantiate(playerPrefab, initialPlayerPosition, initialPlayerRotation);
        }
        else
        {
            player.transform.position = initialPlayerPosition;
            player.transform.rotation = initialPlayerRotation;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
                playerSprite.enabled = true;
        }

        currentLives = originalLive;
        isPlayerInvincible = false;
        ScoreScript.scoreValue = 0;

        // Reset spawn rate to original value when the game resets
        spawnRate = originalSpawnRate;
        CancelInvoke("IntantianteEnemy");
        InvokeRepeating("IntantianteEnemy", 0.1f, spawnRate);

        elapsedTime = 0f;  // Reset the elapsed time when restarting the game

        TimeController.timeValue = 0f;
    }

    public bool CheckIsReset()
    {
        return isEnemyInvincible;
    }

    public void StartGameButton()
    {
        ResetGameState();
        startMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.PlayBGM(1);
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            AudioManager.instance.AdjustBGMForPause(true);
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            AudioManager.instance.AdjustBGMForPause(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        ResetGameState();
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
        AudioManager.instance.PlayBGM(0);

    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void PlusCurrentLives()
    {
        currentLives += 1;
    }

    public void OnPlayerHit()
    {
        if (isPlayerInvincible) return;

        currentLives--;
        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(PlayerInvincibilityCoroutine());
        }
    }

    private System.Collections.IEnumerator PlayerInvincibilityCoroutine()
    {
        isPlayerInvincible = true;
        SpriteRenderer playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        float flashInterval = 0.2f;

        for (float i = 0; i < invincibilityDuration; i += flashInterval)
        {
            if (playerSprite != null)
                playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        if (playerSprite != null)
            playerSprite.enabled = true;

        isPlayerInvincible = false;
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        CancelInvoke("IntantianteEnemy");

        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();

        foreach (GameObject missile in activeMissiles)
        {
            if (missile != null)
            {
                Destroy(missile);
            }
        }
        activeMissiles.Clear();

        foreach (GameObject ord in activeOrds)
        {
            if (ord != null)
            {
                Destroy(ord);
            }
        }
        activeOrds.Clear();
        AudioManager.instance.PlayBGM(2);
        TimeController.timeValue = 0f;
        fireRate = originalFireRate;
    }
}
