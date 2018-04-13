using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryScript : MonoBehaviour
{
    GameObject cam, objectCarrying;
    Collider objectCarryingCollider;

    [SerializeField]
    GameObject pickupCollider;

    Ray carryRay;

    float rayDistance = 2f;

    public bool pickingUp = false;

    AudioSource _audio;

    RaycastHit hit;

    Vector3 camCenter;

    int carryLayer;
    int carryMask;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();

        cam = gameObject;


        // Get the layer number for our CanCarry layer
        carryLayer = LayerMask.NameToLayer("CanCarry");

        // Make a mask that ignores everything BUT the carryLayer
        carryMask = 1 << carryLayer;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the center of our viewport (camera view) as a world point
        camCenter = cam.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        // Create a ray that will cast forward from the center of our camera
        carryRay = new Ray(camCenter, transform.forward);

        // If we press E
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If we are not carrying anything
            if (!pickingUp)
            {
                // If our ray hits a object that we can carry
                if (Physics.Raycast(carryRay, out hit, rayDistance, carryMask))
                {
                    Debug.Log("Hit a carry object.");
                    // Carry that object
                    Carry(hit.transform.gameObject);
                }
            }
            // If we are carrying anything
            else
            {
                // Drop it
                Drop();
            }
        }
    }

    void Carry(GameObject _objectCarrying)
    {
        _audio.Play();
        objectCarrying = _objectCarrying;
        objectCarryingCollider = objectCarrying.GetComponent<Collider>();
        gameObject.GetComponent<PlayerCameraController>().downClampMax = 20f;
        gameObject.GetComponent<PlayerCameraController>().upClampMax = 320f;

        // Turn off the collider on the object we are carrying
        objectCarryingCollider.enabled = false;
        // Turn on the collider we're using to pick up 
        pickupCollider.SetActive(true);
        objectCarrying.transform.GetComponent<Rigidbody>().useGravity = false;
        objectCarrying.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        objectCarrying.transform.position = pickupCollider.transform.position;
        objectCarrying.transform.rotation = pickupCollider.transform.rotation;
        objectCarrying.transform.parent = cam.transform;
        pickingUp = true;
    }

    public void Drop()
    {
        if (objectCarrying != null)
        {
            _audio.Stop();
            objectCarrying.transform.parent = null;
            pickupCollider.SetActive(false);
            gameObject.GetComponent<PlayerCameraController>().downClampMax = 90f;
            gameObject.GetComponent<PlayerCameraController>().upClampMax = 270f;
            objectCarryingCollider.enabled = true;
            objectCarrying.transform.GetComponent<Rigidbody>().useGravity = true;
            objectCarrying.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            objectCarrying = null;
            objectCarryingCollider = null;
            pickingUp = false;
        }
        else
        {
            Debug.LogError("PlayerCarryScript says we're carrying an object but objectCarrying is null.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(camCenter, carryRay.direction * rayDistance);
    }
}
