using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

namespace spyBT
{
    public partial class spyBT
    {
        public class SpyGetTarget : IStrategy
        {
            private blackboard localboard;

            public SpyGetTarget(ref blackboard BB)
            {
                localboard = BB;
            }

            public Node.NodeStates Execute()
            {
                Debug.Log("hack target");
                if (localboard.NeedTarget == true)
                {
                    localboard.NeedTarget = false;

                    //set all the goals active
                    for (int j = 0; j < localboard.goals.Count; j++)

                    {

                        localboard.goals[j].SetActive(true);

                    }

                    //choose a random one

                    localboard.chosen = Random.Range(0, localboard.goals.Count);

                    //make sure it is a new target
                    while (true)
                    {
                        if (localboard.chosen == localboard.i)
                            localboard.chosen = Random.Range(0, localboard.goals.Count);
                        else
                        {
                            break;
                        }
                    }

                    localboard.i = localboard.chosen;
                    for (int j = 0; j < localboard.goals.Count; j++)
                        if (j != localboard.chosen)
                            localboard.goals[j].SetActive(false);
                    
                    

                    return Node.NodeStates.SUCCESS;
                }

                return Node.NodeStates.SUCCESS;

              
               

               
        
              

               
            
                
            }

           
           
        }
    }

}