using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroCollider : MonoBehaviour
{
    private void FixedUpdate()
    {
        this.transform.localPosition = Vector3.zero;
    }
}
