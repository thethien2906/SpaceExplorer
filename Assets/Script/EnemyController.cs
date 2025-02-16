using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float minSpeed = 2f; // Tốc độ nhỏ nhất
    public float maxSpeed = 5f; // Tốc độ lớn nhất
    public float rotationSpeed = 30f; // Tốc độ xoay

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        rb.linearVelocity = randomDirection * randomSpeed;

        rb.angularVelocity = Random.Range(-rotationSpeed, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Vector2 normal = collision.transform.up;
            rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, normal);
        }
    }
}
