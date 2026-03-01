using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform cameraTransform; // Main Camera
    public Transform target;          // Player
    public Transform targetPivot;     // Player/CameraPivot (LockOnCameraAssist bunu hareket ettirecek)

    public Vector3 offset = new Vector3(0, 2, -4);

    public float followSpeed = 10f;
    public float rotateSpeed = 120f;

    public float minPitch = -30f;
    public float maxPitch = 60f;

    float yaw;
    float pitch;
    Vector2 lookInput;

    void LateUpdate()
    {
        if (!target || !cameraTransform) return;

        yaw += lookInput.x * rotateSpeed * Time.deltaTime;
        pitch -= lookInput.y * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);

        // hedef noktası: pivot varsa onu kullan
        Vector3 targetPoint = targetPivot
            ? targetPivot.position
            : (target.position + Vector3.up * 1.5f);

        Vector3 desiredPos = targetPoint + rot * offset;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );

        cameraTransform.LookAt(targetPoint);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
