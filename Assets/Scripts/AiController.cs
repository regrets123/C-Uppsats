using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Cuppsats
{
    public class AiController : MonoBehaviour
    {

        public Transform player;
        private Transform currentWaypoint;
        public string waypointTag;
        public GameObject[] wayPoints;
        public int triggerDistance = 20;
        public int triggerSprint = 10;
        public bool idle = true;
        public float runspeed = 6;
        public float walkspeed = 3;
        Vector3 direction;
        public Vector3 leadDirection;
        AiHead head;
        public int waypointCounter = 0  ;
        public List<GameObject> sortedWaypoints;
        float timetoRest = 10;
        float timetoSleep = 5;
        public float rotationDrag = 10;
        public bool rdyToRun = false;
        public float getRdyTime = 0.5f;
 
        RaycastHit hit;
        public Animator animator;


        // Use this for initialization
        void Awake()
        {   
            wayPoints = (GameObject.FindGameObjectsWithTag(waypointTag));
        }

        void Start()
        {
            animator = GetComponentInParent<Animator>();
            head = GetComponentInChildren<AiHead>();

            ArrangeWaypoints();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (idle)
            {
                rdyToRun = false;
                animator.SetTrigger("standingTrigger");
                animator.ResetTrigger("walkingTrigger");
                WaitingForPlayer();
                if (timetoRest > 0 )
                timetoRest -= Time.deltaTime;
                else if(timetoRest < 0)
                {
                    animator.SetTrigger("sittingTrigger");
                    animator.ResetTrigger("standingTrigger");
                    if (timetoSleep > 0)
                        timetoSleep -= Time.deltaTime;
                    else if (timetoSleep < 0)
                        animator.SetTrigger("sleepingTrigger");
                        animator.ResetTrigger("SittingTrigger");
                }
                
            }
            else
            {
                timetoRest = 10;
                if(rdyToRun)
                {
                    LeadPlayer();
                }
                else
                StartCoroutine("GetUp");
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
                    //Debug.Log("Calculated distance to waypoint " +tempList[j].name +" is " +distance);
                    if ( distance > compare)
                    {
                        distance = compare;
                    }
                }
                for (int k = 0; k < tempList.Count; k++)
                {
                    if (distance == Vector3.Distance(tempList[k].transform.position, transform.position))
                    {
                        closestWaypoint = tempList[k];

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
                rdyToRun = false;
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
                animator.ResetTrigger("standingTrigger");
                animator.ResetTrigger("sittingTrigger");
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

            }
        }

        IEnumerator GetUp()
        {
            yield return new WaitForSeconds(getRdyTime);
            rdyToRun = true;
        }

        public void FollowWaypoint(float speed)
        {
            leadDirection = currentWaypoint.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(leadDirection, transform.up), rotationDrag);
        }

        public void OnTriggerEnter (Collider coll)
        {
            
                Debug.Log("TRIGGERD");
                if (waypointCounter > wayPoints.Length - 1)
                {
 
                }
                else
                {
                    waypointCounter += 1;
                    currentWaypoint = sortedWaypoints[waypointCounter].transform;
                 }
                if(coll.gameObject.tag == waypointTag)
                    coll.gameObject.SetActive(false);            
            
           
        }


    }
}

