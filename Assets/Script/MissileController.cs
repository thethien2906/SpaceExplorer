using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 25f;

    void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gm = Instantiate(GameManager.instance.explosion, transform.position,transform.rotation);
        Destroy(gm,2f);
        Destroy(gameObject); // Destroy the missile
        Destroy(collision.gameObject); // Destroy the enemy
    }
}
 