﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Cuppsats
{
    public class AiController : MonoBehaviour
    {

        public Transform player;
        private Transform currentWaypoint;
        public GameObject[] wayPoints;
        public int triggerDistance = 20;
        public int triggerSprint = 10;
        public bool idle = true;
        public float runspeed = 10;
        public float walkspeed = 5;
        Vector3 direction;
        public Vector3 leadDirection;
        AiHead head;
        public int waypointCounter = 0  ;
        public List<GameObject> sortedWaypoints;
        public NavMeshAgent navMesh;
        float timetoRest = 30;
        float timetoSleep = 15;

        public Animator animator;

        private bool enter = false;


        // Use this for initialization
        void Awake()
        {
            wayPoints = (GameObject.FindGameObjectsWithTag("Waypoint"));
        }

        void Start()
        {
            animator = GetComponentInParent<Animator>();
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
                if (timetoRest > 0 )
                timetoRest -= Time.deltaTime;
                else if(timetoRest < 0)
                {
                    animator.SetTrigger("sittingTrigger");
                    if (timetoSleep > 0)
                        timetoSleep -= Time.deltaTime;
                    else if (timetoSleep < 0)
                        animator.SetTrigger("sleepingTrigger");
                }
                
            }
            else
            {
                timetoRest = 10;
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
                //Debug.Log("Adding this gameobject to templist " + tempList[i].name);
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
                    //Debug.Log("Calculated distance to waypoint " +tempList[j].name +" is " +distance);
                    if ( distance > compare)
                    {
                        //goes thru the full list and finds the smallest distance

                        distance = compare;
                        
                    }
                    //Debug.Log("Smallest distance is" + distance);
                }
                for (int k = 0; k < tempList.Count; k++)
                {
                    if (distance == Vector3.Distance(tempList[k].transform.position, transform.position))
                    {

                        //Debug.Log("Closest waypoint is" + tempList[k].name +" at "+distance);
                        //finds the waypoint with matching distance, removes it from the list and adds it in smallest first index order to a sorted list, then breaks after moving it. 
                        closestWaypoint = tempList[k];
                        //Debug.Log("Removing this object from templist" +tempList[k].name);
                        sortedWaypoints.Add(closestWaypoint);
                        tempList.RemoveAt(k);
                        break;


                    }

                }
            }

            currentWaypoint = sortedWaypoints[0].transform;
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
                Debug.Log("walking");
                animator.SetTrigger("walkingTrigger");
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
                //head.LookAtPlayer(direction);
                navMesh.isStopped = true;
            }
        }

        public void FollowWaypoint(float speed)
        {
            navMesh.speed = speed;
            //leadDirection = currentWaypoint.position - transform.position;
            navMesh.isStopped = false;
            navMesh.SetDestination(currentWaypoint.position);

            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(leadDirection), 0.1f);


            //transform.Translate(0, 0, 0.05f);
        }

        public void OnTriggerEnter (Collider coll)
        {
            
                enter = true;
                Debug.Log("TRIGGERD");
                if (waypointCounter > wayPoints.Length - 1 || waypointCounter == wayPoints.Length - 1)
                {
                    navMesh.isStopped = true;
                }
                else
                {
                    navMesh.isStopped = false;
                    waypointCounter += 1;
                    currentWaypoint = sortedWaypoints[waypointCounter].transform;
                    Debug.Log("Moving Towards "+currentWaypoint);
                    navMesh.SetDestination(currentWaypoint.position);
                }
                coll.isTrigger = false;
            
            
           
        }

        //public void OnTriggerExit(Collider other)
        //{
        //    Debug.Log("LEAVING");
            
        //        enter = false;
        //    Destroy(other.gameObject);
        //}

    }
}

