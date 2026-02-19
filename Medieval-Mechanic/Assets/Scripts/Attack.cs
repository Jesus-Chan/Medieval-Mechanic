using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.4f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                nextAttackTime = Time.time + attackCooldown;
                animator.SetTrigger("yesAttack");
            }
        }
    }

    // This will be called from Animation Event
    public void DealDamage()
    {
        Vector3 localPos = attackPoint.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * (spriteRenderer.flipX ? -1 : 1);
        attackPoint.localPosition = localPos;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<CogEnemy>()?.Die();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
