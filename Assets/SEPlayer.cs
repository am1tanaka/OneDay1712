using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour {

    public void PlaySE()
    {
        SoundController.Stop();
        SoundController.Play(SoundController.SE.GUAGEUP);
    }
}
