using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerAim : MonoBehaviour
{
    public float aimZoneRadius;
    public bool isAiming;

    public _ToolObject currentTool;

    [SerializeField] private InputActionReference clickInput;

    private Camera mainCamera;
    private RaycastHit2D hit;

    private SpriteRenderer aimZoneRenderer;

    private void OnEnable()
    {
        if (clickInput != null) clickInput.action.Enable();

        GlobalEvents.OnToolSelected += EquipTool;
    }

    private void OnDisable()
    {
        if (clickInput != null) clickInput.action.Disable();

        GlobalEvents.OnToolSelected -= EquipTool;
    }

    private void Start()
    {
        aimZoneRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isAiming)
        {
            GenericApplyClick();
        }
    }

    private void GenericApplyClick()
    {
        if (clickInput.action.WasReleasedThisFrame())
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                print("Não é válido: O jogador clicou na UI.");
                return;
            }

            Vector2 mouseScreenPosition = Pointer.current.position.ReadValue();
            Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            float clickDistance = Vector2.Distance(mouseWorldPosition, transform.position);

            print(mouseScreenPosition);

            if (currentTool != null && currentTool.useAim && clickDistance > aimZoneRadius)
            {
                print("Não é válido: Fora da mira");
                return;
            }

            hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Asteroid") || hit.collider.CompareTag("Player"))
                {
                    print("Não é válido: Objeto obstruindo");
                    return;
                }
            }
            else
            {
                print("É válido");
            }
        }
    }

    private void AdjustAimZoneSize()
    {
        if (aimZoneRenderer != null)
        {
            float unscaledWidth = aimZoneRenderer.sprite.bounds.size.x;
            float unscaledRadius = unscaledWidth / 2f;
            float scaleFactor = aimZoneRadius / unscaledRadius;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    public void TurnOffAim()
    {
        isAiming = false;
        aimZoneRenderer.enabled = false;
    }

    public void TurnOnAim()
    {
        isAiming = true;
        aimZoneRenderer.enabled = true;
        AdjustAimZoneSize();
    }

    private void EquipTool(_ToolObject tool)
    {
        currentTool = tool;

        if (currentTool != null && currentTool.useAim)
        {
            aimZoneRadius = currentTool.aimRadius;
            AdjustAimZoneSize();
            TurnOnAim();
        }
        else
        {
            TurnOffAim();
        }
    }
}
