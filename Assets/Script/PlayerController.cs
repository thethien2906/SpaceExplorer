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
    public float speed = 10f;
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
        InvokeRepeating(nameof(FireMissile), 1.5f, fireRate); // ban lien tuc
    }
    private void Update()
    {
        PlayerAnimation();
        PlayerMovement();
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            FireMissile();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void PlayerShoot()
    {

        GameObject gm = Instantiate(missile, missleSpawnPosition.position, Quaternion.identity);
        gm.transform.SetParent(null);
        Destroy(gm, destroyTime);

    }

    private void PlayerAnimation()
    {
        anim.SetBool("isMoving",rb.linearVelocity.x != 0);
        turnInput = Input.GetAxis("Horizontal"); // Get A/D or Left/Right Arrow input
        anim.SetFloat("Turn", turnInput);
    }
    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        transform.Translate(movement);

        RotateToMouse();
    }
    private void RotateToMouse()
    {
        if (Camera.main == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void FireMissile()
    {
        if (Camera.main == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - missleSpawnPosition.position).normalized;
        SpawnMissile(direction);
        //SpawnFlash(direction);
    }
    private void SpawnMissile(Vector3 direction)
    {
        GameObject missileInstance = Instantiate(missile, missleSpawnPosition.position, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        missileInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
        Rigidbody2D rb = missileInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * missileSpeed;
        }

        GameManager.instance.RegisterMissile(missileInstance);
        Destroy(missileInstance, destroyTime);
    }

    //private void SpawnFlash(Vector3 direction)
    //{
    //    GameObject muzzleFlashInstance = Instantiate(GameManager.instance.MuzzleEffect, FlashSpawnPoint.position, Quaternion.identity);
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    //    muzzleFlashInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

    //    Destroy(muzzleFlashInstance, despawnTime);
    //}

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
        }
    }
}
