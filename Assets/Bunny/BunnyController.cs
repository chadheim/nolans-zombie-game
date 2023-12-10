using Assets.Core;
using System.Collections;
using UnityEngine;

public class BunnyController : MonoBehaviour
{
    public float Speed;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator Apply(WorldState state)
    {
        var bunny = state.bunny;

        spriteRenderer.sortingOrder = 100 - bunny.position.y;

        if (!Mathf.Approximately(bunny.position.x, transform.position.x) || !Mathf.Approximately(bunny.position.y, transform.position.y))
        {
            animator.SetBool("IsRunning", true);

            var target = new Vector3(bunny.position.x, bunny.position.y, 0);
            var toTarget = target - transform.position;

            if (Mathf.Abs(toTarget.x) > 0.1f)
                spriteRenderer.flipX = toTarget.x > 0;

            var step = Speed * Time.deltaTime;
            while (toTarget.magnitude > step)
            {
                transform.Translate(step * toTarget.normalized);
                toTarget = target - transform.position;
                yield return null;
                step = Speed * Time.deltaTime;
            }
            transform.position = target;

            animator.SetBool("IsRunning", false);
        }

        animator.SetBool("IsDead", !bunny.alive);
    }
}
