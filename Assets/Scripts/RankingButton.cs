using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingButton : MonoBehaviour {
    // ランキングボタンのインスタンス
    private Button rankingButton;

    private void Awake()
    {
        rankingButton = GetComponent<Button>();
    }

    /** 有効無効を確認*/
    public void Update()
    {
        if (GameController.NowScene == GameController.SCENE.TITLE)
        {
            rankingButton.interactable = true;
        }
        else
        {
            rankingButton.interactable = false;
        }
    }

    /** ランキングを表示*/
    public void CallRanking()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(GameController.HighScore);
    }
}
