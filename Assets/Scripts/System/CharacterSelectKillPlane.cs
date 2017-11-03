using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectKillPlane : MonoBehaviour
{
    Vector3 Player1OriginalPosition;
    Vector3 Player2OriginalPosition;

    private void Start()
    {
        Player1OriginalPosition = GameObject.Find("BridgeMellowMove").GetComponent<Transform>().position;
        Player2OriginalPosition = GameObject.Find("StiltMellowMove").GetComponent<Transform>().position;
    }

    //Check if the thing colliding is player 1 or two, and move if necessary.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.name == "BridgeMellowMove")
        {
            collision.collider.transform.position = Player1OriginalPosition;
        }

        else if(collision.collider.gameObject.name == "StiltMellowMove")
        {
            collision.collider.transform.position = Player2OriginalPosition;
        }
    }
}
