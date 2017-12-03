using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingKillPlane : MonoBehaviour
{
    private Rigidbody2D selfRigidbody2D;
    private MellowCrushed stiltMellowCrushed;
    public Vector3 basePosition = Vector3.zero;

    public float speed = 2;
    public Vector3 endPoint = Vector3.zero;

    private void Awake()
    {
        selfRigidbody2D = this.GetComponent<Rigidbody2D>();
        stiltMellowCrushed = GameObject.Find("StiltMellow").GetComponent<MellowCrushed>();
        basePosition = selfRigidbody2D.position;

        EnforceRigidbodyConstraints();
        DisableEditorGrids();
    }

    private void Start()
    {
        stiltMellowCrushed.Respawn += ReturnToBasePosition;
    }

    private void FixedUpdate()
    {
        DisplaceEntity();
    }

    private void DisplaceEntity()
    {
        //Move towards next node via step
        float step = speed * Time.fixedDeltaTime;
        selfRigidbody2D.position = Vector3.MoveTowards(selfRigidbody2D.position, endPoint + basePosition, step);
        return;
    }

    private void ReturnToBasePosition()
    {
        selfRigidbody2D.position = basePosition;
    }

    private void EnforceRigidbodyConstraints()
    {
        selfRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        selfRigidbody2D.mass = 100000;
        selfRigidbody2D.gravityScale = 0;
        selfRigidbody2D.drag = 0;
        selfRigidbody2D.angularDrag = 0;
        selfRigidbody2D.freezeRotation = true;
    }

    private void DisableEditorGrids()
    {
        EditorGrid selfEditorGrid = this.GetComponent<EditorGrid>();

        if(selfEditorGrid != null)
        {
            selfEditorGrid.enabled = false;
        }

        EditorGrid[] childEditorGrids = this.GetComponentsInChildren<EditorGrid>();

        foreach(EditorGrid target in childEditorGrids)
        {
            target.enabled = false;
        }

        return;
    }
}
