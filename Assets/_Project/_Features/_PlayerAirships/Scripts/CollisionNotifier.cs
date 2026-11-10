using UnityEngine;
using System;

public class CollisionNotifier : MonoBehaviour
{
    public event Action<Collider> OnHit;

    private void OnTriggerEnter(Collider other)
    {
        OnHit?.Invoke(other);
    }
}
