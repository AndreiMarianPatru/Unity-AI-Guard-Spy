using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
    public partial class BehaviourTree
    {
        public class HackInvestigate : IStrategy
        {
            private blackboard localboard;
            public HackInvestigate(ref blackboard BB)
            {
                localboard = BB;
            }
            public Node.NodeStates Execute()
            {
                Debug.Log("hack in progress, omw");
                localboard.guard.SetDestination(localboard.objectHacked);
                if (Vector3.Distance(localboard.guard.gameObject.transform.position, localboard.objectHacked) <= 1)
                {
                    Debug.Log("nothing to see here");
                    return Node.NodeStates.SUCCESS;
                }
                return Node.NodeStates.RUNNING;
            }
        }
    }

}