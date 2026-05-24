using UnityEngine;

public class LockOn : MonoBehaviour
{
    public float searchRadius = 12f;
    public LayerMask targetMask;          // Enemy layer'ını seç
    public Transform currentTarget;       // debug için

    public float breakDistance = 15f;     // hedef bu mesafeden uzaksa lock kapanır

    Transform owner;

    public bool IsLockedOn => currentTarget != null;

    void Update()
    {
        if (!IsLockedOn || !owner) return;

        // hedef yok olduysa kapan
        if (!currentTarget)
        {
            currentTarget = null;
            return;
        }

        float sqr = (currentTarget.position - owner.position).sqrMagnitude;

        // Çok uzaklaştıysa kapan
        if (sqr > breakDistance * breakDistance)
        {
            currentTarget = null;
            return;
        }

        // İstersen ayrıca: hedef searchRadius dışına çıkınca da kapan
        // if (sqr > searchRadius * searchRadius) currentTarget = null;
    }

    public bool ToggleLock(Transform from)
    {
        owner = from;

        if (IsLockedOn)
        {
            currentTarget = null;
            return false;
        }

        currentTarget = FindClosestTarget(from.position);
        return IsLockedOn;
    }

    Transform FindClosestTarget(Vector3 origin)
    {
        Collider[] hits = Physics.OverlapSphere(origin, searchRadius, targetMask);

        Transform best = null;
        float bestSqr = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            Transform t = hits[i].transform;
            float d = (t.position - origin).sqrMagnitude;

            if (d < bestSqr)
            {
                bestSqr = d;
                best = t;
            }
        }

        return best;
    }

    public Vector3 GetTargetFlatDirection(Vector3 fromPos)
    {
        if (!IsLockedOn) return Vector3.zero;

        Vector3 dir = currentTarget.position - fromPos;
        dir.y = 0f;

        return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector3.zero;
    }
}
