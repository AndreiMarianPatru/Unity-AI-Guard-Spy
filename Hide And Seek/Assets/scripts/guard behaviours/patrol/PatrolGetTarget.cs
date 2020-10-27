using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
    public partial class BehaviourTree
    {
        public class PatrolGetTarget : IStrategy
        {
            private blackboard localboard;
            private NavMeshAgent guard;
            private int i;
            private GameObject tempoobj;
            private int ListPosition;
            private List<GameObject> CheckpointsList;
            private bool needTarget;

            public PatrolGetTarget(ref blackboard BB)
            {
                localboard = BB;
            }

            float CalculatePathLength(NavMeshPath path)

            {
                Vector3 previousCorner = path.corners[0];
                float lengthSoFar = 0.0F;
                int j = 1;
                while (j < path.corners.Length)
                {
                    Vector3 currentCorner = path.corners[j];
                    lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
                    previousCorner = currentCorner;
                    j++;

                }

                return lengthSoFar;
            }

            public Node.NodeStates Execute()
            {
                guard = localboard.guard;
                tempoobj = localboard.tempoobj;
                ListPosition = localboard.ListPosition;
                CheckpointsList = localboard.CheckpointsList;
                needTarget = localboard.NeedTarget;
               
                Debug.Log("patrol get target");

                if (needTarget == true)
                {
            
                    i = 0;
                    float temp;
                    Vector3 tempPos;

                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(guard.transform.position, CheckpointsList[i].transform.position,
                        NavMesh.AllAreas, path);
                    tempoobj = CheckpointsList[i];
                    float min = CalculatePathLength(path);
                    tempPos = CheckpointsList[i].transform.position;
                    string Name = "";
                    for (int i = 0; i < CheckpointsList.Capacity; i++)
                    {
                        NavMesh.CalculatePath(guard.transform.position, CheckpointsList[i].transform.position,
                            NavMesh.AllAreas, path);
                        temp = CalculatePathLength(path);
                        if (temp < min)
                        {
                            min = temp;
                            tempoobj = CheckpointsList[i];
                            tempPos = CheckpointsList[i].transform.position;
                            localboard.ListPosition = i;
                            Name = CheckpointsList[i].name;

                        }
                    }
                 
                   
                    guard.SetDestination(tempoobj.transform.parent.transform.position);
                  
                    localboard.NeedTarget = false;

                }
                return Node.NodeStates.SUCCESS;
            }
        }
    }

}