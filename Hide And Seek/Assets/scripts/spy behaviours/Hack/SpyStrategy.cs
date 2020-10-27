using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

namespace spyBT
{
    public partial class spyBT
    {
        public class SpyStrategy : IStrategy
        {
            private blackboard localboard;

            public SpyStrategy(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                //Debug.Log("spy run");
                //if (!localboard.spy.pathPending)
                //{
                //    //if (localboard.spy.remainingDistance <= localboard.spy.stoppingDistance)
                //    //{
                //    //    if (!localboard.spy.hasPath || localboard.spy.velocity.sqrMagnitude == 0f)
                //    //    {
                //    //        Debug.Log("here1");
                //    //        localboard.ReadyToHack = false;
                //    //        //float time = Time.time;
                //    //        //Debug.Log(time);
                //    //        //while (true)
                //    //        //{
                //    //        //    if (Time.time == time + 1)
                //    //        //    {
                //    //        //        break;
                //    //        //    }
                //    //        //}
                //    //        //Debug.Log("Successful hack, well done agent 47");
                //    //        //localboard.NeedTarget = true;
                //    //        //return Node.NodeStates.SUCCESS;
                //    //    }
                //    //}
                //}
                if (localboard.spy.gameObject.transform.position !=
                    localboard.goals[localboard.chosen].transform.position)
                {
                    Debug.Log("walking");
                    localboard.spy.SetDestination(localboard.goals[localboard.chosen].transform.position);
                }
                return Node.NodeStates.RUNNING;

                

            }




        }
    }

}