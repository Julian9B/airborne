using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageButtonManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(5);
        DestroyMessage();
    }

    public void DestroyMessage()
    {
        Destroy(gameObject);
    }
}
