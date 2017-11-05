using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectKillPlane : MonoBehaviour
{
    Vector3 Player1OriginalPosition;
    Vector3 Player2OriginalPosition;

    private void Start()
    {
        Player1OriginalPosition = GameObject.Find("BridgeMellow").GetComponent<Transform>().position;
        Player2OriginalPosition = GameObject.Find("StiltMellow").GetComponent<Transform>().position;
    }

    //Check if the thing colliding is player 1 or two, and move if necessary.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.rigidbody.gameObject.name == "BridgeMellow")
        {
            collision.rigidbody.transform.position = Player1OriginalPosition;
        }

        else if(collision.rigidbody.gameObject.name == "StiltMellow")
        {
            collision.rigidbody.transform.position = Player2OriginalPosition;
        }
    }
}
