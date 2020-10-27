using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reportGuardNearby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "guard")
        {
            gameObject.GetComponentInParent<spyBT.spyBT>().MyBlackboard.GuardNearby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "guard")
        {
            gameObject.GetComponentInParent<spyBT.spyBT>().MyBlackboard.GuardNearby = false;
        }
    }
}
