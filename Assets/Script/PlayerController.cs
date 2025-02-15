using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public GameObject missile;
    public Transform missleSpawnPosition;
    public float destroyTime = 5f;
    public float fireRate = 0.5f;
 
    private void Start()
    {
        InvokeRepeating(nameof(PlayerShoot), 0f, fireRate); // Continuously shoot
    }
    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerShoot()
    {
 
        GameObject gm = Instantiate(missile, missleSpawnPosition.position, Quaternion.identity);
        gm.transform.SetParent(null);
        Destroy(gm, destroyTime);
        
    }

    private void PlayerMovement()
    {
        float xPos = Input.GetAxis("Horizontal");
        float yPos = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(xPos, yPos, 0) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject gm = Instantiate(GameManager.instance.explosion, transform.position, transform.rotation);
            Destroy(gm, 2f);
            Destroy(this.gameObject);
        }
    }
}
