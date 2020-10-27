using UnityEngine;


namespace spyBT
{
    public partial class spyBT
    {
        public class GetHidingTarget : IStrategy
        {
            private blackboard localboard;

            public GetHidingTarget(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                if (localboard.hidingSpotneeded == true)
                {
                    localboard.hidingSpotneeded = false;
                    Debug.Log("get hiding spot");
                    localboard.spy.GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = 90;
                    //  localboard.spy.GetComponent<NavMeshAgent>().radius = 4;
                    foreach (GameObject guard in GameObject.FindGameObjectsWithTag("guard"))
                    {
                        guard.GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = 0;
                    }
                    localboard.hidingdestination = Random.Range(0, localboard.hidingSpots.Count);
                    return Node.NodeStates.SUCCESS;
                }
                return Node.NodeStates.SUCCESS;

            }
        }
    }

}