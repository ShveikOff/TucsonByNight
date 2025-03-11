using UnityEngine;

public class CameraFollowWithDeadZone : MonoBehaviour
{
    [Header("Player & Offset")]
    public Transform player;          // Ссылка на игрока
    public Vector3 offset;            // Смещение камеры относительно игрока

    [Header("Dead Zone Settings")]
    // Размер «мертвой зоны» (в координатах экрана или Viewport)
    public float deadZoneWidth = 0.2f;  // ширина области в долях экрана (viewport)
    public float deadZoneHeight = 0.2f; // высота области

    [Header("Camera Move Speed")]
    public float smoothSpeed = 2f;    // Насколько плавно камера смещается

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // 1) Находим позицию игрока в координатах Viewport (0..1 по x и y)
        Vector3 viewPos = cam.WorldToViewportPoint(player.position);

        // Рассчитаем границы "мертвых зон" по X и Y
        // Предположим, deadZoneWidth/Height - доля экрана, 
        // значит половина этой зоны: deadZoneWidth/2, deadZoneHeight/2
        float leftBound   = 0.5f - (deadZoneWidth / 2f);
        float rightBound  = 0.5f + (deadZoneWidth / 2f);
        float bottomBound = 0.5f - (deadZoneHeight / 2f);
        float topBound    = 0.5f + (deadZoneHeight / 2f);

        // 2) Проверяем, выходит ли персонаж за пределы "мертвых зон"
        // Если вышел - нужно сдвигать камеру
        float deltaX = 0f;
        float deltaY = 0f;

        // По X:
        if (viewPos.x < leftBound)
        {
            deltaX = viewPos.x - leftBound;
        }
        else if (viewPos.x > rightBound)
        {
            deltaX = viewPos.x - rightBound;
        }

        // По Y:
        if (viewPos.y < bottomBound)
        {
            deltaY = viewPos.y - bottomBound;
        }
        else if (viewPos.y > topBound)
        {
            deltaY = viewPos.y - topBound;
        }

        // Если deltaX или deltaY не нули, значит игрок вышел за зону
        if (deltaX != 0f || deltaY != 0f)
        {
            // 3) Корректируем позицию камеры
            // У нас есть смещение в координатах viewport (deltaX, deltaY).
            // Чтобы узнать, насколько сдвинуть камеру в мировых координатах,
            // сделаем обратное преобразование через WorldToViewportPoint / ViewportToWorldPoint.

            // Текущая позиция камеры в viewport = cam.WorldToViewportPoint(transform.position)
            Vector3 camPosViewport = cam.WorldToViewportPoint(transform.position);

            // Прибавляем deltaX, deltaY к позиции камеры в viewport
            camPosViewport.x += deltaX;
            camPosViewport.y += deltaY;

            // Возвращаемся в мир (World)
            Vector3 newCamPosWorld = cam.ViewportToWorldPoint(camPosViewport);

            // Но также учтём offset (если нужно)
            // Иногда offset учитывается проще – (player.position + offset),
            // но если хотим смещать только при выходе за dead zone, то используем расчёт через deltaX/deltaY.

            // Для плавности можно использовать Lerp/Slerp:
            Vector3 smoothPos = Vector3.Lerp(transform.position, newCamPosWorld, Time.deltaTime * smoothSpeed);

            transform.position = smoothPos;
        }

        // При желании можно добавить дополнительное смещение offset, 
        // если нужно фиксированное возвышение камеры над игроком:
        // transform.position = new Vector3(transform.position.x, offset.y, transform.position.z);
    }

    void OnDrawGizmos()
    {
        // Визуализация "мертвых зон" на экране в режиме Play (необязательно, но для дебага)
        if (cam == null) return;

        // Рисуем прямоугольник в координатах viewport
        float left   = 0.5f - deadZoneWidth / 2f;
        float right  = 0.5f + deadZoneWidth / 2f;
        float bottom = 0.5f - deadZoneHeight / 2f;
        float top    = 0.5f + deadZoneHeight / 2f;

        // Конвертируем в мировые координаты 4 точки
        Vector3 bottomLeft  = cam.ViewportToWorldPoint(new Vector3(left,  bottom, cam.nearClipPlane));
        Vector3 bottomRight = cam.ViewportToWorldPoint(new Vector3(right, bottom, cam.nearClipPlane));
        Vector3 topLeft     = cam.ViewportToWorldPoint(new Vector3(left,  top,    cam.nearClipPlane));
        Vector3 topRight    = cam.ViewportToWorldPoint(new Vector3(right, top,    cam.nearClipPlane));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
