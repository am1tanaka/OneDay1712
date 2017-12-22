using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
    [TooltipAttribute("跳ね返り音を鳴らす最低速度")]
    public float MIN_BOUND_SE = 1f;

    private Vector3 startPosition;
    private Quaternion startRoll;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        startPosition = transform.position;
        startRoll = transform.rotation;
        rb = GetComponent<Rigidbody>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= MIN_BOUND_SE)
        {
            SoundController.Play(SoundController.SE.BOUND);
        }
    }
}
