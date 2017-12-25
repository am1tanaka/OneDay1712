using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tweet : MonoBehaviour {
	[TooltipAttribute("チェックするとスコアを記録していなくてもツイート可能")]
	public bool IsDebug = true;

	private Button button;

	public void Start()
	{
		button = GetComponent<Button>();

		button.interactable = (GameController.HighScore > 0) || IsDebug;
	}

    public void Update()
    {
        if (GameController.NowScene == GameController.SCENE.TITLE)
        {
            button.interactable = (GameController.HighScore > 0) || IsDebug;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void tweet()
	{
		if (GameController.HighScore > 0 || IsDebug)
		{
			naichilab.UnityRoomTweet.Tweet(
				"flying-kamo",
				"◆フライング・カモ！で"+((float)GameController.HighScore/100f).ToString("F2")+"m飛びました！",
				"unityroom",
                "unity1week",
                "flyingkamo"
				);
		}
	}
}
