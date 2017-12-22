using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDetector : MonoBehaviour
{
    public void GameStart()
    {
        if (GameController.NowScene == GameController.SCENE.TITLE) {
            SoundController.Play(SoundController.SE.START);
            GameController.ChangeScene(GameController.SCENE.GAME);
        }
    }
}
