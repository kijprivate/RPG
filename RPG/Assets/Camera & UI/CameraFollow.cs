using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject player;
    private Vector3 distanceBetween;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        distanceBetween = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        transform.position = player.transform.position+distanceBetween;
    }
}
