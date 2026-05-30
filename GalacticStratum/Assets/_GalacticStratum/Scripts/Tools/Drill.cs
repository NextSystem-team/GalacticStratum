using UnityEngine;

public class Drill : MonoBehaviour
{
    [Header("Rope Settings")]
    [SerializeField] private LineRenderer rope;
    public Player startRopePoint;
    public float ropeLength;

    [Header("Drill Settings")]
    [SerializeField] private float drillSpeed;
    public Vector2 targetPosition;

    public bool canMove = false;

    private AsteroidData asteroidData;

    private void Update()
    {
        rope.SetPosition(0, startRopePoint.transform.position);
        rope.SetPosition(1, transform.position);

        if (!canMove) return;

        // Move the drill towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, drillSpeed * Time.deltaTime);
        transform.up = transform.position - startRopePoint.transform.position;

        if (Vector2.Distance(rope.GetPosition(0), rope.GetPosition(1)) > ropeLength)
        {
            print("Rope broke!");
            Destroy(gameObject);
        }

        if ((Vector2)transform.position == targetPosition && asteroidData == null)
        {
            print("Drill hit the target position but no asteroid data was found!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            drillSpeed = 0;
            asteroidData = collision.transform.parent.GetComponent<Asteroid>().data;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ropeLength);
    }
}
