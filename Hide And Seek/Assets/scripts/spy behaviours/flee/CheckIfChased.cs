using UnityEngine;

namespace spyBT
{
    public partial class spyBT
    {
        public class CheckIfChased : IStrategy
        {
            private blackboard localboard;

            public CheckIfChased(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                if (localboard.hackInProgress && localboard.GuardNearby)
                {
                    Debug.Log("now targeted");
                    localboard.Targeted = true;
                    localboard.ReadyToHack = false;
                }
                Debug.Log("chased check");
                if (localboard.Targeted == true)
                {
                    
                    if (localboard.hidingdestination < 0)
                    {
                        localboard.hidingSpotneeded = true;
                    }
                    localboard.spy.ResetPath();

                    return Node.NodeStates.SUCCESS;
                }
                else
                {
                    
                    return Node.NodeStates.FAILURE;
                }
                
            }
        }
    }

}