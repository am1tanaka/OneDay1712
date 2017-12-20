using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
    private Vector3 startPosition;
    private Quaternion startRoll;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        startRoll = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
