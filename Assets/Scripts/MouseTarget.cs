using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour {

    [TooltipAttribute("カメラからの距離")]
    public float TARGET_DISTANCE = 10f;

    [TooltipAttribute("左方向の上限角度")]
    public float LEFT_MAX = -30f;

    [TooltipAttribute("右方向の上限角度")]
    public float RIGHT_MAX = 110f;

    [TooltipAttribute("上方向の上限角度")]
    public float UP_MAX = 60f;

    [TooltipAttribute("下方向の上限角度")]
    public float DOWN_MAX = 0f;

    [TooltipAttribute("カメラのフォローオブジェクトのtransform")]
    public Transform followObjectTransform;

    [TooltipAttribute("カモのtransform")]
    public Transform kamoTransform;

    /** 基本の視線方向*/
    private Vector3 defaultVector;
    /** 基本視線の水平要素*/
    private Vector3 defaultH;
    /** 基本視線の垂直要素*/
    private Vector3 defaultV;

    /** 基本姿勢のQuaternion*/
    private Quaternion defaultQuaternion;
    /** 基本姿勢のQuaternionのInverse*/
    private Quaternion defaultQuaternionInverse;

    private void Awake()
    {
        // 基本方向を算出
        defaultVector = (kamoTransform.position-followObjectTransform.position).normalized;
        defaultH = new Vector3(defaultVector.x, 0f, defaultVector.z);
        defaultV = new Vector3(0f, defaultVector.y, defaultVector.z);
        defaultQuaternion = Quaternion.LookRotation(defaultVector);
        defaultQuaternionInverse = Quaternion.Inverse(defaultQuaternion);
    }

    // Update is called once per frame
    void Update () {
        if (GameController.NowPhase == GameController.GAME_PHASE.SET_TARGET)
        {
            Vector3 mpos = Input.mousePosition;
            mpos.z = TARGET_DISTANCE;
            Vector3 tpos = Camera.main.ScreenToWorldPoint(mpos);

            // 射出方向は、カモからマウスターゲットへの方向
            // 1. 初期方向を向くQuaternionを求めておく
            // 2. カモからマウスターゲットへの向きを、1のQuaternionのInverseで補正
            // 3. 1の前方と、2のベクトルの成す角を求める

            // 新しい向き
            Vector3 newdir = (tpos - followObjectTransform.position).normalized;
            // 基本姿勢から見るように変形
            Vector3 fromDef = defaultQuaternionInverse*newdir;

            Vector3 euler = Quaternion.LookRotation(fromDef).eulerAngles;

            // 角度を算出して範囲を調整
            float h = euler.y > 180f ? euler.y - 360f : euler.y;
            float v = euler.x > 180f ? euler.x - 360f : euler.x;

            h = Mathf.Clamp(h, LEFT_MAX, RIGHT_MAX);
            v = -Mathf.Clamp(-v, DOWN_MAX, UP_MAX);

            // 求めた範囲から補正後の的の座標を算出
            Quaternion rot = defaultQuaternion*Quaternion.Euler(v, h, 0f);
            Vector3 nextdir = rot * Vector3.forward;
            tpos = nextdir * TARGET_DISTANCE + kamoTransform.position;

            transform.position = tpos;
        }
    }
}
