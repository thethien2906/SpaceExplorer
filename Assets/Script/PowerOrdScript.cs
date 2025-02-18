using UnityEngine;

public class PowerOrdScript : MonoBehaviour
{
    private float speed = 0.5f; // Gi?m t?c ?? ?? r?i ch?m h?n
    private float destroyTime = 10f;
    public float maxFireRate = 0.11f;
    public float fireRateUp = 0.03f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0; // Kh�ng ch?u ?nh h??ng c?a tr?ng l?c
        rb.linearVelocity = Vector2.down * speed; // ?i?u ch?nh t?c ?? r?i
        rb.bodyType = RigidbodyType2D.Kinematic; // Kh�ng b? ?nh h??ng b?i v?t l�
        GetComponent<Collider2D>().isTrigger = true; // Ch? k�ch ho?t s? ki?n va ch?m
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlusFireRate();
            Destroy(gameObject);
        }
    }

    void PlusFireRate()
    {
        if (GameManager.instance.fireRate >= maxFireRate)
            GameManager.instance.fireRate -= fireRateUp;
    }
}
