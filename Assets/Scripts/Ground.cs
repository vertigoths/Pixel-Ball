using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private BallSpawner _ballSpawner;
    [SerializeField] private GameObject splashPrefab;
    private LinkedList<GameObject> _splashes;

    private UIController _uiController;

    private void Awake()
    {
        _ballSpawner = FindObjectOfType<BallSpawner>();
        _uiController = FindObjectOfType<UIController>();

        _splashes = new LinkedList<GameObject>();
        var parent = new GameObject();

        for (var i = 0; i < 20; i++)
        {
            var splash = Instantiate(splashPrefab, parent.transform, true);
            splash.SetActive(false);
            _splashes.AddLast(splash);
        }
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
            _uiController.OnBlockCollect(other.gameObject.transform.position);
            
            if (_splashes.Count > 0)
            {
                var splash = _splashes.First.Value;
                _splashes.RemoveFirst();
                
                splash.SetActive(true);
                splash.transform.localPosition = other.gameObject.transform.position + new Vector3(0f, 0.38f, 0f);
                splash.GetComponent<ParticleSystem>().Play();

                StartCoroutine(AddSplashBack(splash));
            }
        }
    }

    private IEnumerator AddSplashBack(GameObject splash)
    {
        yield return new WaitForSeconds(0.5f);
        
        splash.SetActive(false);
        _splashes.AddLast(splash);
    }
}
