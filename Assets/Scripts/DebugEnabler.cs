using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnabler : MonoBehaviour {
    private bool lastDebug = true;

	// Update is called once per frame
	void Update () {
	    if (GameController.isDebug != lastDebug)
        {
            lastDebug = GameController.isDebug;
            gameObject.SetActive(lastDebug);
        }	
	}
}
