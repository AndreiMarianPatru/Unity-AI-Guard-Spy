using System;
using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class HackAlarm : MonoBehaviour
{
    public GameObject[] guards;
    // Start is called before the first frame update
    void Start()
    {
            guards=GameObject.FindGameObjectsWithTag("guard");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "spy")
        {
            foreach (GameObject guard in guards)
            {
                guard.GetComponent<BehaviourTree>().MyBlackboard.hackInProgress = true;
                guard.GetComponent<BehaviourTree>().MyBlackboard.objectHacked = transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "spy")
        {
            foreach (GameObject guard in guards)
            {
                guard.GetComponent<BehaviourTree>().MyBlackboard.hackInProgress = false;
               
            }
        }
    }
}
