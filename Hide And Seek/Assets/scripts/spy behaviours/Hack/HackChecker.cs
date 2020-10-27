using UnityEngine;

namespace spyBT
{
    public partial class spyBT
    {
        public class HackChecker : IStrategy
        {
            private blackboard localboard;

            public HackChecker(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                Debug.Log("hack check");
                if (localboard.NeedTarget == true)
                {
                    return Node.NodeStates.SUCCESS;
                }
                else
                {
                    return Node.NodeStates.SUCCESS;
                }
                
            }
        }
    }

}