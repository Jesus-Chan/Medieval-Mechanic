using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;     // Final rope length
    [SerializeField] private float reelSpeed = 5f;         // How fast rope shortens
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer chain;
    [SerializeField] private float releaseBoost = 2f; // Small extra push on release

    private Vector3 grapplePoint;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;

    private bool isGrappling = false;

    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        joint.enabled = false;
        chain.enabled = false;

        joint.autoConfigureDistance = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero,
                Mathf.Infinity,
                grappleLayer);

            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                grapplePoint.z = 0;

                joint.connectedAnchor = grapplePoint;

                // Start with current distance (prevents snap)
                joint.distance = Vector2.Distance(transform.position, grapplePoint);

                joint.enabled = true;
                isGrappling = true;

                chain.enabled = true;
                chain.SetPosition(0, grapplePoint);
                chain.SetPosition(1, transform.position);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }

        if (chain.enabled)
        {
            chain.SetPosition(1, transform.position);
        }
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

        // Gradually shorten rope until it reaches desired length
        if (joint.distance > grappleLength)
        {
            joint.distance -= reelSpeed * Time.fixedDeltaTime;
        }
    }

    void StopGrapple()
    {
        // Store current velocity direction
        Vector2 currentVelocity = rb.linearVelocity;

        joint.enabled = false;
        chain.enabled = false;
        isGrappling = false;

        // Only boost if player is actually moving
        if (currentVelocity.magnitude > 0.1f)
        {
            Vector2 boostDirection = currentVelocity.normalized;
            rb.AddForce(boostDirection * releaseBoost, ForceMode2D.Impulse);
        }
    }
}