﻿using System.Collections;
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

	public void tweet()
	{
		if (GameController.HighScore > 0 || IsDebug)
		{
			naichilab.UnityRoomTweet.Tweet(
				"flying-kamo",
				"◆フライング・カモ！で記録更新！:"+(float)GameController.HighScore/100f+"m @am1tanaka",
				"unityroom"
				);
		}
	}
}