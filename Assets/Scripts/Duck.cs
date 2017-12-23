using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {
    private static Duck instance;

    [TooltipAttribute("跳ね返り音を鳴らす最低速度")]
    public float MIN_BOUND_SE = 0.2f;

    [TooltipAttribute("回転時のLerpの割合(deltaTimeを掛けるので内部的に60倍した値を使う)"), Range(0, 1)]
    public float rotateDumping = 0.2f;
    /** プレイヤーモデルのトランスフォーム*/
    private Transform playerModel;
    // プレイヤーが向く方向
    private Vector3 rotateTarget;

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
        playerModel = gameObject.transform.Find("DuckB").GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        startRoll = transform.rotation;
        SetTitleRotate();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
	}

    /**
     * 指定した座標を向くように設定
     * @params Vector3 プレイヤーが見る場所
     */
    public static void SetPlayerRoll(Vector3 target)
    {
        instance.rotateTarget = target;
    }

    /**
     * タイトル画面時の向きに設定
     */
    public static void SetTitleRotate()
    {
        instance.rotateTarget = instance.transform.position + instance.startRoll*Vector3.forward;
    }

    private void Update()
    {
        Quaternion to = Quaternion.LookRotation(rotateTarget - transform.position);
        playerModel.rotation =
            Quaternion.Lerp(playerModel.rotation, to, rotateDumping * 60f * Time.deltaTime);
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
