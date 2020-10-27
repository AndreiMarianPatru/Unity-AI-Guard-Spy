using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;



namespace spyBT
{
    public partial class spyBT : MonoBehaviour
    {
        private Vector3 position;

        [System.Serializable]
        public class blackboard
        {
            public NavMeshAgent spy;
            public int i;//used for keeping track of old goal's index when selecting a new one
            public List<GameObject> hidingSpots=new List<GameObject>();
            public int ListPosition;
            public List<GameObject> goals;
            public bool ReadyToHack;
            public bool Targeted;
            public bool NeedTarget;
            public int chosen;//used for storing the new goal's index
            public int hidingdestination; //used for storing chosen hiding spot index
            public bool hidingSpotneeded;
            public bool hackInProgress;
            public bool GuardNearby;

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
        private Selector root;

        private Sequence hackSequence;
        private Sequence fleeSequence;

        private ActionNode HackCheckerAct;
        private ActionNode SpyGetTargetAct;
        private ActionNode SpyStrategyAct;

        private ActionNode CheckIfChasedAct;
        private ActionNode GetHidingTargetAct;
        private ActionNode FleeAct;
        void Start()
        {
            
            for (int i = 0; i <= 5; i++)
            {
                MyBlackboard.goals.Add(GameObject.Find("goal"+i));

            }

            //just to be sure everything is as intended
            MyBlackboard.ReadyToHack = true;
            MyBlackboard.NeedTarget = true;
            MyBlackboard.hidingdestination = -1; //if hidingdestination <0 that means it wasn't set up(this is the default value that needs to be changed
            MyBlackboard.hackInProgress = false;
            

            root= new Selector();

            hackSequence=new Sequence();
            fleeSequence=new Sequence(); 
            
            root.addChildern(fleeSequence);
            root.addChildern(hackSequence);

            CheckIfChasedAct=new ActionNode(new CheckIfChased(ref MyBlackboard));
            GetHidingTargetAct=new ActionNode(new GetHidingTarget(ref MyBlackboard));
            FleeAct=new ActionNode(new Flee(ref MyBlackboard));

            HackCheckerAct=new ActionNode(new HackChecker(ref MyBlackboard));
            SpyGetTargetAct=new ActionNode(new SpyGetTarget(ref MyBlackboard));
            SpyStrategyAct=new ActionNode(new SpyStrategy(ref MyBlackboard));

           fleeSequence.addChildern(CheckIfChasedAct);
           fleeSequence.addChildern(GetHidingTargetAct);
           fleeSequence.addChildern(FleeAct);




            hackSequence.addChildern(HackCheckerAct);
            hackSequence.addChildern(SpyGetTargetAct);
            hackSequence.addChildern(SpyStrategyAct);






        }

        // Update is called once per frame
        void Update()
        {
            root.Evaluate();
          

            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "goal")
            {
                Debug.Log("after?");
                position = MyBlackboard.spy.gameObject.transform.position;
                MyBlackboard.hackInProgress = true;
                StartCoroutine(EndHack());
            }

            if (other.tag == "hidingSpot")
            {
                Debug.Log("now hiding");
                StartCoroutine(EndHiding());

            }


        }

        private void OnTriggerExit(Collider other)//in case the spy flees from guard
        {
            if (other.tag == "goal")
            {
                MyBlackboard.hackInProgress = false;

            }

        }

        private IEnumerator EndHack()
        {
            yield return new WaitForSeconds(5);
            if (MyBlackboard.spy.gameObject.transform.position.x == MyBlackboard.spy.destination.x &&
                MyBlackboard.spy.gameObject.transform.position.z == MyBlackboard.spy.destination.z)
            {
                Debug.Log("mission passed respect+");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
               
            }
            

        }

        private IEnumerator EndHiding()
        {
            yield return new WaitForSeconds(3);
            MyBlackboard.Targeted = false;
            MyBlackboard.hidingdestination = -1;//set the value back to default
            MyBlackboard.NeedTarget = true;
            

        }

    }

}

