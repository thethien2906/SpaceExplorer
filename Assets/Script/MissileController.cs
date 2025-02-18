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

        if (collision.gameObject.tag == "Enemy")
        {  
            ScoreScript.scoreValue += ScoreScript.enemyPoint;
            GameObject gm = Instantiate(GameManager.instance.explosion, transform.position, transform.rotation);
            Destroy(gm, 2f);
            Destroy(gameObject); // Destroy the missile
            Destroy(collision.gameObject); // Destroy the enemy
            AudioManager.instance.PlaySFX(1);
        }
    }
}
