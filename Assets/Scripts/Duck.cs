using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
    private static Duck instance;

    [TooltipAttribute("跳ね返り音を鳴らす最低速度")]
    public float MIN_BOUND_SE = 1f;

    private Vector3 startPosition;
    private Quaternion startRoll;
    private Rigidbody rb;
    private Animator anim;

    public enum ANIM
    {
        IDLE,
        WALK,
        RUN,
        JUMP,
        EAT,
        REST
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        startRoll = transform.rotation;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
	}

    /** アニメを設定する*/
    public static void SetAnim(ANIM state)
    {
        instance.anim.SetInteger("animation", (int)state);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude >= MIN_BOUND_SE)
        {
            anim.SetTrigger("Jitabata");
            SoundController.Play(SoundController.SE.BOUND);
        }
    }
}
