using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject pixelBlock;

    public void CreateThreeDimensionalModel()
    {
        var pixels = sprite.texture.GetPixels();
        var squaredLength = Mathf.Sqrt(pixels.Length);
        var parentPixelBlock = new GameObject();
        var scale = pixelBlock.transform.localScale.x;

        for (var i = 0; i < pixels.Length; i++)
        {
            if (GetMagnitude(pixels[i]) != 0)
            {
                var spawnedPixelBlock = Instantiate(pixelBlock, new Vector3((i % squaredLength) * scale, 
                    (int)(i / squaredLength) * scale, Random.Range(0f, 0.05f)), Quaternion.identity);

                spawnedPixelBlock.GetComponent<MeshRenderer>().material.color = pixels[i];
                spawnedPixelBlock.transform.SetParent(parentPixelBlock.transform);
                spawnedPixelBlock.transform.localScale *= 0.99f;
            }
        }
    }

    private float GetMagnitude(Color color)
    {
        return color.r + color.g + color.b;
    }
}
