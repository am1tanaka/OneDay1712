using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPower : MonoBehaviour {

    [TooltipAttribute("パワーバー")]
    public GameObject FillingObject;

    [TooltipAttribute("パーセント表示")]
    public TextMeshProUGUI textPercent;

    [TooltipAttribute("最大値になる秒数")]
    public float MAX_SECONDS = 1f;

    [TooltipAttribute("パワーの再生速度")]
    public float ANIME_SPEED = 1.2f;

    /** ターゲットのワールド座標*/
    private Vector3 targetWorldPosition;

    /** 円形のイメージ*/
    private Image fillingImage;

    /** アニメーター*/
    private Animator anim;

    /** 射出パワー*/
    public static int power;

    /** シーン切り替え*/
    public void StartPower(Vector3 pos)
    {
        targetWorldPosition = pos;
        FillingObject.transform.position = pos;
        FillingObject.SetActive(true);
        FillingObject.transform.position = Camera.main.WorldToScreenPoint(pos);
        anim = FillingObject.GetComponent<Animator>();
        anim.speed = ANIME_SPEED;
        anim.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update () {
		if (GameController.NowPhase == GameController.GAME_PHASE.POWER)
        {
            // パラメーターの表示
            float fpower = Mathf.Clamp01(Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f) + 0.01f);
            power = (int)(fpower * 100f);
            textPercent.text = "" + power + "%";

            // 射出
            if (Input.GetButtonUp("Jump"))
            {
                anim.SetTrigger("Flying");
                GameController.ChangePhaser(GameController.GAME_PHASE.FLYING);
            }

            // 右クリックでキャンセル
            if (Input.GetMouseButtonDown(1))
            {
                // 戻す
                GameController.ChangePhaser(GameController.GAME_PHASE.SET_TARGET);
                anim.SetTrigger("Abort");
            }
        }
	}
}
