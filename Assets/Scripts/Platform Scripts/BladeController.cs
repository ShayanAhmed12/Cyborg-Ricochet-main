using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeController : MonoBehaviour
{
    public Transform PosA, PosB;
    public float Speed;
    private Rigidbody rb;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = PosB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, PosA.position) < 0.1f) targetPos = PosB.position;

        if (Vector3.Distance(transform.position, PosB.position) < 0.1f) targetPos = PosA.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ball"))
        {
            Debug.Log("Game over!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(PosA.position, PosB.position);
    }
    public float rotationSpeed;



}
