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
        public float currentSpeed = 0;
        public float acceleration = 0.5f;
        Vector3 direction;
        public Transform leadDirection;
        AiHead head;
        public int waypointCounter = 0  ;
        public List<GameObject> sortedWaypoints;
        float timetoRest = 10;
        float timetoSleep = 5;
        public float rotationDrag = 10;
        public bool rdyToRun = false;
        public float getRdyTime = 2f;
        
 
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
            //agent = GetComponent<NavMeshAgent>();
            ArrangeWaypoints();
            
        }

        // Update is called once per frame
        void Update()
        {
            if (idle)
            {
                
                currentSpeed = 0;
                
                WaitingForPlayer();
                if (timetoRest > 0 )
                {
                    animator.ResetTrigger("walkingTrigger");    
                    animator.SetTrigger("standingTrigger");
                    timetoRest -= Time.deltaTime;
                }
                else if(timetoRest < 0)
                {
                    rdyToRun = false;
                    animator.SetTrigger("sittingTrigger");
                }
                
            }
            else
            {
                timetoRest = 10;

                if (rdyToRun)
                {
                    animator.SetTrigger("walkingTrigger");
                    animator.ResetTrigger("standingTrigger");
                    animator.ResetTrigger("sittingTrigger");
                    LeadPlayer();
                }
                else
                { 

                StartCoroutine("GetUp");
                        animator.SetTrigger("walkingTrigger");
                        animator.ResetTrigger("standingTrigger");
                        animator.ResetTrigger("sittingTrigger");
                }
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
                
                currentSpeed = 0;
            }
            else
            {
                if (Vector3.Distance(player.position, transform.position) < triggerSprint)
                {
                    currentSpeed = FollowWaypoint(runspeed,currentSpeed,acceleration);
                }
                currentSpeed = FollowWaypoint(walkspeed,currentSpeed,acceleration);
                //Debug.Log("walking");
                
            }

        }

        public void WaitingForPlayer()
        {
            if (Vector3.Distance(player.position, transform.position) < triggerDistance)
            {
                idle = false;  

                //agent.isStopped = true;
            }
            else
            {
                direction = player.position - transform.position;
                


            }
        }

        IEnumerator GetUp()
        {
            yield return new WaitForSeconds(1f);    
            Debug.Log("IM READY NOW!!!");
            rdyToRun = true;
        }

        public float FollowWaypoint(float maxSpeed, float currentSpeed,float acceleration)
        {
            float speed;
            
            speed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration);
                
            currentSpeed = speed;
            //agent.destination = currentWaypoint.transform.position;
            direction = currentWaypoint.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), rotationDrag);

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.transform.position, speed * Time.deltaTime);
            animator.SetFloat("Blend", currentSpeed);

            return currentSpeed;
        }

        public void OnTriggerEnter (Collider coll)
        {
            if(coll.gameObject.tag == waypointTag)
            { 
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
}

