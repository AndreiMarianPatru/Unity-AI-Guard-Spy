using UnityEngine;

namespace BT
{
    public partial class BehaviourTree
    {
        public class HackCheck : IStrategy
        {
         
            private blackboard localboard;
            public HackCheck(ref blackboard BB)
            {
                localboard = BB;
            }
            public Node.NodeStates Execute()
            {
                
                Debug.Log("hack check "+ localboard.hackInProgress);
                if (localboard.hackInProgress)
                    return Node.NodeStates.SUCCESS;
                else
                    return Node.NodeStates.FAILURE;
            }
        }
    }

}