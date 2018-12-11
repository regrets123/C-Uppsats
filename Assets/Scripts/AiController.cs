using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Cuppsats
{
    public class AiController : MonoBehaviour
    {

        public Transform player;
        public Transform currentWaypoint;
        public GameObject[] wayPoints;
        public int triggerDistance = 20;
        public int triggerSprint = 10;
        public bool idle = true;
        public int runspeed = 2;
        public int walkspeed = 1;
        Vector3 direction;
        public Vector3 leadDirection;
        AiHead head;
        public int waypointCounter = 3  ;
        public List<GameObject> sortedWaypoints;
        public NavMeshAgent navMesh;



        // Use this for initialization
        void Start()
        {
            wayPoints = (GameObject.FindGameObjectsWithTag("Waypoint"));
            head = GetComponentInChildren<AiHead>();
            ArrangeWaypoints();
            navMesh = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (idle)
            {
                WaitingForPlayer();
            }
            else
            {
                LeadPlayer();
            }



        }

        public void ArrangeWaypoints()
        {
            List<GameObject> tempList = new List<GameObject>();  // list to use when saving and moving the closest waypoint. 
            GameObject closestWaypoint; //saving the waypoint temporary.


             
            for (int i = 0; i < wayPoints.Length; i++)
            {
                tempList.Add(wayPoints[i]);
                //converts from array to list so we can remove only the closest waypoint.
            }

            for (int l = 0; l < wayPoints.Length; l++)
            {   //everytime we go thru this loop the list will get 1 index smaller until its only 1 element left in list. 
                float compare = 0;
                float distance = Mathf.Infinity; ; //reference point to compare distances.

                if(tempList.Count ==1)
                {
                    sortedWaypoints.Add(tempList[0]);
                    break;
                }

                //int indexHolder = 0;
                for (int j = 0; j < tempList.Count; j++)
                {



                    compare = Vector3.Distance(tempList[j].transform.position, transform.position);
                    if ( distance > compare)
                    {
                        //goes thru the full list and finds the smallest distance
                        distance = compare;
                    }

                }
                for (int k = 0; k < tempList.Count - 1; k++)
                {
                    if (distance == Vector3.Distance(tempList[k].transform.position, transform.position))
                    {
                        //finds the waypoint with matching distance, removes it from the list and adds it in smallest first index order to a sorted list, then breaks after moving it. 
                        closestWaypoint = tempList[k];
                        sortedWaypoints.Add(closestWaypoint);
                        //indexHolder = k;
                        tempList.RemoveAt(k);
                        break;


                    }

                }
                //sortingList.RemoveAt(indexHolder);
            }


        }

        public void LeadPlayer()
        {
            if (Vector3.Distance(player.position, transform.position) > triggerDistance)
            {
                idle = true;
            }
            else
            {
                if (Vector3.Distance(player.position, transform.position) < triggerSprint)
                {
                    FollowWaypoint(runspeed);
                }
                FollowWaypoint(walkspeed);
            }

        }

        public void WaitingForPlayer()
        {
            if (Vector3.Distance(player.position, transform.position) < triggerDistance)
            {
                idle = false;
                
            }
            else
            {
                direction = player.position - transform.position;
                head.LookAtPlayer(direction);
                navMesh.isStopped = true;
            }
        }

        public void FollowWaypoint(int speed)
        {


            //leadDirection = currentWaypoint.position - transform.position;
            navMesh.isStopped = false;
            navMesh.SetDestination(currentWaypoint.position);

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(leadDirection), 0.1f);


            //transform.Translate(0, 0, 0.05f);
        }

        public void OnTriggerEnter (Collider coll)
        {
            waypointCounter -= 1;
            currentWaypoint = wayPoints[waypointCounter].transform;
        }

    }
}

