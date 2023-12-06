using Assets.Core;
using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float Speed;

    public int Id { get; set; }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator Apply(WorldState state)
    {
        var zombie = state.zombies.Find(z => z.id == Id);

        if (Mathf.Approximately(zombie.position.x, transform.position.x) && Mathf.Approximately(zombie.position.y, transform.position.y))
            yield break;

        animator.SetBool("IsWalking", true);

        var target = new Vector3(zombie.position.x, zombie.position.y, 0);
        var toTarget = target - transform.position;

        if (Mathf.Abs(toTarget.x) > 0.1f)
            spriteRenderer.flipX = toTarget.x < 0;

        var step = Speed * Time.deltaTime;
        while (toTarget.magnitude > step)
        {
            transform.Translate(step * toTarget.normalized);
            toTarget = target - transform.position;
            yield return null;
            step = Speed * Time.deltaTime;
        }
        transform.position = target;

        animator.SetBool("IsWalking", false);

        if (zombie.position == state.bunny.position)
            animator.SetTrigger("Attack");

        animator.SetBool("IsDead", !zombie.alive);
    }
}
