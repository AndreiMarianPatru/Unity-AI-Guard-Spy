using System;
using System.Collections;
using System.Collections.Generic;
using BT;
using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public int i;

   
    // Start is called before the first frame update
    void Start()
    {
        i = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "spy")
        {
           
            
            foreach (GameObject guard in GameObject.FindGameObjectsWithTag("guard"))
            {
                guard.GetComponent<BehaviourTree>().MyBlackboard.VisibleEnemy = false;
            }
            other.gameObject.tag = "invisibleSpy";//the guard can apprehend only a spy with "spy" tag so by changing its tag we can simulate a safezone

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "invisibleSpy")
        {
            
            other.gameObject.tag = "spy";
        }
    }
}
