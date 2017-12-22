using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingButton : MonoBehaviour {

	public void CallRanking() {
		{
            GameController.ChangeScene(GameController.SCENE.RANKING);
		}
	}
}
