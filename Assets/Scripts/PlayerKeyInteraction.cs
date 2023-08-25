/* This is my complete random logic you can change if you want to.
 I just made this for the sample scene, my key asset in this package is 
 the shader and not the lock and key logic*/

using System;
using System.Collections.Generic;
using UnityEngine;
namespace FandP_POC
{
    public class PlayerKeyInteraction : MonoBehaviour
    {
        [Header("References")]
        public Camera playerCam;
        public Transform keysAttractionPoint;
        public List<DoorColor> doors;
        [Space]
        [Header("Player Instructions")]
        public float maxDistanceOfInteraction = 20f;
        public KeyCode attractionKey = KeyCode.E;
        public float attractionSpeed = 1.2f;
        [Header("Current Key, Assigns in RUNTIME")]
        public GameObject SelectedKey;
        public GameObject SelectdDoor;
        [Space]
        public GameObject keysDefaultParent;
        //privateVariables
        RaycastHit hit;
        bool isKeyInHand = false;
        private void Start()
        {
            SelectedKey = null;
        }
        private void Update()
        {
            if (Input.GetKeyDown(attractionKey))
            {
                if (!isKeyInHand)
                {
                    PickKey();
                }
                else
                {
                    ReleaseKey();
                }
            }

        }

        private void ReleaseKey()
        {
            if (SelectedKey)
            {
                SelectedKey.transform.SetParent(keysDefaultParent.transform);
                SelectedKey = null;
                SelectdDoor = null;
            }

            isKeyInHand = false;
        }

        private void PickKey()
        {
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxDistanceOfInteraction))
            {
                if (hit.transform.gameObject.GetComponent<KeyColor>())
                {
                    //_selectedkey will be 'null' on the first key attraction 
                    if (SelectedKey != null)
                    {
                        SelectedKey.transform.SetParent(keysDefaultParent.transform);
                    }

                    SelectedKey = hit.transform.gameObject.GetComponent<KeyColor>().gameObject;

                    //Get Correct Door Game Object
                    foreach (DoorColor door in doors)
                    {
                        door.GetComponent<MeshCollider>().enabled = true;
                        if (door.doorColor.ToString() == SelectedKey.GetComponent<KeyColor>().selectedKeyColor.ToString())
                        {
                            SelectdDoor = door.gameObject;
                            SelectdDoor.GetComponent<MeshCollider>().enabled = false;
                        }
                    }

                    isKeyInHand = true;
                }
            }
        }
        private void FixedUpdate()
        {
            if (SelectedKey != null)
            {
                //move with rigidbody
                //_selectedKey.GetComponent<Rigidbody>().AddForce((keysAttractionPoint.position - _selectedKey.transform.position) * attractionSpeed * Time.smoothDeltaTime);

                //move with transform
                SelectedKey.transform.position = Vector3.MoveTowards(SelectedKey.transform.position, keysAttractionPoint.position, maxDistanceOfInteraction * Time.deltaTime);

                if (SelectedKey != null && SelectedKey.transform.parent.name == keysDefaultParent.name)
                {
                    if (SelectedKey.transform.position == keysAttractionPoint.position)
                    {
                        SelectedKey.transform.SetParent(keysAttractionPoint);
                    }
                }
            }
        }
    }
}
