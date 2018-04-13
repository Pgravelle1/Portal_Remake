using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    GameObject bluePortalProjectilePrefab, orangePortalProjectilePrefab;

    AudioSource audioSource;

    [SerializeField]
    AudioClip portalSuccess = new AudioClip(), portalFail = new AudioClip();

    [SerializeField]
    ParticleSystem blueShotParticles, orangeShotParticles;

    // Create a ray that will shoot forward from our gunEnd
    Ray cameraRay;
    RaycastHit hit;

    PlayerCarryScript carryScript;

    public bool debugCameraRay = true;

    PortalController controller;

    enum PortalType
    {
        Blue,
        Orange
    }

    // Use this for initialization
    void Start()
    {
        bluePortalProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/BluePortalProjectile");
        orangePortalProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/OrangePortalProjectile");

        carryScript = GetComponentInParent<PlayerCarryScript>();
        controller = GameObject.FindGameObjectWithTag("PortalController").GetComponent<PortalController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            // Projectile Look at hit.point and then move forward

            // If we left click
            if (Input.GetMouseButtonDown(0) && !carryScript.pickingUp)
            {
                // Shoot a blue portal
                Shoot(PortalType.Blue);
            }
            // If we right click
            else if (Input.GetMouseButtonDown(1) && !carryScript.pickingUp)
            {
                // Shoot a orange portal
                Shoot(PortalType.Orange);
            }
        }
    }

    private void Shoot(PortalType portalType)
    {
        // If we want to instantiate a blue portal, then set it to bluePortalProjectilePrefab
        // If we want to instantiate a orange portal, then set it to orangePortalProjectilePrefab
        GameObject projectileToInstantiate = portalType == PortalType.Blue ? bluePortalProjectilePrefab : orangePortalProjectilePrefab;
        int mask = 1 << 10;
        mask = ~mask;

        // Create a ray that will cast forward from the center of our camera
        cameraRay = new Ray(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), transform.forward);

        // If we hit something with the camera ray
        if (Physics.Raycast(cameraRay, out hit, 9999f, mask))
        {
            // If we shot at a portal surface
            if(hit.transform.tag == "PortalSurface")
            {
                // Check what's around this raycast
                Collider[] overlapBox = Physics.OverlapBox(hit.point, new Vector3(0.8f, 1.2f, 1));

                // 
                bool isValid = true;

                foreach (Collider obj in overlapBox)
                {
                    if (obj.transform.CompareTag("OrangePortal") && portalType == PortalType.Blue ||
                        obj.transform.CompareTag("BluePortal") && portalType == PortalType.Orange ||
                        obj.transform.CompareTag("PortalSurfaceEdge"))
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    audioSource.clip = portalSuccess;
                    audioSource.Play();

                    if (portalType == PortalType.Blue)
                    {
                        blueShotParticles.Play();
                        controller.CreateBluePortal(hit.point, Quaternion.LookRotation(hit.normal.normalized));
                    }
                    else if (portalType == PortalType.Orange)
                    {
                        orangeShotParticles.Play();
                        controller.CreateOrangePortal(hit.point, Quaternion.LookRotation(hit.normal.normalized));
                    }
                }
                else
                {
                    // Play fail sound
                    audioSource.clip = portalFail;
                    audioSource.Play();
                }

                //// Create the portal projectile we requested
                //Instantiate(projectileToInstantiate, gunEnd.transform.position, Quaternion.LookRotation(hit.point - gunEnd.transform.position));
            }
            else
            {
                audioSource.clip = portalFail;
                audioSource.Play();
            }
        }

    }

    private void OnDrawGizmos()
    {
        if(debugCameraRay)
        {
            Debug.DrawRay(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)), transform.forward * 100f, Color.green);
        }
    }
}
