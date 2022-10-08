using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private BallSpawner _ballSpawner;

    private void Awake()
    {
        _ballSpawner = FindObjectOfType<BallSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            _ballSpawner.AddBallBack(other.gameObject);
        }

        else
        {
            other.gameObject.SetActive(false);
        }
    }
}
