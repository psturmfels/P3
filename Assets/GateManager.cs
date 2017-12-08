using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    public SpriteRenderer Level1;
    public SpriteRenderer Level2;
    public SpriteRenderer Level3;
    public SpriteRenderer Level4;
    public SpriteRenderer Level5;
    public SpriteRenderer Level6;
    public SpriteRenderer Level7;

    public Sprite Red;
    public Sprite Green;
    public Sprite Blue;
    public Sprite Rainbow;


    // Use this for initialization
    void Start ()
    {
        int Level1Finish = PlayerPrefs.GetInt("Level1Finish", 0);
        int Level2Finish = PlayerPrefs.GetInt("Level2Finish", 0);
        int Level3Finish = PlayerPrefs.GetInt("Level3Finish", 0);
        int Level4Finish = PlayerPrefs.GetInt("Level4Finish", 0);
        int Level5Finish = PlayerPrefs.GetInt("Level5Finish", 0);
        int Level6Finish = PlayerPrefs.GetInt("Level6Finish", 0);
        int Level7Finish = PlayerPrefs.GetInt("Level7Finish", 0);

        if(Level1Finish == 0)
        {
            Level1.sprite = Green;
        }

        if(Level1Finish != 0)
        {
            Level1.sprite = Rainbow;
        }

        if(Level2Finish == 0)
        {
            Level2.sprite = Green;
        }

        if(Level2Finish != 0)
        {
            Level2.sprite = Rainbow;
        }

        if(Level3Finish == 0)
        {
            Level3.sprite = Red;
        }

        if(Level3Finish != 0)
        {
            Level3.sprite = Rainbow;
        }

        if(Level4Finish == 0)
        {
            Level4.sprite = Red;
        }

        if(Level4Finish != 0)
        {
            Level4.sprite = Rainbow;
        }

        if(Level5Finish == 0)
        {
            Level5.sprite = Red;
        }

        if(Level5Finish != 0)
        {
            Level5.sprite = Rainbow;
        }

        if(Level6Finish == 0)
        {
            Level6.sprite = Blue;
        }

        if(Level6Finish != 0)
        {
            Level6.sprite = Rainbow;
        }

        if(Level7Finish == 0)
        {
            Level7.sprite = Blue;
        }

        if(Level7Finish != 0)
        {
            Level7.sprite = Rainbow;
        }
    }
}
