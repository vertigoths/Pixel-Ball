using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RayWeapon : MonoBehaviour
{
    private Camera _camera;
    private GameObject[,] _map;
    private int blockCount;

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
                RemoveBlocks(hit.transform.gameObject);
            }
        }
    }

    public void SetMapReference(GameObject[,] map)
    {
        _map = map;
        
        StoreBlocksCount(_map);
    }

    private void StoreBlocksCount(GameObject[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    blockCount++;
                }
            }
        }
    }

    public void RemoveFromMap(int posX, int posY)
    {
        _map[posY, posX] = null;
        blockCount -= 1;

        if (CheckIfAllBlocksRemoved())
        {
            Debug.Log("Finished!");
        }
    }

    private void RemoveBlocks(GameObject hitBlock)
    {
        var indices = hitBlock.name.Split("-");
        var posY = int.Parse(indices[0]);
        var posX = int.Parse(indices[1]);

        _map[posY, posX].GetComponent<Rigidbody>().useGravity = true;
        
        RemoveAdjacentBlocks(posX, posY);
    }

    private bool CheckIfAllBlocksRemoved()
    {
        return blockCount == 0;
    }

    private void RemoveAdjacentBlocks(int posX, int posY)
    {
        if (posX != 0 && _map[posY, posX - 1] != null)
        {
            _map[posY, posX - 1].GetComponent<Rigidbody>().useGravity = true;
        }
        else if (posX != _map.GetLength(1) && _map[posY, posX + 1] != null)
        {
            _map[posY, posX + 1].GetComponent<Rigidbody>().useGravity = true;
        }

        if (posY != 0 && _map[posY - 1, posX] != null)
        {
            _map[posY - 1, posX].GetComponent<Rigidbody>().useGravity = true;
        }
        else if (posY != _map.GetLength(0) && _map[posY + 1, posX])
        {
            _map[posY + 1, posX].GetComponent<Rigidbody>().useGravity = true;
        }
    }
}