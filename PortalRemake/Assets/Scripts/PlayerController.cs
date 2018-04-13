using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3.0f;
    float walkSpeed = 3f;
    float runSpeed = 7f;
    float airSpeed = 2f;
    float jumpForce = 230f;
    float groundCheckRayDistance = 0.1f;
    public Vector3 playerVelocity;
    float maxMagnitude = 30f;

    AudioSource _audio;

    RaycastHit groundCheckHit;

    Rigidbody rb;
    public GameObject groundCheck;

    public bool debugGroundCheck = true;

    public float forwardMotion, sidewaysMotion;
    float groundCheckStamp = -100f;
    float groundCheckDelay = 0.3f;

    public enum PlayerState
    {
        Walking,
        Running,
        InAir
    }

    public PlayerState myState = PlayerState.Walking;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // For physics
    private void FixedUpdate()
    {
        playerVelocity = gameObject.GetComponent<Rigidbody>().velocity;

        if (playerVelocity.magnitude > maxMagnitude)
        {
            rb.velocity = Vector3.ClampMagnitude(playerVelocity, maxMagnitude);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && myState != PlayerState.InAir)
        {
            groundCheckStamp = Time.time;
            rb.AddForce(new Vector3(0, jumpForce));
            myState = PlayerState.InAir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If we're not on the ground, make sure we're set to InAir
        if (!Physics.Linecast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), groundCheck.transform.position))
        {
            myState = PlayerState.InAir;
        }

        switch (myState)
        {
            case PlayerState.Walking:
                movementSpeed = walkSpeed;
                CheckMovementInput();
                break;
            case PlayerState.Running:
                movementSpeed = runSpeed;
                CheckMovementInput();
                break;
            case PlayerState.InAir:
                if(rb.velocity.y < -10)
                {
                    if(!_audio.isPlaying)
                    {
                        _audio.Play();
                    }
                }
                else
                {
                    if(_audio.isPlaying)
                    {
                        _audio.Stop();
                    }
                }

                // Wait before checking the ground
                if (Time.time > groundCheckStamp + groundCheckDelay)
                {
                    // If our groundcheck hits something that is NOT a portal
                    if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), groundCheck.transform.position, out groundCheckHit) &&
                        groundCheckHit.transform.gameObject.layer != LayerMask.NameToLayer("Portal"))
                    {
                        if (_audio.isPlaying)
                        {
                            _audio.Stop();
                        }

                        // If the player is holding left shift while landing
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            // Then they're running
                            myState = PlayerState.Running;
                        }
                        else
                        {
                            myState = PlayerState.Walking;
                        }
                    }
                    else
                    {
                        myState = PlayerState.InAir;
                    }
                }

                #region Prevents Movement in Mid-Air (COMMENTED)
                //transform.Translate(Vector3.right * sidewaysMotion * movementSpeed * Time.deltaTime);
                //transform.Translate(Vector3.forward * forwardMotion * movementSpeed * Time.deltaTime);
                #endregion

                #region Allows Movement in Mid-Air
                //movementSpeed = airSpeed;
                CheckMovementInput();
                #endregion
                break;
            default:
                break;
        }

        if(debugGroundCheck)
        {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Vector3.down * groundCheckRayDistance, Color.red);
        }
    }

    void CheckMovementInput()
    {
        #region Character Movement
        if (Input.GetKey(KeyCode.W))
        {
            forwardMotion = 1;
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            forwardMotion = -1;
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }
        else
        {
            forwardMotion = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            sidewaysMotion = -1;
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            sidewaysMotion = 1;
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
        else
        {
            sidewaysMotion = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift) && myState != PlayerState.InAir)
        {
            myState = PlayerState.Running;
        }
        else if (!Input.GetKey(KeyCode.LeftShift) && myState != PlayerState.InAir)
        {
            myState = PlayerState.Walking;
        }
        #endregion
    }

    public float GetForwardMotion()
    {
        return forwardMotion;
    }
}
