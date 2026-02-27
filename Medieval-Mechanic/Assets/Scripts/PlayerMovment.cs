using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Testkommentar
public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 1f;
    [SerializeField] private float JumpHeight = 300f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private Transform SpawnPosition;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMP_Text CherryAmount;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject cherrieParticles, dustParticles;
    [SerializeField] private Transform wrench;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private TMP_Text CogCounterText;
    [SerializeField] private int questGoal = 5;

    private float horizontalValue;
    private float RayDistance = 0.25f;
    private bool canMove;
    private int StartingHealth = 4;
    private int Health = 0;
    public int CherriesCollected =0;
    public int CogsKilled = 0;
    private bool InWater;


    private Rigidbody2D rigbod;
    private SpriteRenderer SpriteRend;
    private Animator anim;
    private AudioSource audiosource;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = true;
        Health = StartingHealth;
        CherryAmount.text = "" + CherriesCollected;
        rigbod = GetComponent<Rigidbody2D>();
        SpriteRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        UpdateCogUI();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        
        if(horizontalValue < 0) 
        {
            FlipSprite(true);
        }

        if(horizontalValue > 0) 
        {
            FlipSprite(false);
        }

        if (Input.GetButtonDown("Jump") && (CheckIfOnGround() == true || InWater == true)) 
        {
            Jump();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs( rigbod.linearVelocityX));
        anim.SetFloat("VerticalSpeed", rigbod.linearVelocityY);
        anim.SetBool("IsGrounded", CheckIfOnGround());  

       
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }
        else 
        { 
        rigbod.linearVelocity = new Vector2(horizontalValue * MoveSpeed * Time.deltaTime, rigbod.linearVelocityY);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cherry"))
        {
            Destroy(other.gameObject);
            CherriesCollected++;
            CherryAmount.text = "" + CherriesCollected;
            audiosource.pitch = Random.Range(0.8f, 1.2f);
            audiosource.PlayOneShot(pickupSound,0.5f);
            Instantiate(cherrieParticles, other.transform.position, Quaternion.identity);
        }

        if (other.CompareTag("Melon"))
        {
            RestoreHealth(other.gameObject);
        }

        
    }

    private void OnTriggerStay2D(Collider2D other)// ändrar momment när man är i vatten
    {
        if (other.CompareTag("Water"))
        {
            InWater = true;
            anim.SetBool("Swim", true);
            JumpHeight = 200f;
            GetComponent<Rigidbody2D>().AddForce(-Physics.gravity, (ForceMode2D)ForceMode.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other)// ändrar tillbaka momment när man lämmnar vatten
    {
        if (other.CompareTag("Water"))
        {
            InWater = false;
            anim.SetBool("Swim", false);
            JumpHeight = 400f;
        }
    }


    private void FlipSprite(bool direction) 
    {
        float dir = direction ? -1f : 1f;
        transform.localScale = new Vector3(dir, 1f, 1f);

    }

    private void Jump()
    {
        rigbod.linearVelocity = new Vector2(rigbod.linearVelocity.x, 0);

        rigbod.AddForce(new Vector2(0, JumpHeight));

        int randomValue = Random.Range(0, jumpSounds.Length);
        audiosource.PlayOneShot(jumpSounds[randomValue], 0.5f);
        if (CheckIfOnGround() == true) 
        {
            Instantiate(dustParticles, transform.position, dustParticles.transform.localRotation);
        }
    }

    public void TakeKnockback(float knockbackForce, float upwards)
    {
        canMove = false;
        rigbod.AddForce(new Vector2(knockbackForce, upwards));
        Invoke("canMoveAgain", 0.25f);
    }

    private void canMoveAgain() 
    {
        canMove = true; 
    }
    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;
        HealthBar.value = Health;

        if (Health<= 0)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Health = StartingHealth;
        HealthBar.value = Health;
        transform.position = SpawnPosition.position;
       GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    private void RestoreHealth(GameObject HealthPickup)
    {
        if(Health >= StartingHealth)
        {
            return;
        }
        else
        {
            Health+= 1;
            HealthBar.value = Health;
            Destroy(HealthPickup);
        }
        
    }
    private bool CheckIfOnGround() // kollar om det finns ground under playersn fötter
    {
        RaycastHit2D LeftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, RayDistance, WhatIsGround);
        RaycastHit2D RightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, RayDistance, WhatIsGround);

        //Debug.DrawRay(leftFoot.position, Vector2.down * RayDistance, Color.yellow, 0.25f);
        //Debug.DrawRay(rightFoot.position, Vector2.down * RayDistance, Color.magenta, 0.25f);

        if (LeftHit.collider != null && LeftHit.collider.CompareTag("Ground") || RightHit.collider != null && RightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else 
        { 
            return false;
        }
        
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        SpawnPosition = newSpawnPoint;
    }

    private void UpdateCogUI()
    {
        CogCounterText.text = CogsKilled + " / " + questGoal;
    }
    public void AddCogKill()
    {
        CogsKilled++;
        UpdateCogUI();
    }



}
