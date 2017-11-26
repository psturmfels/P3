using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoObjectTranslate))]
public class AutoTranslateDetector : MonoBehaviour
{
    private AutoObjectTranslate selfAutoObjectTranslate;
    public bool stopped = false;

    private void Awake()
    {
        selfAutoObjectTranslate = this.GetComponent<AutoObjectTranslate>();
    }

    private void FixedUpdate()
    {
        if(stopped)
        {
            selfAutoObjectTranslate.enabled = false;
            AutoTranslateLinker selfLinker = this.GetComponent<AutoTranslateLinker>();
            if(selfLinker != null)
            {
                selfLinker.DisableLinkedTranslators();
            }
        }

        else
        {
            selfAutoObjectTranslate.enabled = true;

            AutoTranslateLinker selfLinker = this.GetComponent<AutoTranslateLinker>();
            if(selfLinker != null)
            {
                selfLinker.EnableLinkedTranslators();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StateMachineForJack selfStateMachine = collision.rigidbody.GetComponent<StateMachineForJack>();
        if((selfStateMachine.currentState == StateMachineForJack.State.InTransition) ||
            (selfStateMachine.currentState == StateMachineForJack.State.Transformed))
        {
            stopped = true;
        }
       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StateMachineForJack selfStateMachine = collision.rigidbody.GetComponent<StateMachineForJack>();
        if((selfStateMachine.currentState == StateMachineForJack.State.InTransition) ||
            (selfStateMachine.currentState == StateMachineForJack.State.Transformed))
        {
            stopped = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StateMachineForJack selfStateMachine = collision.rigidbody.GetComponent<StateMachineForJack>();
        if((selfStateMachine.currentState == StateMachineForJack.State.InTransition) ||
            (selfStateMachine.currentState == StateMachineForJack.State.Transformed))
        {
            stopped = false;
        }
        
    }
}
