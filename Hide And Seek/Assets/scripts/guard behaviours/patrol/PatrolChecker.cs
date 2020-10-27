using UnityEngine;

namespace BT
{
    public partial class BehaviourTree
    {
        public class PatrolChecker : IStrategy
        {
         
            private blackboard localboard;
            public PatrolChecker(ref blackboard BB)
            {
                localboard = BB;
            }
            public Node.NodeStates Execute()
            {
                
                Debug.Log("patrol check "+ localboard.ReadyToPatrol);
                if (localboard.ReadyToPatrol)
                    return Node.NodeStates.SUCCESS;
                else
                    return Node.NodeStates.FAILURE;
            }
        }
    }

}