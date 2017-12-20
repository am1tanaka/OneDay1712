using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour {

    /** カメラからの距離*/
    public float TARGET_DISTANCE = 10f;

	// Update is called once per frame
	void Update () {
        if (GameController.NowPhase == GameController.GAME_PHASE.SET_TARGET)
        {
            Vector3 mpos = Input.mousePosition;
            mpos.z = TARGET_DISTANCE;
            Vector3 tpos = Camera.main.ScreenToWorldPoint(mpos);
            transform.position = tpos;
        }
    }
}
