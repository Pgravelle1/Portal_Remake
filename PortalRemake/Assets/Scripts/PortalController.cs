using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    GameObject bluePortalPrefab, orangePortalPrefab;

    GameObject bluePortal, orangePortal, player;

    List<GameObject> allBluePortals, allOrangePortals;

    PlayerCarryScript carryScript;

    Vector3 playerVelocity;

    AudioSource _audio;


    // Use this for initialization
    float halfPlayerHeight;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _audio = GetComponent<AudioSource>();
        carryScript = player.GetComponentInChildren<PlayerCarryScript>();
        halfPlayerHeight = player.GetComponent<CapsuleCollider>().bounds.extents.y;
        bluePortalPrefab = Resources.Load<GameObject>("Prefabs/Portals/BluePortal");
        orangePortalPrefab = Resources.Load<GameObject>("Prefabs/Portals/OrangePortal");

        allBluePortals = GameObject.FindGameObjectsWithTag("BluePortal").ToList<GameObject>();
        allOrangePortals = GameObject.FindGameObjectsWithTag("OrangePortal").ToList<GameObject>();

        if(allBluePortals.Count > 0)
        {
            bluePortal = allBluePortals[0];
        }

        if(allOrangePortals.Count > 0)
        {
            orangePortal = allOrangePortals[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Time.timeScale = 0.1f;
        // While we have more than 1 orange portal
        while(allOrangePortals.Count > 1)
        {
            // Remove the oldest one
            Destroy(allOrangePortals[0]);
            allOrangePortals.RemoveAt(0);
        }

        // While we have more than 1 blue portal
        while (allBluePortals.Count > 1)
        {
            // Remove the oldest one
            Destroy(allBluePortals[0]);
            allBluePortals.RemoveAt(0);
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            if (allBluePortals.Count > 0 || allOrangePortals.Count > 0)
            {
                _audio.Play();
                // Close all portals
                if (bluePortal != null)
                {
                    Destroy(bluePortal);
                    allBluePortals.Clear();
                }

                if (orangePortal != null)
                {
                    Destroy(orangePortal);
                    allOrangePortals.Clear();
                }
            }
        }
    }

    public void Teleport(GameObject enteredPortal, GameObject objectTeleported)
    {
        if (objectTeleported.tag == "Player")
        {
            // If the player is carrying something
            if (carryScript.pickingUp)
            {
                // Drop it
                carryScript.Drop();
            }
        }

        switch (enteredPortal.tag)
        {
            // If we entered the blue portal
            case "BluePortal":
                if (orangePortal != null)
                {
                    // Tell the orange portal not to teleport the object when they arrive
                    orangePortal.GetComponent<Portal>().ReceiveTeleport();

                    if(objectTeleported.tag == "Player")
                    {
                        player.transform.position = new Vector3(orangePortal.transform.position.x, orangePortal.transform.position.y - halfPlayerHeight, orangePortal.transform.position.z);
                        
                        // If the player moved right into the portal
                        if (player.GetComponent<PlayerController>().sidewaysMotion > 0)
                        {
                            player.transform.eulerAngles = new Vector3(0, orangePortal.transform.eulerAngles.y - 90, 0);
                        }
                        else if (player.GetComponent<PlayerController>().sidewaysMotion < 0)
                        {
                            player.transform.eulerAngles = new Vector3(0, orangePortal.transform.eulerAngles.y + 90, 0);
                        }
                        // If the player walked forward into the portal
                        else if (player.GetComponent<PlayerController>().GetForwardMotion() > 0)
                        {
                            player.transform.eulerAngles = new Vector3(0, orangePortal.transform.eulerAngles.y, 0);
                        }
                        // If the player backed up into the portal
                        else if (player.GetComponent<PlayerController>().GetForwardMotion() < 0)
                        {
                            player.transform.eulerAngles = new Vector3(0, orangePortal.transform.eulerAngles.y - 180, 0);
                            // Set our position and rotation to that of the orange portal
                        }

                        playerVelocity = player.GetComponent<PlayerController>().playerVelocity;
                        player.GetComponent<Rigidbody>().velocity = playerVelocity.magnitude * orangePortal.transform.forward;

                    }

                }
                break;
            // If we entered the orange portal
            case "OrangePortal":
                if (bluePortal != null)
                {
                    // Tell the blue portal not to teleport the player when they arrive
                    bluePortal.GetComponent<Portal>().ReceiveTeleport();

                    // Set our position and rotation to that of the blue portal
                    player.transform.position = new Vector3(bluePortal.transform.position.x, bluePortal.transform.position.y - halfPlayerHeight, bluePortal.transform.position.z);

                    // If the player moved right into the portal
                    if (player.GetComponent<PlayerController>().sidewaysMotion > 0)
                    {
                        player.transform.eulerAngles = new Vector3(0, bluePortal.transform.eulerAngles.y - 90, 0);
                    }
                    else if (player.GetComponent<PlayerController>().sidewaysMotion < 0)
                    {
                        player.transform.eulerAngles = new Vector3(0, bluePortal.transform.eulerAngles.y + 90, 0);
                    }
                    // If the player walked forward into the portal
                    else if (player.GetComponent<PlayerController>().GetForwardMotion() > 0)
                    {
                        player.transform.eulerAngles = new Vector3(0, bluePortal.transform.eulerAngles.y, 0);
                    }
                    // If the player backed up into the portal
                    else if (player.GetComponent<PlayerController>().GetForwardMotion() < 0)
                    {
                        player.transform.eulerAngles = new Vector3(0, bluePortal.transform.eulerAngles.y - 180, 0);
                        // Set our position and rotation to that of the blue portal
                    }


                    playerVelocity = player.GetComponent<PlayerController>().playerVelocity;
                    player.GetComponent<Rigidbody>().velocity = playerVelocity.magnitude * bluePortal.transform.forward;
                }
                break;
            default:
                break;
        }
        
    }

    //public void SetPortalReference(GameObject portal)
    //{
    //    switch (portal.tag)
    //    {
    //        case "BluePortal":
    //            bluePortal = portal;
    //            break;
    //        case "OrangePortal":
    //            orangePortal = portal;
    //            break;
    //        default:
    //            break;
    //    }
    //}

    public void CreateBluePortal(Vector3 position, Quaternion rotation)
    {
        bluePortal = Instantiate(bluePortalPrefab, position, rotation);
        allBluePortals.Add(bluePortal);
    }
    public void CreateOrangePortal(Vector3 position, Quaternion rotation)
    {
        orangePortal = Instantiate(orangePortalPrefab, position, rotation);
        allOrangePortals.Add(orangePortal);
    }
}
