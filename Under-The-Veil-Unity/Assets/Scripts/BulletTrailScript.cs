using UnityEngine;

public class BulletTrailScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float progress;

    [SerializeField] private float speed = 40f;

    void Start()
    {
        startPosition = transform.position.WithAxis(SnapAxis.Z, value: -1);
    }

    void Update()
    {
        progress += Time.deltaTime * speed;
        transform.position = Vector3.Lerp(a: startPosition, b: targetPosition, progress);
    }

    public void SetTargetPosition(Vector3 target_Position)
    {
        targetPosition = target_Position.WithAxis(SnapAxis.Z, value: -1);
    }
}
