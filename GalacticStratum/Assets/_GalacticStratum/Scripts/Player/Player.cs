using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float secondsToReachMaxSpeed;
    [SerializeField] private float secondsToStop;
    [SerializeField] private float rotationSpeed;
    [SerializeField, Range(0f, 1f)] private float driftFactor; //O quanto vai deslizar nas curvas

    private Vector2 movement;
    private Vector2 moveInputValue;

    private float zoomInputValue;

    [Header("Inputs")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference zoom;

    [Header("Camera")]
    public CinemachineCamera mainCamera;
    public Collider2D map;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    private Rigidbody2D rigidBody;
    private CinemachineConfiner2D confiner;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        confiner = mainCamera.GetComponent<CinemachineConfiner2D>();
    }

    private void Update()
    {
        HandleActions();
        ApplyZoom();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    private void HandleActions()
    {
        moveInputValue = move.action.ReadValue<Vector2>();
        zoomInputValue = zoom.action.ReadValue<float>();
    }

    private void FixDrift()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(movement, transform.up); //Coleta a velocidade vertical
        Vector2 rightVelocity = transform.right * Vector2.Dot(movement, transform.right); //Coleta a velocidade horizontal
        movement = forwardVelocity + (rightVelocity * driftFactor); 
        //Reduz a velocidade horizontal, fazendo o player deslizar menos ou mais nas curvas
    }

    private void ApplyMovement()
    {
        FixDrift();

        Vector2 targetVelocity = moveInputValue.y > 0 ? (Vector2)transform.up * maxSpeed : Vector2.zero; //Se o input for positivo, o targetVelocity é para frente

        float timeToReachTargetSpeed = moveInputValue.y == 0 ? secondsToStop : secondsToReachMaxSpeed;

        movement = Vector2.MoveTowards(movement, targetVelocity, (maxSpeed/timeToReachTargetSpeed) * Time.fixedDeltaTime);

        if (Vector2.Distance(movement, targetVelocity) <= 0.5f)
        {
            movement = targetVelocity;
        }

        rigidBody.linearVelocity = movement;
    }

    private void ApplyRotation()
    {
        if (moveInputValue.x != 0)
        {
            float targetAngle = -moveInputValue.x * rotationSpeed * Time.fixedDeltaTime;

            rigidBody.MoveRotation(rigidBody.rotation + targetAngle);
        }
    }

    private void ApplyZoom()
    {
        if (zoomInputValue != 0)
        {
            float zoomMultiplier = zoomInputValue * zoomSpeed;
            float currentZoom = mainCamera.Lens.OrthographicSize;

            mainCamera.Lens.OrthographicSize = Mathf.Clamp(currentZoom - zoomMultiplier, minZoom, maxZoom);

            confiner.InvalidateBoundingShapeCache();
        }
    }
}
