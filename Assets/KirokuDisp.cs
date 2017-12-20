using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirokuDisp : MonoBehaviour {

    /** 表示が完了*/
    public void Done()
    {
        GameController.ChangePhase(GameController.GAME_PHASE.RESULT_COUNT);
    }

    /** タイトルへ*/
    public void ToTitle()
    {
        GameController.ChangeScene(GameController.SCENE.TITLE);
    }
}
