﻿using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float minSpeed = 2f; // Tốc độ nhỏ nhất
    public float maxSpeed = 5f; // Tốc độ lớn nhất
    public float rotationSpeed = 30f; // Tốc độ xoay
    public float rotationSmoothness = 5f; // Để làm mượt quá trình xoay

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector3 bottomEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0));

        float targetX = Random.Range(-10f, 10f);
        float targetY = bottomEdge.y - 5f;

        // Điểm mục tiêu
        targetPosition = new Vector2(targetX, targetY);

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        rb.linearVelocity = direction * randomSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Update()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
