using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSetTarget : MonoBehaviour {
    [TooltipAttribute("標的オブジェクト")]
    public GameObject uiTarget;

    [TooltipAttribute("標的を表示する奥行き")]
    public float TARGET_KYORI = 10f;

    [TooltipAttribute("ゲーム中の仮想カメラのインスタンス")]
    public CinemachineVirtualCamera virtualCamera;

    [TooltipAttribute("マウスで操作するターゲットのトランスフォーム")]
    public Transform mouseTarget;

    /** 射出パワー制御*/
    private PlayerPower playerPower;

	// Use this for initialization
	void Start () {
        playerPower = GetComponent<PlayerPower>();
	}

    /** 標的操作開始処理*/
    public void StartSetTarget()
    {
        virtualCamera.LookAt = mouseTarget;
    }
	
	// Update is called once per frame
	void Update () {
		if (GameController.NowPhase == GameController.GAME_PHASE.SET_TARGET)
        {
            Vector3 tpos = Camera.main.WorldToScreenPoint (mouseTarget.position);
            uiTarget.transform.position = tpos;

            // パワー決定へ以降
            if (Input.GetButtonDown("Jump"))
            {
                GameController.ChangePhaser(GameController.GAME_PHASE.POWER);
            }
        }
    }
}
