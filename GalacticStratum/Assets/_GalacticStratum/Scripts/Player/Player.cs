using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float secondsToReachMaxSpeed;

    private Vector2 movement;
    private Vector2 currentVelocity;

    [SerializeField] private InputActionReference move;

    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleActions();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleActions()
    {
        Vector2 moveValue = move.action.ReadValue<Vector2>().normalized * maxSpeed;
        movement = Vector2.SmoothDamp(movement, moveValue, ref currentVelocity, (maxSpeed/secondsToReachMaxSpeed));

        if (Vector2.Distance(movement, moveValue) <= 1.0f)
        {
            movement = moveValue;
        }

        if (movement == Vector2.zero)
        {
            currentVelocity = Vector2.zero;
        }
    }

    private void ApplyMovement()
    {
        rigidBody.linearVelocity = movement;
    }
}
