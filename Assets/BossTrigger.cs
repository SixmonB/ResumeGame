using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static BossTrigger current;
    public event Action onDoorwayTriggerEnter;
    private bool triggered = false;
    private void Awake()
    {
        current = this;
    }

    private void DoorwayTriggerEnter()
    {
        if(onDoorwayTriggerEnter != null) onDoorwayTriggerEnter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && triggered == false)
        {
            triggered = true;
            DoorwayTriggerEnter();
        }
    }
}
