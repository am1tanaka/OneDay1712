using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPower : MonoBehaviour {

    [TooltipAttribute("パワーバー")]
    public GameObject FillingObject;

    /** ターゲットのワールド座標*/
    private Vector3 targetWorldPosition;

    /** シーン切り替え*/
    public void StartPower(Vector3 pos)
    {
        targetWorldPosition = pos;
        FillingObject.transform.position = pos;
        FillingObject.SetActive(true);
        FillingObject.transform.position = Camera.main.WorldToScreenPoint(pos);
    }

    // Update is called once per frame
    void Update () {
		if (GameController.NowPhase == GameController.GAME_PHASE.POWER)
        {
            // クリック
            if (Input.GetButtonUp("Jump"))
            {

            }

            // 右クリックでキャンセル
            if (Input.GetMouseButtonDown(1))
            {
                // 戻す
                GameController.ChangePhaser(GameController.GAME_PHASE.SET_TARGET);
            }
        }
	}
}
