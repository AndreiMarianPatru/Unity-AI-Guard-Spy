using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
    public partial class BehaviourTree
    {
        public class ChaseStrategy : IStrategy
        {
            private blackboard localboard;
            public ChaseStrategy(ref blackboard BB)
            {
                localboard = BB;
            }
            public Node.NodeStates Execute()
            {
                Debug.Log("the chase is on");
                localboard.guard.SetDestination(localboard.SpyLastPosition);
                if (Vector3.Distance(localboard.guard.gameObject.transform.position, localboard.SpyLastPosition) <= 5)
                {
                    Debug.Log("your spy days are over");
                    return Node.NodeStates.SUCCESS;
                }
                return Node.NodeStates.RUNNING;
            }
        }
    }

}