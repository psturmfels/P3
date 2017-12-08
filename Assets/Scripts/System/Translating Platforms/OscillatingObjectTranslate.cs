using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OscillatingObjectTranslate : MonoBehaviour
{
    private Rigidbody2D selfRigidbody2D;
    private Vector2 basePosition = Vector3.zero;        //Base position
    private Vector2 midPoint = Vector3.zero;            //Distance that the platform must travel.
    private Vector2 endPosition = Vector3.zero;
    private float pathDistance = 0;
    private bool forward = true;
    private bool bridgeContact = false;
    private bool stiltContact = false;
    private ContactPoint2D[] contacts;

    public Vector2 translation = Vector3.zero;
    public float maxSpeed = 2;                          //Maximum Speed of the platform
    public float minimumSpeed = .1f;                    //Minimum Speed of the platform
    public float startingOffset = 0;
    public float accelerationOffset = .5f;              //Between [0,1] Distance that the platform must be from its path midpoint
                                                        //prior to experiencing acceleration.

    private void Awake()
    {
        DisableEditorGrids();

        selfRigidbody2D = this.GetComponent<Rigidbody2D>();

        EnforceRigidbodyConstraints();

        //Set the base position, midpoint, and endpoint.
        basePosition = selfRigidbody2D.position;
        midPoint = basePosition + (translation * .5f);
        endPosition = basePosition + translation;

        pathDistance = translation.magnitude;

        EmplaceEntity();

        contacts = new ContactPoint2D[5];
    }

    private void FixedUpdate()
    {
        //Check if motion must reverse
        if(ReverseCheck())
        {
            //Clamp motion to base/end position
            ClampPosition();
        }
        
        //Double check contacts
        CheckContacts();

        //Compute the speed
        ComputeSpeed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TransformBehavior collidingTransformBehavior = collision.gameObject.GetComponentInChildren<TransformBehavior>();
        //If colliding with a transformed player
        if(collidingTransformBehavior)
        {
            //If the transformed player is bridge
            if(collidingTransformBehavior.slideOffset.x > 0)
            {
                bridgeContact = true;
            }

            //If the transformed player is stilt
            if(collidingTransformBehavior.slideOffset.y > 0)
            {
                stiltContact = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TransformBehavior collidingTransformBehavior = collision.gameObject.GetComponentInChildren<TransformBehavior>();
        //If colliding with a transformed player
        if(collidingTransformBehavior)
        {
            //If the transformed player is bridge
            if(collidingTransformBehavior.slideOffset.x > 0)
            {
                bridgeContact = false;
            }

            //If the transformed player is stilt
            if(collidingTransformBehavior.slideOffset.y > 0)
            {
                stiltContact = false;
            }
        }
    }

    private bool ReverseCheck()
    {
        Vector2 nextPoint = forward ? endPosition : basePosition;
        float distanceToNextPoint = Vector3.Magnitude(selfRigidbody2D.position - nextPoint);

        //If the distance to the next point will be reached in the next physics frame
        if(distanceToNextPoint <= (minimumSpeed * Time.fixedDeltaTime))
        {
            //Indicate the direction has reversed and return true
            forward = !forward;
            return true;
        }

        return false;
    }

    private void ClampPosition()
    {
        //Clamp position based on which direction this is moving.
        selfRigidbody2D.position = (forward ? basePosition : endPosition);
    }

    private void ComputeSpeed()
    {
        
        if(bridgeContact || stiltContact)
        {
            selfRigidbody2D.velocity = Vector2.zero;
            return;
        }
        
        bool passedMidPoint = CheckMidpointPassed();
        Vector2 nextPoint = forward ? endPosition : basePosition;
        Vector2 lastPoint = forward ? basePosition : endPosition;
        Vector2 nextDirection = Vector3.Normalize(nextPoint - selfRigidbody2D.position);

        //If the object is far enough from the midpoint
        float distanceFromMidpoint = Vector3.Magnitude(selfRigidbody2D.position - midPoint);
        if((distanceFromMidpoint / (pathDistance / 2)) >= accelerationOffset)
        {
            //Calculate the focus for the oscillating force to be between midpoint and next point.
            Vector2 focus = Vector2.Lerp(midPoint, (passedMidPoint ? nextPoint : lastPoint), accelerationOffset);

            float distanceFromFocus = Vector3.Magnitude(selfRigidbody2D.position - focus);
            float distanceFocusToNext = Vector3.Magnitude((passedMidPoint ? nextPoint : lastPoint) - focus);

            //Adjust speed based on distance from focus
            selfRigidbody2D.velocity = nextDirection * Mathf.Lerp(maxSpeed, minimumSpeed, distanceFromFocus / distanceFocusToNext);
            return;
        }

        selfRigidbody2D.velocity = nextDirection * maxSpeed;    
    }

    private void CheckContacts()
    {
        if(selfRigidbody2D.GetContacts(contacts) == 0)
        {
            bridgeContact = false;
            stiltContact = false;
        }
    }

    private bool CheckMidpointPassed()
    {
        //Check if the direction from this object to the midpoint is the opposite of the direction from the midpoint
        //to the next point. This would be true if the object has passed the midpoint.
        Vector2 nextPoint = forward ? endPosition : basePosition;
        Vector3 nextDirectionFromMidPoint = Vector3.Normalize(nextPoint - midPoint);
        Vector3 directionFromSelfToMidPoint = Vector3.Normalize(midPoint - selfRigidbody2D.position);

        if(directionFromSelfToMidPoint == -nextDirectionFromMidPoint)
        {
            return true;
        }

        return false;
    }

    private void EmplaceEntity()
    {
        selfRigidbody2D.position = basePosition + (translation * Mathf.Clamp01(startingOffset));
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
        selfRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        selfRigidbody2D.freezeRotation = true;
        selfRigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
}
