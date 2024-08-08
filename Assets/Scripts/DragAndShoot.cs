
using System;
using UnityEngine;


public class DragAndShoot : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] public float power = 5f;


    private float trajYPos;
    private float trajYPosInitial;
    private int bounceHit;
    private int bounceCount;

    public Vector2 minPower;
    public Vector2 maxPower;
    public GameObject spritePrefab;
    public GameObject instantiatedSprite;

    private Camera _camera;
    private Vector3 _force;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private LineTrail _trail;
    private Trajectory _trajectory;
    [SerializeField] private int steps;

    private Vector3 _tempVec;
    private Vector3 _CharacterCenter;

    private bool lookingRight;

    private bool dragforce;

    public bool isGrounded;
    private bool _isDragging;
    private bool _isOnPlatform;
    private bool _isCollidingwithFloor;
    private bool _isCollidingwithPlatform;
    private RaycastHit _hit;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public Audio _audioManager;

    private void Start()
    {
        bounceHit = 0;
        _camera = Camera.main;
        // _trail = GetComponent<LineTrail>();
        _trajectory = GetComponentInChildren<Trajectory>();
        _animator = GetComponent<Animator>();
        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }


    private void Update()
    {
        Vector3 halfExtents = new Vector3(0.1f, 0.7f, 0.1f);
        _CharacterCenter = transform.position + (0.35f * new Vector3(0, 2.27f, 0));
        float maxDistance = 0.2f;
        Quaternion orientation = Quaternion.identity;

        isGrounded = Physics.BoxCast(_CharacterCenter, halfExtents, Vector3.down, out _hit, orientation, maxDistance);
        if (!isGrounded)
        {
            // _trail.EndLine();
            _trajectory.EndLine02();
            Debug.Log("Not grounded");

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
            _isDragging = true;
            _animator.SetBool("ChargeUp", true);
            trajYPosInitial = transform.position.y;

}
        if (Input.GetMouseButton(0) && _isDragging)
        {
            trajYPos = transform.position.y - trajYPosInitial;
            Debug.Log("traj y pos: "+trajYPos);
            Debug.Log("start pos: " + _startPoint.y);
            if (trajYPos != trajYPosInitial)
            {
                _startPoint.y = _startPoint.y + trajYPos;
            }

            Vector3 currentPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));
            // _trail.RenderLine(currentPoint);

            _force = new Vector3(Mathf.Clamp(_startPoint.x - currentPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(_startPoint.y - currentPoint.y, minPower.y, maxPower.y), 0);


            Vector3[] trajectory = _trajectory.Plot(_CharacterCenter, _force * power, steps); // so that the trajectory starts from the middle of the character
            _trajectory.RenderTrajectory(trajectory);


            if (_force.x < 0)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
                lookingRight = false;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
                lookingRight = true;
            }
        }


        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _tempVec = _trajectory.tempVec;
            bounceCount = _trajectory.bounceCount;
            _isDragging = false;     
            _endPoint = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3f));

            if (instantiatedSprite != null)
            {
                Destroy(instantiatedSprite); // a check if any unwanted sprites still remain 
            }

            _force = new Vector3(Mathf.Clamp(_startPoint.x - _endPoint.x, minPower.x, maxPower.x),
                Mathf.Clamp(_startPoint.y - _endPoint.y, minPower.y, maxPower.y), 0);
             if (_tempVec.y <= _CharacterCenter.y && _isOnPlatform)
            {
                rb.velocity = Vector3.zero;
                _trajectory.EndLine02(); 
            }
             else
             {
                 rb.velocity = _force * power;
                 rb.useGravity = false;
                 _animator.SetBool("ChargeUp", false);
                 _audioManager.PlaySFX(_audioManager.Booster);
                 if (_tempVec.y >= _CharacterCenter.y && !_trajectory.characterAim) { // launched upwards and not towards enemy 
                     _animator.SetBool("StartJumping", true); 
                     instantiatedSprite = Instantiate(spritePrefab, _tempVec - new Vector3(0f,0.2f,0f), Quaternion.identity);
                     _trajectory.EndLine02(); 
                 }  else
                 {
                     _trajectory.EndLine02(); 
                     instantiatedSprite = Instantiate(spritePrefab, _tempVec + new Vector3(0f,0.4f,0), Quaternion.identity); 
                     // if launched vertically downwards, spawn a gravity trigger slightly above to prevent infinite sliding   
                 }
             }
        }
            
    }

    void FixedUpdate()
    {
        if (dragforce)
        {
            ApplyDrag();
        }
    }

    void ApplyDrag()
    {
        Vector3 dragForce = -1.1f * rb.velocity.sqrMagnitude * rb.velocity.normalized;

        rb.AddForce(dragForce);

    }

    private void resetDrag()
    {
        dragforce = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("gravity_trigger"))
        {
            rb.useGravity = true;
            Destroy(instantiatedSprite);
            dragforce = true;
            Invoke("resetDrag", 0.5f);
        }
    }

    private void PlatformCollisionUpdate()
    {
        _isOnPlatform = _isCollidingwithPlatform && !_isCollidingwithFloor;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            _isCollidingwithPlatform = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _isCollidingwithFloor = true;
        }
        PlatformCollisionUpdate();
        if (!other.gameObject.CompareTag("bouncy"))
        {
            _animator.SetBool("StartJumping", false);
            rb.useGravity = true;
            Destroy(instantiatedSprite);
            resetDrag();

        }
        else if (other.gameObject.CompareTag("bouncy"))
        {
            bounceHit++;
            if (lookingRight)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
                lookingRight = false;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
                lookingRight = true;
            }
            if ((bounceCount != 0 && bounceHit >= bounceCount))
            {
                Debug.Log("2");
                bounceHit = 0;
                bounceCount = 0;
                _audioManager.PlaySFX(_audioManager.Bounce);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        int collidedLayer = collision.gameObject.layer;

        if (collidedLayer == LayerMask.NameToLayer("Platform"))
        {
            _isCollidingwithPlatform = false;
        }

        if (collidedLayer == LayerMask.NameToLayer("Floor"))
        {
            _isCollidingwithFloor = false;
        }
        PlatformCollisionUpdate();
    }

}
