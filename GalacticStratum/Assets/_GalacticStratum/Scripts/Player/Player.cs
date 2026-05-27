using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Vector2 movement;

    [SerializeField] private InputActionReference move;

    private void Update()
    {
        HandleActions();
        ApplyMovement();
    }

    private void HandleActions()
    {
        movement = move.action.ReadValue<Vector2>().normalized * movementSpeed * Time.deltaTime;
    }

    private void ApplyMovement()
    {
        transform.Translate(new Vector3(movement.x, movement.y, 0.0f));
    }
}
