using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomNoise : MonoBehaviour {

    public float maxPositionNoise = 1;
    public float maxRotationNoise = 1;

    private float lastTime;
    public float speed = 10;

	// Use this for initialization
	void Start () {
        lastTime = -speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastTime > speed)
        {
            // change destination
            transform.DOLocalRotate(new Vector3(getRandomRotationComponent(), getRandomRotationComponent(), getRandomRotationComponent()), speed);
            transform.DOLocalMove(new Vector3(getRandomPositionComponent(), getRandomPositionComponent(), getRandomPositionComponent()), speed);
            lastTime = Time.time;
        }
	}

    float getRandomPositionComponent()
    {
        return UnityEngine.Random.Range(-maxPositionNoise, maxPositionNoise);
    }

    float getRandomRotationComponent()
    {
        return UnityEngine.Random.Range(-maxRotationNoise, maxRotationNoise);
    }
}
