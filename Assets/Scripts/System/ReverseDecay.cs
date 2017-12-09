using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseDecay : MonoBehaviour {

    public Sprite RedColumn;
    public Sprite GreenColumn;
    public Sprite BlueColumn;
    public Sprite RainbowColumn;

    private void Awake()
    {
        string waveColor = PlayerPrefs.GetString("WaveColor", "Red");
        ReplaceSprite(waveColor);
    }

    public void ReverseWaveDecay() {
        var alphaDecays = GetComponentsInChildren<AlphaDecay>();

        foreach(var decay in alphaDecays) {
            decay.ReverseDecay();
        }
    }

    public void ReplaceSprite(string color)
    {
        var childSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach(var target in childSpriteRenderers)
        {
            if(color == "Red")
            {
                target.sprite = RedColumn;
            }

            else if(color == "Green")
            {
                target.sprite = GreenColumn;
            }

            else if(color == "Blue")
            {
                target.sprite = BlueColumn;
            }

            else if(color == "Rainbow")
            {
                target.sprite = RainbowColumn;
            }

            else
            {
                target.sprite = RedColumn;
            }
        }

        PlayerPrefs.SetString("WaveColor", color);
    }
}
