using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cuppsats
{
    public class GameManager : MonoBehaviour
    {
        public GameManager gameManager;
        public Collider item;
        RaycastHit hit;
        public Text [] text;
        public bool enter;
        public bool exit;
        string inget = " ";
        public string ItemTag = "Artefact";
        string beenHere = "Iw already examined this.";


        // Use this for initialization
        void Start()
        {
            gameManager = gameObject.GetComponent<GameManager>();
            
            text = gameManager.GetComponentsInChildren<Text>();
        }
        void Update()
        {
            ItemScan();
            Examine();

        }

        private void Examine()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(text[1].text == "E to Examine "+item.gameObject.name)
                {
                    if (item.gameObject.GetComponent<Artefact>().examined)
                    {
                        text[2].text = beenHere;
                        text[1].text = inget;
                        text[0].text = inget;
                    }
                    else
                    {  
                        text[2].text = item.gameObject.GetComponent<Artefact>().Interact();
                        text[1].text = inget;
                        text[0].text = inget;
                        item.gameObject.GetComponent<Artefact>().examined = true;
                    }
                }
            }
        }

        private void ItemScan()
        {
            if(enter)
            { return; }
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.gameObject.tag == ItemTag)
                {
                    //GameObject lookingAt;
                    text[0].text = hit.collider.gameObject.GetComponent<Artefact>().WatchMe();
                    //lookingAt = hit.collider.gameObject;
                    //ItemInspect(lookingAt);

                }
                else
                {
                    string inget = " ";
                    text[0].text = inget;
                }
            }
            else
            {
                string inget = " ";
                text[0].text = inget;
            }
        }

        private void ItemInspect(GameObject theItem)
        {
           

        }

        private void OnTriggerEnter(Collider other)
        {
            enter = true;
            item = other;
            text[1].text = "E to Examine " +item.gameObject.name;
                

        }
        private void OnTriggerExit (Collider other)
        {    
                text[1].text = inget;
                text[2].text = inget;
                enter = false;
        }


    }
}