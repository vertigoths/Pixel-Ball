using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using TMPro;
using UnityEngine;

public class Converter : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject pixelBlock;
    private GameObject[,] _map;
    private Vector3 _hardcodedScale = new Vector3(0.15f, 0.15f, 0.15f);

    public void CreateThreeDimensionalModel()
    {
        var pixels = sprite.texture.GetPixels();
        var squaredLength = Mathf.Sqrt(pixels.Length);

        var parent = new GameObject();

        var scale = pixelBlock.transform.localScale.x;
        _map = new GameObject[(int) squaredLength, (int) squaredLength];

        StartCoroutine(IterateOverMap(pixels, squaredLength, scale, parent));
    }

    private float GetMagnitude(Color color)
    {
        return color.r + color.g + color.b;
    }

    private IEnumerator IterateOverMap(IReadOnlyList<Color> pixels, float squaredLength, float scale, GameObject parent)
    {
        var halfOfPixels = pixels.Count / 2;
        
        for (var i = 0; i < halfOfPixels; i++)
        {
            SpawnBlock(pixels, squaredLength, scale, parent, halfOfPixels - i);
            SpawnBlock(pixels, squaredLength, scale, parent, halfOfPixels + i);

            yield return null;
        }
        
        FindObjectOfType<LevelController>().ChangeGameState(GameState.Play);
        FindObjectOfType<BlockController>().SetMapReference(_map);
    }

    private void SpawnBlock(IReadOnlyList<Color> pixels, float squaredLength, float scale, GameObject parentPixelBlock, int index)
    {
        if (GetMagnitude(pixels[index]) != 0)
        {
            var spawnedPixelBlock = Instantiate(pixelBlock, new Vector3((index % squaredLength) * scale,
                                                                (int)(index / squaredLength) * scale, Random.Range(0f, 0.05f))
                                                            + new Vector3(-2f, 0.5f, -0.75f), Quaternion.identity);

            spawnedPixelBlock.GetComponent<MeshRenderer>().material.color = pixels[index];
            spawnedPixelBlock.transform.SetParent(parentPixelBlock.transform);

            var posY = (int)(index / squaredLength);
            var posX = (int)(index % squaredLength);
                
            _map[posY, posX] = spawnedPixelBlock;
            
            _map[posY, posX].name = posY + "-" + posX;
        }
    }
}
