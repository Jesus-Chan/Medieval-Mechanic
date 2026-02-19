using UnityEngine;

public class Attack : MonoBehaviour
{


    [SerializeField] private float range = 1.0f; // The range of the attack
    [SerializeField] private int damage = 1; // The amount of damage this attack will deal
    private float attackCooldown = 0f; // Cooldown timer for the attack

    [SerializeField] private float attackSpeed = 1.0f; // The speed of the attack animation

    public LayerMask enemyLayer; // Layer mask to specify which layers are considered enemies
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Time.time >= attackCooldown)
        {
            if (Input.GetKeyDown("Fire1")) // Check for attack input (mouse 1 or ctrl)
            {
                performAttack();
                attackCooldown = Time.time + attackSpeed; // Set the next time the attack can be performed
            }
        }


    }

    void performAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

}
