using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] enemyRefab;
    public float ExistTime = 20f;
    public float SpawnLevel;
    public float spawnRate;
    public float spawnRadius = 20f;
    public float minSpawnRadius = 10f;


    [Header("Game Settings")]
    public int maxLives = 3;
    private int currentLives;
    public float invincibilityDuration = 2f;
    private bool isPlayerInvincible = false;


    [Header("Particle Effects")]
    public GameObject explosion;

    [Header("Panels")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject gameOverPanel;

    // Luu vi tri ban dau
    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;


    private GameObject player;
    public GameObject playerPrefab;

    public GameObject TrackingPosition;

    // List to keep track of spawned enemies
    private List<GameObject> activeEnemies = new List<GameObject>();
    // List to keep track of missles
    private List<GameObject> activeMissiles = new List<GameObject>();
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Store the initial player position and rotation
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
            TimeController.timeValue += Time.deltaTime;  // Cập nhật thời gian khi game không pause
        }
    }

    void InitializeGame()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
        currentLives = maxLives;
        CancelInvoke("IntantianteEnemy");
        InvokeRepeating("IntantianteEnemy", 1f, 2f);
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
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        CancelInvoke("IntantianteEnemy");
        TimeController.timeValue = 0f;
    }
    void IntantianteEnemy()
    {
        int randomIndex = Random.Range(0, enemyRefab.Length);
        GameObject enemy = enemyRefab[randomIndex];

        // Lấy vị trí spawn từ rìa trên cùng của camera
        Vector3 enemyPos = GetEdgeSpawnPosition();

        // Spawn asteroid ở vị trí xác định
        GameObject asteroid = Instantiate(enemy, enemyPos, Quaternion.Euler(0, 0, 180));  // Xoay nếu cần
        activeEnemies.Add(asteroid);

        // Hủy asteroid sau một khoảng thời gian
        Destroy(asteroid, ExistTime);
    }

    // Hàm để lấy vị trí spawn ở rìa trên cùng của camera
    // Hàm lấy vị trí spawn với giới hạn trục X
    Vector3 GetEdgeSpawnPosition()
    {

        Vector3 topEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0));


        float minSpawnX = -10f;
        float maxSpawnX = 10f;
        float offsetX = Random.Range(minSpawnX, maxSpawnX);

        return new Vector3(topEdge.x + offsetX, topEdge.y, 0);
    }

    // track missiles
    public void RegisterMissile(GameObject missile)
    {
        activeMissiles.Add(missile);
    }
    void ResetGameState()
    {
        // Clear all active enemies and missiles
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

        // Check if player exists; if not, instantiate new one
        if (player == null)
        {
            player = Instantiate(playerPrefab, initialPlayerPosition, initialPlayerRotation);
        }
        else
        {
            // Reset position and rotation
            player.transform.position = initialPlayerPosition;
            player.transform.rotation = initialPlayerRotation;

            // Reset Rigidbody
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Reset player sprite visibility
            SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
                playerSprite.enabled = true;
        }

        // Reset lives
        currentLives = maxLives;
        isPlayerInvincible = false;

        // Reset score
        ScoreScript.scoreValue = 0;

        // Cancel and restart enemy spawning
        CancelInvoke("IntantianteEnemy");
        InvokeRepeating("IntantianteEnemy", 1f, 0.1f);

        TimeController.timeValue = 0f;
    }



    public void StartGameButton()
    {
        ResetGameState();
        startMenu.SetActive(false);
        gameOverPanel.SetActive(false);
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

    public void ReturnToMainMenu()
    {
        ResetGameState();
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}