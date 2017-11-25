using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BinaryObjectTranslate : MonoBehaviour
{
    private Transform selfTransform;
    private Rigidbody2D selfRigidbody2D;
    private int activeTriggers = 0;
    private bool activated = false;
    private Vector3 basePosition = Vector3.zero;        //Base position
    private float pathDistance = 0;                     //Distance that the platform must travel.
    private float speed = 0;                            //Current Scalar speed

    public List<GameObject> Activators = new List<GameObject>();
    public Vector3 endPoint = Vector3.zero;
    public float maxSpeed = 2;                          //Maximum Speed of the platform
    public float minimumSpeed = .1f;                    //Minimum Speed of the platform
    public float accelerationOffset = .5f;              //Between [0,1] Distance that the platform must be from its path midpoint
                                                        //prior to experiencing acceleration.

    

    private void Start()
    {
        selfTransform = this.GetComponent<Transform>();
        selfRigidbody2D = this.GetComponent<Rigidbody2D>();

        basePosition = selfTransform.position;

        InitializeDelegates();
        DisableEditorGrids();
        EnforceRigidbodyConstraints();
        ComputePathDistance();
    }

    private void FixedUpdate()
    {
        CheckActivators();

        //Compute the speed if acceleration at path terminus is desired
        if(accelerationOffset < 1)
        {
            ComputeSpeed();
        }

        //Displace the entity
        DisplaceEntity();
    }

    private void InitializeDelegates()
    {
        foreach(GameObject target in Activators)
        {
            if(target.GetComponent<ButtonActivate>() != null)
            {
                ButtonActivate targetButtonActivate = target.GetComponent<ButtonActivate>();
                targetButtonActivate.OnButtonPress += TriggerPressed;
                targetButtonActivate.OnButtonRelease += TriggerReleased;
            }

            else if(target.GetComponent<SwitchLatch>() != null)
            {
                SwitchLatch sl = target.GetComponent<SwitchLatch>();
                sl.OnSwitchTrigger += LatchSwitched;
            }
        }
    }

    private void CheckActivators()
    {
        activated = (activeTriggers == Activators.Count);
    }

    private void ComputeSpeed()
    {
        //Determine whether or not the entity is far enough from its path midpoint to be accelerating.
        float pathOffset = ComputePathOffset();
        if(pathOffset >= accelerationOffset)
        {
            //Determine the distance from the start of the acceleration zone
            float accelerationZoneSize = 1 - accelerationOffset;
            float accelerationZoneTraversal = pathOffset - accelerationOffset;
            float speedFactor = accelerationZoneTraversal / accelerationZoneSize;

            //Lerp the speed based on this.
            speed = Mathf.Lerp(maxSpeed, minimumSpeed, speedFactor);
        }

        else
        {
            speed = maxSpeed;
        }
    }

    private float ComputePathOffset()
    {
        float distanceFromMidpoint = Vector3.Distance(selfRigidbody2D.position, basePosition) - (pathDistance / 2);
        return Mathf.Abs(distanceFromMidpoint / (pathDistance / 2));
    }

    private void DisplaceEntity()
    {
        //Move towards next node via step
        float step = speed * Time.fixedDeltaTime;

        if(activated)
        {
            selfRigidbody2D.position = Vector3.MoveTowards(selfRigidbody2D.position, endPoint + basePosition, step);
        }

        else
        {
            selfRigidbody2D.position = Vector3.MoveTowards(selfRigidbody2D.position, basePosition, step);
        }
    }

    private void LatchSwitched()
    {
        ++activeTriggers;
    }

    private void TriggerPressed()
    {
        ++activeTriggers;
    }

    private void TriggerReleased()
    {
        --activeTriggers;
    }

    //Make sure that editor grid components are disabled on this object and all children.
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

    //Ensure that the attached rigidbody follows the necessary constraints
    private void EnforceRigidbodyConstraints()
    {
        selfRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        selfRigidbody2D.mass = 100000;
        selfRigidbody2D.gravityScale = 0;
        selfRigidbody2D.drag = 0;
        selfRigidbody2D.angularDrag = 0;
        selfRigidbody2D.freezeRotation = true;
    }

    //Compute the total path distance defined by the nodes.
    private void ComputePathDistance()
    {
        pathDistance = Vector3.Distance(Vector3.zero, endPoint);
    }
}
