using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseDecay : MonoBehaviour {

    public void ReverseWaveDecay() {
        var alphaDecays = GetComponentsInChildren<AlphaDecay>();

        foreach(var decay in alphaDecays) {
            decay.ReverseDecay();
        }
    }
}
