using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private RayWeapon _rayWeapon;

    private void Awake()
    {
        _rayWeapon = FindObjectOfType<RayWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        var indices = other.name.Split("-");
        _rayWeapon.RemoveFromMap(int.Parse(indices[1]), int.Parse(indices[0]));
        other.gameObject.SetActive(false);
    }
}
