using Assets.Core;
using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int Id { get; set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public IEnumerator Apply(WorldState state)
    {
        var trap = state.traps.Find(t => t.id == Id);

        spriteRenderer.sortingOrder = 100 - trap.position.y;

        yield break;
    }
}
