using UnityEngine;

namespace spyBT
{
    public partial class spyBT
    {
        public class Flee : IStrategy
        {
            private blackboard localboard;

            public Flee(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                
                Debug.Log(localboard.hidingdestination);
                Debug.Log("Flee");


                localboard.spy.SetDestination(localboard.hidingSpots[localboard.hidingdestination].transform
                    .position);

               
                if (localboard.spy.remainingDistance <= localboard.spy.stoppingDistance)
                {
                    if (!localboard.spy.hasPath || localboard.spy.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log(1);

                        return Node.NodeStates.RUNNING; 
                        

                    }
                    else
                    {
                       

                        
                        Debug.Log(2);

                        return Node.NodeStates.RUNNING;
                       

                    }


                }
                else
                {
                    Debug.Log(3);
                    
                    Debug.Log("reached destination");

                }
                Debug.Log(4);

                return Node.NodeStates.RUNNING;
            }

        }
    }

}