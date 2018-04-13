using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : MonoBehaviour
{
    PortalController controller;

    float projectileSpeed = 30f;

    float delayToDestroy = 15f;
    float instantiateTime = 0f;

    RaycastHit hit;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * projectileSpeed;
        controller = GameObject.FindGameObjectWithTag("PortalController").GetComponent<PortalController>();

        instantiateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > instantiateTime + delayToDestroy)
        {
            Debug.Log("Portal Projectile Lasted Longer than " + delayToDestroy + " seconds and was Destroyed");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            if (collision.transform.CompareTag("PortalSurface"))
            {
                if (transform.CompareTag("BluePortalProjectile"))
                {
                    Collider[] overlapBox = Physics.OverlapBox(transform.position, new Vector3(0.8f, 1.2f, 1));
                    
                    bool isValid = true;

                    foreach (Collider obj in overlapBox)
                    {
                        if (obj.transform.CompareTag("OrangePortal") || obj.transform.CompareTag("PortalSurfaceEdge"))
                        {
                            isValid = false;
                        }
                    }

                    if (isValid)
                    {
                        controller.CreateBluePortal(hit.point, Quaternion.LookRotation(hit.normal.normalized));
                    }
                }
                else if (transform.CompareTag("OrangePortalProjectile"))
                {
                    Collider[] overlapBox = Physics.OverlapBox(transform.position, new Vector3(0.8f, 1.2f, 1));
                    bool isValid = true;
                    foreach(Collider obj in overlapBox)
                    {
                        if(obj.transform.CompareTag("BluePortal") || obj.transform.CompareTag("PortalSurfaceEdge"))
                        {
                            isValid = false;
                        }
                    }

                    if(isValid)
                    {
                        controller.CreateOrangePortal(hit.point, Quaternion.LookRotation(hit.normal.normalized));
                    }
                }
            }
        }
        else
        {
            //EditorApplication.isPaused = true;
            Debug.Log("OnCollisionEnter raycast FAILED on portal projectile");
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Gizmos.DrawWireCube(transform.position, new Vector3(1.6f, 2.4f, 2));
    }
}
