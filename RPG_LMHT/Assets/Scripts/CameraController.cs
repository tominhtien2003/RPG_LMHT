using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float zoomSpeed = 4f;
    [SerializeField] private float pitch = 2f;
    [SerializeField] private float yaw = 100f;

    private float currentZoom = 10f;
    private float currentYaw = 0f;
    private void Start()
    {
    }
    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yaw * Time.deltaTime;
    }
    private void LateUpdate()
    {
        transform.position = playerTransform.position - offset * currentZoom;
        transform.LookAt(playerTransform.position + Vector3.up * pitch);

        transform.RotateAround(playerTransform.position, Vector3.up, currentYaw);
    }
}
