using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    PortalController portalController;
    public bool receiving = false;

    // Once the player steps out of the receiving portal's collider,
    //  this is the amount of time to wait 
    //  before allowing teleportation again
    float receiveStamp = -1000f;
    float receiveDelay = 1f;

    // Use this for initialization
    void Start()
    {
        portalController = GameObject.FindGameObjectWithTag("PortalController").GetComponent<PortalController>();

        Collider[] overlapSphere = Physics.OverlapSphere(transform.position, 0.1f);

        foreach(Collider obj in overlapSphere)
        {
            if(obj.CompareTag("PortalSurface"))
            {
                //objOurPOV = transform.InverseTransformPoint(obj.transform.position);

                //if (objOurPOV.x > 1)
                //{
                //    objOurPOV.x = 0.5f;
                //}

                //if (objOurPOV.y > 1)
                //{
                //    objOurPOV.y = 0.5f;
                //}

                //transform.position = transform.TransformPoint(objOurPOV);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            // If we are not already receiving and we're allowed to receive again
            if (!receiving && Time.time > receiveStamp + receiveDelay)
            {
                // Then teleport the object to the other portal
                portalController.Teleport(this.gameObject, other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(receiving)
        {
            receiveStamp = Time.time;
            receiving = false;
        }
    }

    public void ReceiveTeleport()
    {
        receiving = true;
    }
}
