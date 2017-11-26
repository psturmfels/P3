using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoObjectTranslate))]
public class AutoTranslateLinker : MonoBehaviour
{
    public List<AutoObjectTranslate> LinkedTranslators = new List<AutoObjectTranslate>();

    private AutoObjectTranslate selfAutoObjectTranslate;

    private void Awake()
    {
        selfAutoObjectTranslate = this.GetComponent<AutoObjectTranslate>();
        LinkedTranslators.Add(selfAutoObjectTranslate);
    }

    private void FixedUpdate()
    {
        if(!CheckLinkState())
        {
            DisableLinkedTranslators();
        }
    }

    private bool CheckLinkState()
    {
        //If one of the translators is disabled, disable all linked movement.
        foreach(AutoObjectTranslate target in LinkedTranslators)
        {
            if((target.enabled == false))
            {
                return false;
            }
        }

        return true;
    }

    public void DisableLinkedTranslators()
    {
        foreach(AutoObjectTranslate target in LinkedTranslators)
        {
            target.enabled = false;
        }
    }

    public void EnableLinkedTranslators()
    {
        foreach(AutoObjectTranslate target in LinkedTranslators)
        {
            target.enabled = true;
        }
    }
}