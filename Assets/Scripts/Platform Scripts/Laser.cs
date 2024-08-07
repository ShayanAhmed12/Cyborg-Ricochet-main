using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserDistance = 8f;
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private UnityEvent OnHitTarget;

    private RaycastHit rayHit;
    private Ray ray;

    private bool state;

    // Start is called before the first frame update
    void Start()
    {
        state = true;
        lineRenderer.positionCount = 2;
        InvokeRepeating("ToggleLaser", 2f, 2f);
    }

    void ToggleLaser()
    {
        if (state)
        {
            state = false;
            lineRenderer.enabled = false;
        }
        else
        {
            state = true;
            lineRenderer.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ray = new(transform.position, transform.forward);

        if (state)
        {
            if (Physics.Raycast(ray, out rayHit, laserDistance, ~ignoreMask))
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, rayHit.point);
                if (rayHit.collider.TryGetComponent(out Target target))
                {
                    target.Hit();
                }
            }
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.forward * laserDistance);
            }
        }
    }

        private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, ray.direction * laserDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rayHit.point, 0.23f);
    }
}
