using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum GravityBase
    {
        leftWall, rightWall, ceiling, floor
    }

    [SerializeField]
    public AudioClip bounce;

    [SerializeField]
    public AudioClip coinCollect;

    [SerializeField]
    public AudioClip obstaclePass;

    [HideInInspector]
    public bool didBeginMovement = false;

    [SerializeField]
    private KeyCode forwardMoveKey = KeyCode.UpArrow;

    [SerializeField]
    private KeyCode backwardMoveKey = KeyCode.DownArrow;

    [SerializeField]
    private KeyCode leftMoveKey = KeyCode.LeftArrow;

    [SerializeField]
    private KeyCode rightMoveKey = KeyCode.RightArrow;

    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    private KeyCode restartKey = KeyCode.R;

    [SerializeField]
    private float speedChange = 1;

    [SerializeField]
    private float maximumForwardVelocity = 10;

    [SerializeField]
    private float jumpStrength = 10;

    [SerializeField]
    private GameObject cameraAnchor;

    private GravityBase gravityBase = GravityBase.floor;
    private bool isOnGround = true;
    private float backWallPositionZ;

    private Vector3 forwardPositiveMove
    {
        get { return new Vector3(0, 0, speedChange); }
    }

    private Vector3 horizontalPositiveMove
    {
        get
        {
            if (gravityBase == GravityBase.leftWall) return new Vector3(0, -speedChange, 0);
            if (gravityBase == GravityBase.rightWall) return new Vector3(0, speedChange, 0);
            if (gravityBase == GravityBase.ceiling) return new Vector3(-speedChange, 0, 0);
            return new Vector3(speedChange, 0, 0);
        }
    }

    private Vector3 jump
    {
        get { return -jumpStrength * Physics.gravity.normalized; }
    }

    public void setBackWall(float positionZ)
    {
        backWallPositionZ = positionZ;
    }

    public void playSound(AudioClip clip)
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

    private void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        bool didPressActionKey = false;

        if (Input.GetKey(forwardMoveKey))
        {
            rigidbody.velocity += forwardPositiveMove;
            didPressActionKey = true;
        }
        else if (Input.GetKey(backwardMoveKey))
        {
            rigidbody.velocity -= forwardPositiveMove;
            didPressActionKey = true;
        }
        else if (Input.GetKey(leftMoveKey))
        {
            rigidbody.velocity -= horizontalPositiveMove;
            didPressActionKey = true;
        }
        else if (Input.GetKey(rightMoveKey))
        {
            rigidbody.velocity += horizontalPositiveMove;
            didPressActionKey = true;
        }
        else if (Input.GetKey(jumpKey) && isOnGround)
        {
            rigidbody.velocity += jump;
            isOnGround = false;
            didPressActionKey = true;
        }
        else if (Input.GetKey(restartKey))
        {
            GameObject.Find("Gameplay Controller").GetComponent<GameplayController>().restartGame();
        }

        if (!didBeginMovement && didPressActionKey)
        {
            GameObject.Find("Gameplay Controller").GetComponent<GameplayController>().startTime = Time.time;
            didBeginMovement = true;
        }

        if (rigidbody.velocity.z > maximumForwardVelocity)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, maximumForwardVelocity);
        }

        if (transform.position.z < backWallPositionZ)
        {
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, transform.position.y, backWallPositionZ + 0.1f);
            playSound(bounce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        List<string> gravityBaseChangeColliderTags = new List<string> { "Left Wall", "Right Wall", "Ceiling", "Floor" };

        if (gravityBaseChangeColliderTags.Contains(collision.collider.tag))
        {
            isOnGround = true;

            RotateToGravityBase rotationScript = cameraAnchor.GetComponent<RotateToGravityBase>();

            if (collision.collider.CompareTag("Left Wall") && gravityBase != GravityBase.leftWall)
            {
                Physics.gravity = new Vector3(-9.81f, 0, 0);
                rotationScript.RotateTo(GravityBase.leftWall, gravityBase);
                gravityBase = GravityBase.leftWall;
            }
            else if (collision.collider.CompareTag("Right Wall") && gravityBase != GravityBase.rightWall)
            {
                Physics.gravity = new Vector3(9.81f, 0, 0);
                rotationScript.RotateTo(GravityBase.rightWall, gravityBase);
                gravityBase = GravityBase.rightWall;
            }
            else if (collision.collider.CompareTag("Ceiling") && gravityBase != GravityBase.ceiling)
            {
                Physics.gravity = new Vector3(0, 9.81f, 0);
                rotationScript.RotateTo(GravityBase.ceiling, gravityBase);
                gravityBase = GravityBase.ceiling;
            }
            else if (collision.collider.CompareTag("Floor") && gravityBase != GravityBase.floor)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                rotationScript.RotateTo(GravityBase.floor, gravityBase);
                gravityBase = GravityBase.floor;
            }
        }
    }
}