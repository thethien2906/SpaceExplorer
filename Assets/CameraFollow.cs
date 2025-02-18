using UnityEngine;

public class PlayerClampToCamera : MonoBehaviour
{
    public float speed = 5f; // Tốc độ di chuyển
    private float minX, maxX, minY, maxY;
    private float playerWidth, playerHeight;

    void Start()
    {
        AdjustBounds();
    }

    void Update()
    {
        MovePlayer();
        ClampPosition();
    }

    void AdjustBounds()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Lấy kích thước của Player
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            playerWidth = sr.bounds.extents.x; // Nửa chiều rộng
            playerHeight = sr.bounds.extents.y; // Nửa chiều cao
        }
        else
        {
            playerWidth = 0.5f;  // Nếu không có SpriteRenderer, đặt mặc định
            playerHeight = 0.5f;
        }

        // Chuyển tọa độ Viewport (0,0) và (1,1) sang tọa độ thế giới
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        // Tính toán giới hạn với kích thước Player
        minX = bottomLeft.x + playerWidth;
        maxX = topRight.x - playerWidth;
        minY = bottomLeft.y + playerHeight;
        maxY = topRight.y - playerHeight;
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

        // Giữ Player trong giới hạn biên của Camera
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
