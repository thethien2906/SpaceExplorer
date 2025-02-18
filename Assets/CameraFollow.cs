using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển

    // Giới hạn biên theo tọa độ bạn cung cấp
    private float minX = -7.14f, maxX = 8.34f;
    private float minY = -5.05f, maxY = 4.88f;

    void Update()
    {
        MovePlayer();
        ClampPosition();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY, 0);
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        // Giữ Player trong giới hạn biên
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}