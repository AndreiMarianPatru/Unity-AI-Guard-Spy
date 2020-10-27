using System;
using BT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionScript : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)//if the spy is in the guard's cone of vision we must check if the guard can actually see the spy( he is not over a wall)
    {
        RaycastHit hit;
        LayerMask newMask = ~mask.value;
        if (Physics.Raycast(transform.parent.transform.position,
            (other.transform.position - transform.parent.transform.position), out hit, 15, newMask))
        {

            if (hit.transform == other.transform && other.tag=="spy")
            {
                Debug.Log("Spy is now visible! ");
                gameObject.GetComponentInParent<BehaviourTree>().MyBlackboard.VisibleEnemy = true;
                gameObject.GetComponentInParent<BehaviourTree>().MyBlackboard.SpyLastPosition =other.transform.position;
                    

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponentInParent<BehaviourTree>().MyBlackboard.VisibleEnemy = false;
    }
}
