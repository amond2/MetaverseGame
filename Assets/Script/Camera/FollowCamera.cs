using UnityEngine;
using UnityEngine.Tilemaps;

public class FollowCamera : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target;

    [Header("Camera Offset")]
    // Inspector에서 직접 오프셋 값을 설정할 수 있습니다.
    // 예를 들어, 플레이어보다 위쪽에 위치하게 하려면 (0, 5, 0)과 같이 y값을 양수로 설정합니다.
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.15f, -10f);

    [Header("Tilemap for Level Bounds")]
    public Tilemap levelTilemap; // 타일맵을 Inspector에서 할당

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    // 타일맵 Bounds를 기반으로 계산된 맵 경계값
    private float mapMinX;
    private float mapMaxX;
    private float mapMinY;
    private float mapMaxY;

    void Start()
    {
        if (target == null)
            return;

        cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth  = halfHeight * cam.aspect;

        // 타일맵이 할당되어 있다면 타일맵의 Bounds를 가져와서 맵 경계를 설정합니다.
        if (levelTilemap != null)
        {
            Bounds levelBounds = levelTilemap.localBounds;
            mapMinX = levelBounds.min.x;
            mapMaxX = levelBounds.max.x;
            mapMinY = levelBounds.min.y;
            mapMaxY = levelBounds.max.y;
        }
        else
        {
            Debug.LogWarning("카메라 영역 타일 맵이 할당되지 않음");
        }
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // 타겟(플레이어)의 위치에 오프셋을 더하여 카메라의 원하는 위치 계산
        Vector3 pos = target.position + offset;

        // 카메라가 이동할 수 있는 최소/최대 값 계산 (카메라 중심 기준)
        float minXClamp = mapMinX + halfWidth;
        float maxXClamp = mapMaxX - halfWidth;
        float minYClamp = mapMinY + halfHeight;
        float maxYClamp = mapMaxY - halfHeight;

        pos.x = Mathf.Clamp(pos.x, minXClamp, maxXClamp);
        pos.y = Mathf.Clamp(pos.y, minYClamp, maxYClamp);

        // z 값은 고정 (예: 2D의 경우 보통 -10)
        transform.position = pos;
    }
}