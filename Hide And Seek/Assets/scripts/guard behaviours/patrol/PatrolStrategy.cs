using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace BT
{
    public partial class BehaviourTree
    {



        public class PatrolStrategy :  IStrategy
        {



            private NavMeshAgent guard;
            private int i;
            private GameObject tempoobj;
            private int ListPosition;
            private List<GameObject> CheckpointsList;

            private blackboard localboard;
            
            public PatrolStrategy(ref blackboard BB)
            {
                localboard = BB;
                guard = localboard.guard;
                CheckpointsList = localboard.CheckpointsList;
            }



           
            public Node.NodeStates Execute()
            {
                Debug.Log("patrol act");
                ListPosition = localboard.ListPosition;

             


                if (guard.gameObject.transform.position.x == guard.destination.x &&
                                guard.gameObject.transform.position.z == guard.destination.z)
                {
           
                    if (ListPosition != CheckpointsList.Capacity - 1)
                    {
                    

                        guard.SetDestination(CheckpointsList[ListPosition + 1].transform.position);

                        localboard.ListPosition++;
                    }
                    else
                    {
                     

                        localboard.ListPosition = 0;
                        guard.SetDestination(CheckpointsList[0].transform.position);
                    }


                }




                return Node.NodeStates.RUNNING;
            }
        }

    }

}

