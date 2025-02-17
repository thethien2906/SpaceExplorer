using System;
using System.Reflection;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public GameObject missile;
    public Transform missleSpawnPosition;
    public float destroyTime = 5f;

    
    [Header("Fire Rate")]
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private float missileSpeed = 25f;

    private void Update()
    {
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
        if (collision.gameObject.tag == "Enemy")
        {
         
            ScoreScript.scoreValue += ScoreScript.enemyPoint;
            ScoreScript.scoreValue += ScoreScript.enemyPoint;
            GameObject gm = Instantiate(GameManager.instance.explosion, transform.position, transform.rotation);
            Destroy(gm, 2f);
            Destroy(this.gameObject);
        }
    }
}
