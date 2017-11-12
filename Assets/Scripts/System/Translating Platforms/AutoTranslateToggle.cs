using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AutoObjectTranslate))]
public class AutoTranslateToggle : MonoBehaviour
{
    public bool startEnabled = true;
    public List<GameObject> Activators = new List<GameObject>();

    private int active = 0;
    private AutoTranslateLinker selfAutoLinker;
    private AutoObjectTranslate selfAutoObjectTranslate;

    private void Awake()
    {
        selfAutoLinker = this.GetComponent<AutoTranslateLinker>();
        selfAutoObjectTranslate = this.GetComponent<AutoObjectTranslate>();
    }

    private void Start()
    {
        InitializeDelegates();
        InitializeLinkers();
    }

    private void Update()
    {
        checkActivators();
    }

    private void InitializeLinkers()
    {
        if((selfAutoLinker != null) && (startEnabled))
        {
            selfAutoLinker.EnableLinkedTranslators();
        }

        else if(!startEnabled)
        {
            selfAutoObjectTranslate.enabled = false;
        }
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
                if(startEnabled)
                {
                    sl.OnSwitchTrigger += LatchSwitched;
                }
                else
                {
                    sl.OnSwitchTrigger += TriggerPressed;
                }
            }
        }
    }

    private void checkActivators()
    {
        if(startEnabled)
        {
            if(active == Activators.Count)
            {
                selfAutoObjectTranslate.enabled = false;
            }

            else
            {
                if(selfAutoLinker != null)
                {
                    selfAutoLinker.EnableLinkedTranslators();
                }

                selfAutoObjectTranslate.enabled = true;
            }
        }

        else if(!startEnabled)
        {
            if(active == Activators.Count)
            {
                if(selfAutoLinker != null)
                {
                    selfAutoLinker.EnableLinkedTranslators();
                }

                selfAutoObjectTranslate.enabled = true;
            }

            else
            {
                selfAutoObjectTranslate.enabled = false;
            }
        }
    }

    private void LatchSwitched()
    {
        ++active;
        return;
    }

    private void TriggerPressed()
    {
        ++active;
        return;
    }

    private void TriggerReleased()
    {
        --active;
        return;
    }
}
