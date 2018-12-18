using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cuppsats
{
    public class Artefact : MonoBehaviour
    {

        public GameObject player;
        [SerializeField]
        public string itemName, itemDescription;
        public bool examined = false;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public string WatchMe()
        {
            return itemName;

        }

        public string Interact()
        {

            return itemDescription;
        }

    }
}

