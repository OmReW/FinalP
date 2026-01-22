using UnityEngine;

public class KameraKod : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
            );
    }
}