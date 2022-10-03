using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PixelBlock : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        GetComponent<Rigidbody>().useGravity = true;
    }
}