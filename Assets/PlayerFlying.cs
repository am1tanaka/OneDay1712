using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlying : MonoBehaviour {

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.NowPhase == GameController.GAME_PHASE.FLYING)
        {
            Debug.Log(rb.IsSleeping());
            // 静止をチェック
            if (rb.IsSleeping())
            {
                GameController.ChangePhase(GameController.GAME_PHASE.RESULT);
            }
        }
	}
}
