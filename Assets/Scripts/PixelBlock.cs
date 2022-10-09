using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PixelBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ball>())
        {
            OnBallTouch(other);
        }
    }

    private IEnumerator ChangeColorOvertime(Material material, GameObject other)
    {
        material.Lerp(material, other.GetComponent<MeshRenderer>().material, 0.5f);
        yield return new WaitForSeconds(0.5f);
        material.Lerp(material, material, 0.5f);
    }

    public void OnBallTouch(Collider other)
    {
        transform.DOLocalRotate(
            new Vector3(Random.Range(45f, 135f), Random.Range(45f, 135f), 0f), 1.25f);
            
        var rb = transform.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), 0f, 0f), ForceMode.Impulse);
            
        var material = transform.GetComponent<MeshRenderer>().material;

        if (other != null)
        {
            StartCoroutine(ChangeColorOvertime(material, other.gameObject));
        }
    }
}