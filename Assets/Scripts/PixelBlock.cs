using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PixelBlock : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Rigidbody>().useGravity = true;
        transform.DOLocalMove(transform.localPosition + GetRandomDirection(), 0.1f);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * transform.localScale.x;
    }
}