using UnityEngine;

public class LockOnCameraAssist : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public LockOn lockOn;

    [Header("Offset")]
    public Vector3 normalLocalOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Lock-on framing")]
    [Range(0f, 1f)] public float targetWeight = 0.35f;
    public float maxShift = 2.5f;
    public float smooth = 12f;

    Vector3 currentWorldPos;

    void Start()
    {
        currentWorldPos = player.position + normalLocalOffset;
        transform.position = currentWorldPos;

        // ⭐ başlangıçta da sıfırla
        transform.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        if (!player || !lockOn) return;

        Vector3 desired = player.position + normalLocalOffset;

        if (lockOn.IsLockedOn && lockOn.currentTarget)
        {
            Vector3 mid = Vector3.Lerp(player.position, lockOn.currentTarget.position, targetWeight);

            Vector3 shift = mid - player.position;
            shift.y = 0f;
            shift = Vector3.ClampMagnitude(shift, maxShift);

            desired += shift;
        }

        currentWorldPos = Vector3.Lerp(
            currentWorldPos,
            desired,
            1f - Mathf.Exp(-smooth * Time.deltaTime)
        );

        transform.position = currentWorldPos;

        // ⭐⭐⭐ EN ÖNEMLİ SATIR – JITTER'I KESER
        transform.rotation = Quaternion.identity;
    }
}