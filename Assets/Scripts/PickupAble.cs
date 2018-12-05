using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

namespace Cuppsats
{
    public class PickupAble : MonoBehaviour
    {
        [SerializeField]
        private int pickUpValue;


        void Start ()
        {
            pickUpValue = 5;

        }
        
        public void PickMeUp(GameManager player)
        {
            player.Score += pickUpValue;
            Destroy(gameObject);
        }

        private void OnTriggerEnter (Collider player)
        {

            if (Input.GetKeyDown(KeyCode.E))
                PickMeUp(player.GetComponentInParent<GameManager>());
            
        }
        
    }
}
