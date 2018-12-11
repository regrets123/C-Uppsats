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

        public NavMeshAgent navMesh;



        // Use this for initialization
        void Start()
        {
            wayPoints = (GameObject.FindGameObjectsWithTag("Waypoint"));
            head = GetComponentInChildren<AiHead>();
            currentWaypoint = wayPoints[waypointCounter].transform;
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

