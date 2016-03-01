using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Actor : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private float invMoveTime;

    // Use this for initialization
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        invMoveTime = 1f / moveTime;
    }

    protected bool CanMove(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        try
        {
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, blockingLayer);
        }
        finally
        {
            boxCollider.enabled = true;
        }

        if (hit.transform == null)
        {
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, end, invMoveTime * Time.deltaTime);
            body.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected virtual bool AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = CanMove(xDir, yDir, out hit);

        if (canMove)
        {
            // We can move, so start the movement!
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        // Otherwise, there is something there.
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
        return false;
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;
}
