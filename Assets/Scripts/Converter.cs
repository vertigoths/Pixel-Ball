using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject pixelBlock;
    private GameObject[,] _map;

    public void CreateThreeDimensionalModel()
    {
        var pixels = sprite.texture.GetPixels();
        var squaredLength = Mathf.Sqrt(pixels.Length);

        var parentPixelBlock = new GameObject();

        var scale = pixelBlock.transform.localScale.x;
        _map = new GameObject[(int) squaredLength, (int) squaredLength];

        for (var i = 0; i < pixels.Length; i++)
        {
            if (GetMagnitude(pixels[i]) != 0)
            {
                var spawnedPixelBlock = Instantiate(pixelBlock, new Vector3((i % squaredLength) * scale, 
                    (int)(i / squaredLength) * scale, Random.Range(0f, 0.05f)), Quaternion.identity);
                
                spawnedPixelBlock.GetComponent<MeshRenderer>().material.color = pixels[i];
                spawnedPixelBlock.transform.SetParent(parentPixelBlock.transform);
                spawnedPixelBlock.transform.localScale *= 0.99f;

                var posY = (int)(i / squaredLength);
                var posX = (int)(i % squaredLength);
                
                _map[posY, posX] = spawnedPixelBlock;
                
                _map[posY, posX].name = posY + "-" + posX;
            }
        }

        parentPixelBlock.transform.position = new Vector3(-2f, 0.5f, -0.75f);
    }

    private float GetMagnitude(Color color)
    {
        return color.r + color.g + color.b;
    }

    public GameObject[,] GetMap()
    {
        return _map;
    }
}
