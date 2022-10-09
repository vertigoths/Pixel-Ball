using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using TMPro;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject pixelBlock;
    private GameObject[,] _map;
    private Vector3 _hardcodedScale = new Vector3(0.15f, 0.15f, 0.15f);

    public void CreateThreeDimensionalModel()
    {
        var pixels = sprites[Random.Range(0, sprites.Length)].texture.GetPixels();
        var squaredLength = Mathf.Sqrt(pixels.Length);

        var parent = new GameObject();

        var scale = pixelBlock.transform.localScale.x;
        _map = new GameObject[(int) squaredLength, (int) squaredLength];

        IterateOverMap(pixels, squaredLength, scale, parent);

        var localPosition = parent.transform.localPosition;
        Vector3[] path =
        {
            localPosition,
            new Vector3(localPosition.x + Random.Range(-0.45f, 0.45f), localPosition.y + Random.Range(-0.45f, 0.45f), localPosition.z), 
            new Vector3(localPosition.x + Random.Range(-0.45f, 0.45f), localPosition.y + Random.Range(-0.45f, 0.45f), localPosition.z),
            localPosition
        };

        parent.transform.DOLocalPath(path, 12f, PathType.CatmullRom).SetLoops(25);
        FindObjectOfType<BlockController>().SetMapReference(_map);
        PlayerPrefs.Save();
    }

    private float GetMagnitude(Color color)
    {
        return color.r + color.g + color.b;
    }

    private void IterateOverMap(IReadOnlyList<Color> pixels, float squaredLength, float scale, GameObject parent)
    {
        var halfOfPixels = pixels.Count / 2;
        
        for (var i = 0; i < halfOfPixels; i++)
        {
            SpawnBlock(pixels, squaredLength, scale, parent, halfOfPixels - i);
            SpawnBlock(pixels, squaredLength, scale, parent, halfOfPixels + i);
        }
    }

    private void SpawnBlock(IReadOnlyList<Color> pixels, float squaredLength, float scale, GameObject parentPixelBlock, int index)
    {
        if (GetMagnitude(pixels[index]) != 0)
        {
            var spawnedPixelBlock = Instantiate(pixelBlock, new Vector3((index % squaredLength) * scale,
                                                                (int)(index / squaredLength) * scale, Random.Range(0f, 0.05f))
                                                            + new Vector3(-2.185f, 0.5f, -0.75f), Quaternion.identity);

            spawnedPixelBlock.GetComponent<MeshRenderer>().material.color = pixels[index];
            spawnedPixelBlock.transform.SetParent(parentPixelBlock.transform);

            var posY = (int)(index / squaredLength);
            var posX = (int)(index % squaredLength);
                
            _map[posY, posX] = spawnedPixelBlock;
            
            _map[posY, posX].name = posY + "-" + posX;
        }
    }
}
