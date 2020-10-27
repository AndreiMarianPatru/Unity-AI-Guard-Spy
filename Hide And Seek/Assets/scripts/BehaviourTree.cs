using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;


namespace BT
{
    public partial class BehaviourTree : MonoBehaviour
    {
        [System.Serializable]
        public class blackboard
        {
            public NavMeshAgent guard;
            private int i;
            public GameObject tempoobj;
            public int ListPosition;
            public List<GameObject> CheckpointsList;
            public bool ReadyToPatrol;
            public bool VisibleEnemy;
            public bool NeedTarget;
            public Vector3 SpyLastPosition;
            public bool hackInProgress;
            public Vector3 objectHacked;




        }
        public blackboard MyBlackboard;




        public abstract class Node
        {
            public enum NodeStates
            {
                SUCCESS,
                FAILURE,
                RUNNING,
            };

            /* Delegate that returns the state of the node.*/
            public delegate NodeStates NodeReturn();

            /* The current state of the node */
            protected NodeStates currentState;

            public NodeStates nodeState
            {
                
                get { return currentState; }
            }

            /* The constructor for the node */
            public Node() {}

            /* Implementing classes use this method to evaluate the desired set of conditions */
            public abstract NodeStates Evaluate();

        }
        

        public interface IStrategy
        {
            
            Node.NodeStates Execute();
        }


        public class Selector : Node
        {
            /** Children nodes that belong to this sequence */
            private List<Node> m_nodes = new List<Node>();
            public void addChildern(Node newnode)
            {
                m_nodes.Add(newnode);
            }
          
            public Selector()
            {
            }

            /* If any of the children reports a success, the selector will 
                * immediately report a success upwards. If all children fail, 
                * it will report a failure instead.*/
            public override NodeStates Evaluate()
            {
                
                foreach (Node node in m_nodes)
                {
                    switch (node.Evaluate())
                    {
                        case NodeStates.FAILURE:
                            continue;
                        case NodeStates.SUCCESS:
                            currentState = NodeStates.SUCCESS;
                            return currentState;
                        case NodeStates.RUNNING:
                            currentState = NodeStates.RUNNING;
                            return currentState;
                        default:
                            continue;
                    }
                }
                currentState = NodeStates.FAILURE;
                return currentState;
            }
        }

        public class Sequence : Node
        {
            /** Children nodes that belong to this sequence */
            private List<Node> m_nodes = new List<Node>();
            
            public void addChildern(Node newnode)
            {
                m_nodes.Add(newnode);
            }
            
            public Sequence()
            {
                
            }

            /* If any child node returns a failure, the entire node fails. Whence all  
             * nodes return a success, the node reports a success. */
            public override NodeStates Evaluate()
            {


                bool anyChildRunning = false;

                foreach (Node node in m_nodes)
                {
                    switch (node.Evaluate())
                    {
                        case NodeStates.FAILURE:
                            currentState = NodeStates.FAILURE;
                            return currentState;
                        case NodeStates.SUCCESS:
                            continue;
                        case NodeStates.RUNNING:
                            anyChildRunning = true;
                            continue;
                        default:
                            currentState = NodeStates.SUCCESS;
                            return currentState;
                    }
                }
                currentState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
                return currentState;




            }
        }

        public class ActionNode : Node
        {
            private IStrategy _strategy;

            public ActionNode(IStrategy strategy)
            {
                _strategy = strategy;
            }

            /* Evaluates the node using the passed in delegate and  
             * reports the resulting state as appropriate */
            public override NodeStates Evaluate()
            {
                var nodeState = _strategy.Execute();
                switch (nodeState)
                {
                    case NodeStates.SUCCESS:
                        currentState = NodeStates.SUCCESS;
                       

                        return currentState;
                    case NodeStates.FAILURE:
                        currentState = NodeStates.FAILURE;
                   

                        return currentState;
                    case NodeStates.RUNNING:
                        currentState = NodeStates.RUNNING;
                      

                        return currentState;
                    default:
                        currentState = NodeStates.FAILURE;
                        

                        return currentState;
                }
            }
        }

        Selector root;

        Sequence PatrollingSeq; 
        Sequence HackResponseSeq;
        Sequence ChaseSeq;

        ActionNode SightCheckAct;
        ActionNode ChaseAct;

        ActionNode HackCheckAct;
        ActionNode HackInvestigateAct;

        ActionNode PatrolCheckAct;
        ActionNode PatrolGetTargetAct;
        ActionNode PatrollingAct;
        
        
        void Start()
        {
            for (int i = 1; i <= 16; i++)
            {
                MyBlackboard.CheckpointsList.Add(GameObject.Find("cpGroundPoint"+i));

            }

            int Rand = new Random().Next(0, MyBlackboard.CheckpointsList.Count);
            gameObject.transform.SetPositionAndRotation(MyBlackboard.CheckpointsList[Rand].transform.position,Quaternion.identity);
            
            
            MyBlackboard.ReadyToPatrol = true;
            MyBlackboard.NeedTarget = true;

            //layer 1
            root = new Selector();

            PatrollingSeq = new Sequence();
            HackResponseSeq=new Sequence();
            ChaseSeq = new Sequence();

            root.addChildern(ChaseSeq);
            root.addChildern(HackResponseSeq);
            root.addChildern(PatrollingSeq);

            //layer 2
            PatrolCheckAct = new ActionNode(new PatrolChecker(ref MyBlackboard));
            PatrolGetTargetAct = new ActionNode(new PatrolGetTarget(ref MyBlackboard));
            PatrollingAct = new ActionNode(new PatrolStrategy(ref MyBlackboard));
                     
            SightCheckAct = new ActionNode(new DetectionChecker(ref MyBlackboard));
            ChaseAct=new ActionNode(new ChaseStrategy(ref MyBlackboard));

            HackCheckAct=new ActionNode(new HackCheck(ref MyBlackboard));
            HackInvestigateAct=new ActionNode(new HackInvestigate(ref MyBlackboard));
            
            ChaseSeq.addChildern(SightCheckAct);
            ChaseSeq.addChildern(ChaseAct);

            HackResponseSeq.addChildern(HackCheckAct);
            HackResponseSeq.addChildern(HackInvestigateAct);

            PatrollingSeq.addChildern(PatrolCheckAct);
            PatrollingSeq.addChildern(PatrolGetTargetAct);
            PatrollingSeq.addChildern(PatrollingAct);


            


            


        }

        // Update is called once per frame
        void Update()
        {
            root.Evaluate();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "guard")//in case 2 guards get stucked because of each other we will set a random target to each other
            {
                Debug.Log("guard nearby");
                int rand = UnityEngine.Random.Range(0, MyBlackboard.CheckpointsList.Count-1);
                MyBlackboard.ReadyToPatrol = false;
                MyBlackboard.guard.SetDestination(MyBlackboard.CheckpointsList[rand].transform.position);
              
            }

           
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "guard")//after they are not stucked anymore they can go back to patrolling
            {
                
                MyBlackboard.ReadyToPatrol = true;
                
            }
        }
    }

}

