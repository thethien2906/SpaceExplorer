using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public bool isMoving;
    private float turnInput;

    [Header("Movement")]
    public GameObject missile;
    public Transform missleSpawnPosition;
    public float destroyTime = 5f;


    [Header("Fire Rate")]
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private float missileSpeed = 25f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        PlayerAnimation();
        PlayerMovement();
        if (Time.time >= nextFireTime)
        {
            FireMissile();
            nextFireTime = Time.time + GameManager.instance.fireRate;
        }
    }

    private void PlayerAnimation()
    {
        anim.SetBool("isMoving", rb.linearVelocity.x != 0);
        turnInput = Input.GetAxis("Horizontal");
        anim.SetFloat("Turn", turnInput);
    }
    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical).normalized * GameManager.instance.playerSpeed;
        rb.linearVelocity = movement;

    }
    private void FireMissile()
    {
        Vector3 direction = Vector3.up;
        SpawnMissile(direction);
        AudioManager.instance.PlaySFX(0);
    }
    private void SpawnMissile(Vector3 direction)
    {
        GameObject missileInstance = Instantiate(missile, missleSpawnPosition.position, Quaternion.identity);
        missileInstance.transform.rotation = Quaternion.Euler(0, 0, 0); // Không xoay theo chuột nữa
        Rigidbody2D rb = missileInstance.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * missileSpeed; // Bắn thẳng lên trên
        }

        GameManager.instance?.RegisterMissile(missileInstance);
        Destroy(missileInstance, destroyTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.OnPlayerHit();
            GameObject explosionEffect = Instantiate(GameManager.instance.explosion, transform.position, transform.rotation);
            Destroy(explosionEffect, 2f);
            Destroy(collision.gameObject);
            ScoreScript.scoreValue += ScoreScript.enemyPoint;
            if (GameManager.instance.GetCurrentLives() <= 0)
            {
                Destroy(this.gameObject);
            }
            AudioManager.instance.PlaySFX(1);
        }
    }
}
