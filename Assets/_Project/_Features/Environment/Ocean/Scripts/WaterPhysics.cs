using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public float waterLevel = 0f;
    public float maxBuoyancyForce = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (!rb) return;

        Collider col = other;
        float objectTop = col.bounds.max.y;
        float objectBottom = col.bounds.min.y;

        float submersion =
            Mathf.Clamp01((waterLevel - objectBottom) / (objectTop - objectBottom));

        if (submersion <= 0f)
            return;

        Vector3 buoyancy = Vector3.up * maxBuoyancyForce * submersion;

        rb.AddForce(buoyancy, ForceMode.Force);
    }
}
