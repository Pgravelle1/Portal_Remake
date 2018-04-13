using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CubeDetector : MonoBehaviour
{
    bool detected = false;

    Collider[] overlapBox;

    Vector3 boxExtents = new Vector3(1.5f, 0.7f, 1.5f);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        overlapBox = Physics.OverlapBox(transform.position, boxExtents / 2);

        detected = false;
        foreach(Collider obj in overlapBox)
        {
            if(obj.tag == "WeightedCube")
            {
                detected = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxExtents);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    // If a cube is in our trigger
    //    if(other.tag == "WeightedCube")
    //    {
    //        // We detect something
    //        detected = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.tag == "WeightedCube")
    //    {
    //        // We no longer detect something
    //        detected = false;
    //    }
    //}

    /// <summary>
    /// Returns whether the detector is detecting a cube or not.
    /// </summary>
    /// <returns></returns>
    public bool IsDetecting()
    {
        return detected;
    }
}
