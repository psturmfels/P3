using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneCheckpoint : MonoBehaviour
{
    public MovingKillPlane movingKillPlane;
    public Vector3 resetPosition = Vector3.zero;

    private bool bridgeContact = false;
    private bool stiltContact = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Contains("Bridge"))
        {
            bridgeContact = true;
        }

        if(collision.gameObject.name.Contains("Bridge"))
        {
            stiltContact = true;
        }

        if(bridgeContact && stiltContact)
        {
            movingKillPlane.basePosition = resetPosition;
        }
    }


}
