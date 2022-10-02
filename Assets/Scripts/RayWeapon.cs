using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayWeapon : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SendRay();
        }
    }

    private void SendRay()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 10))
        {
            if (hit.transform != null)
            {
                hit.transform.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
