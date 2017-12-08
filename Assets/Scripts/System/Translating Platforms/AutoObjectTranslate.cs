using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automated Object Translator.
[RequireComponent(typeof(Rigidbody2D))]     //Must be attached to a kinematic rigidbody.
public class AutoObjectTranslate : MonoBehaviour
{
    private Rigidbody2D selfRigidbody2D;
    private float traveledDistance = 0;                 //Keeps track of how far along its path this has moved
    private float pathDistance = 0;                     //Distance that the platform must travel.
    private float speed = 0;                            //Current Scalar speed
    private Vector3 basePosition = Vector3.zero;        //Base position
    private int nextIndex = 0;                          //Index of the next node
    
    public List<Vector3> nodes;                         //Nodes are defined relative to the starting position of the platform.

    //Movement Parameters
    public float maxSpeed = 2;                          //Maximum Speed of the platform
    public float minimumSpeed = .1f;                    //Minimum Speed of the platform
    public float startingOffset = 0;                    //Between [0,1] starting offset along path from 0th node.
    public float accelerationOffset = .5f;              //Between [0,1] Distance that the platform must be from its path midpoint
                                                        //prior to experiencing acceleration.


    private void Awake()
    {
        DisableEditorGrids();

        selfRigidbody2D = this.GetComponent<Rigidbody2D>();

        EnforceRigidbodyConstraints();

        //Set the base position.
        basePosition = selfRigidbody2D.position;

        ComputePathDistance();

        EmplaceEntity();
    }

    private void FixedUpdate()
    {
        //Update the next node
        UpdateNodes();

        //Compute the speed if acceleration at path terminus is desired
        if(accelerationOffset < 1)
        {
            ComputeSpeed();
        }

        //Displace the entity
        DisplaceEntity();
    }

    //Compute the total path distance defined by the nodes.
    private void ComputePathDistance()
    {
        Vector3 current = nodes[0];
        Vector3 next = nodes[0];

        //Look at the next pair of nodes, and add their distance to the total.
        foreach(Vector3 targetNode in nodes)
        {
            current = next;
            next = targetNode;
            pathDistance += Vector3.Distance(current, next);
        }
    }

    //Emplace the entity at the position along the path based on its starting offset
    private void EmplaceEntity()
    {
        if(startingOffset == 0)
        {
            selfRigidbody2D.position = nodes[0] + basePosition;
            return;
        }

        float emplacedDistance = startingOffset * pathDistance;     //Emplaced distance traveled
        float nodeDistance = 0;                                     //Distance of currently considered node from 0th node.

        //Determine which index ought to be the next index
        while(nodeDistance < emplacedDistance)
        {
            ++nextIndex;
            nodeDistance += Vector3.Distance(nodes[nextIndex - 1], nodes[nextIndex]);
        }

        
        float distanceBetweenNodes = Vector3.Distance(nodes[nextIndex - 1], nodes[nextIndex]);      //Distance between the most previous node and next one
        float previousNodeDistance = nodeDistance - distanceBetweenNodes;                           //Distance from start to previous node
        float emplacedOffset = (emplacedDistance - previousNodeDistance) / distanceBetweenNodes;    //Normalized offset from previous node to next node

        //Emplace entity at the correct position.
        selfRigidbody2D.position = (Vector3.Lerp(nodes[nextIndex - 1], nodes[nextIndex], emplacedOffset) + basePosition);

        return;
    }

    //Update Nodes - Return true if nodes needed to be updated
    private void UpdateNodes()
    {
        Vector3 nextPosition = basePosition + nodes[nextIndex];
        Vector3 currentPosition = selfRigidbody2D.position;
        if(currentPosition == nextPosition)
        {
            //Increment next node
            ++nextIndex;

            //Reached the final node, reverse the list and start from 1.
            if(nextIndex == nodes.Count)
            {
                nodes.Reverse();
                nextIndex = 1;
                traveledDistance = 0;
            }
        }
    }

    //Compute the speed
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

        return;
    }

    //Displace the entity
    private void DisplaceEntity()
    {
        Vector2 nextPosition = nodes[nextIndex] + basePosition;
        selfRigidbody2D.velocity = Vector3.Normalize(nextPosition - selfRigidbody2D.position) * speed;
        //Move towards next node via step
        /*
        float step = speed * Time.fixedDeltaTime;
        Vector3 nextPosition = Vector3.MoveTowards(selfRigidbody2D.position, nodes[nextIndex] + basePosition, step);

        //Update distance traveled
        traveledDistance += Vector3.Distance(selfRigidbody2D.position, nextPosition);

        selfRigidbody2D.position = nextPosition;
        */
        traveledDistance += selfRigidbody2D.velocity.magnitude * Time.fixedDeltaTime;
        return;
    }

    //Compute the current distance from the midpoint.
    private float ComputePathOffset()
    {
        float distanceFromMidpoint = traveledDistance - (pathDistance / 2);
        return Mathf.Abs(distanceFromMidpoint / (pathDistance / 2));
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
        //selfRigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

}