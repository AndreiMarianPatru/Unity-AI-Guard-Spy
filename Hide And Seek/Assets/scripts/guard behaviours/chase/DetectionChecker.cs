using UnityEngine;

namespace BT
{
    public partial class BehaviourTree
    {
        public class DetectionChecker : IStrategy
        {
       
            private bool readyToChase;
       
      
             blackboard tembb;
            public DetectionChecker(ref blackboard BB)
            {
                tembb = BB;
            }

            public Node.NodeStates Execute()
            {
                readyToChase = tembb.VisibleEnemy;
                Debug.Log("sightcheck");
                if (readyToChase)
                {
                    Debug.Log("bt sight on");
                    return Node.NodeStates.SUCCESS;
                }
                else
                {
                    Debug.Log("bt sight off");
                    return Node.NodeStates.FAILURE;


                }

            }
        }
    }

}

