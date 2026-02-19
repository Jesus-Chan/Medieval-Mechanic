using UnityEngine;

public class Bunny : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float JumpHeight = 250f;
    [SerializeField] private float knockbackForce = 400f;
    [SerializeField] private float upwardForce = 100f;
    [SerializeField] private int damageGiven = 2;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform Feet;
    private Rigidbody2D rig;
    private float RayDistance = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       int jumptime = Random.Range(0, 1000);
        if (jumptime == 0 && CheckIfOnGround() == true)
        {
            Jump();
        }

        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);

       
    }

    private void Jump()
    {
        rig.AddForce(new Vector2(0, JumpHeight));
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovment>().TakeDamage(damageGiven);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(knockbackForce, upwardForce);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovment>().TakeKnockback(-knockbackForce, upwardForce);
            }
        }

    }

    private bool CheckIfOnGround() // kollar om det finns ground under playersn fötter
    {
        RaycastHit2D Hit = Physics2D.Raycast(Feet.position, Vector2.down, RayDistance, WhatIsGround);

        Debug.DrawRay(Feet.position, Vector2.down * RayDistance, Color.yellow, 0.25f);

        if (Hit.collider != null && Hit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
