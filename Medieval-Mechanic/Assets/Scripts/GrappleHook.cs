using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength = 5f;
    [SerializeField] private float reelSpeed = 5f;
    [SerializeField] private float grappleShootSpeed = 20f;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer chain;
    [SerializeField] private float releaseBoost = 2f;


    //Audio and Particles
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip releaseSound;

    [SerializeField] private ParticleSystem grappleParticles;
 




    private Vector3 grapplePoint;
    private DistanceJoint2D joint;
    private Rigidbody2D rb;

    private bool isGrappling = false;
    private bool isShooting = false;

    private float shootProgress = 0f;
    private float shootDuration = 1f;

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
        if (Input.GetMouseButtonDown(1) && !isGrappling && !isShooting)
        {
            StartGrapple();
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopGrapple();
        }

        // Shooting rope outward
        if (isShooting)
        {
            shootProgress += grappleShootSpeed * Time.deltaTime;

            Vector2 ropePos = Vector2.Lerp(transform.position, grapplePoint, shootProgress / shootDuration);

            chain.SetPosition(0, transform.position);
            chain.SetPosition(1, ropePos);

            if (shootProgress >= shootDuration)
            {
                FinishGrapple();
            }
        }

        // Normal grappling rope
        if (isGrappling)
        {
            chain.SetPosition(0, grapplePoint);
            chain.SetPosition(1, transform.position);
        }
    }

    void StartGrapple()
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
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(shootSound, 0.3f);  // Play shoot sound

            shootProgress = 0f;

            chain.enabled = true;
            chain.SetPosition(0, transform.position);
            chain.SetPosition(1, transform.position);

            isShooting = true;
            
        }
        

    }

    void FinishGrapple()
    {
        isShooting = false;
        isGrappling = true;

        joint.connectedAnchor = grapplePoint;

        joint.distance = Vector2.Distance(transform.position, grapplePoint) * 0.98f;

        joint.enabled = true;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(hitSound, 0.3f); // hit sound

    

        grappleParticles.transform.position = grapplePoint;
        grappleParticles.Play();


    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

        if (joint.distance > grappleLength)
        {
            joint.distance -= reelSpeed * Time.fixedDeltaTime;
        }
    }

    void StopGrapple()
    {
        Vector2 currentVelocity = rb.linearVelocity;

        if(isGrappling)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);

            audioSource.PlayOneShot(releaseSound, 0.3f); // release sound
        }

        joint.enabled = false;
        chain.enabled = false;

        isGrappling = false;
        isShooting = false;

        

        grappleParticles.Stop();



        if (currentVelocity.magnitude > 0.1f)
        {
            Vector2 boostDirection = currentVelocity.normalized;
            rb.AddForce(boostDirection * releaseBoost, ForceMode2D.Impulse);
        }
    }
}