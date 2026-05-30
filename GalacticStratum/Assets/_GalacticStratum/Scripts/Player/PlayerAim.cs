using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    public float aimZoneRadius;
    public bool isAiming;

    [SerializeField] private InputActionReference clickInput;

    private Camera mainCamera;
    private RaycastHit2D hit;

    private SpriteRenderer aimZoneRenderer;

    private void Start()
    {
        aimZoneRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        AdjustAimZoneSize();

        if (isAiming)
        {
            GenericApplyClick();
        }
    }

    private void GenericApplyClick()
    {
        if (clickInput.action.WasReleasedThisFrame())
        {
            Vector2 mouseScreenPosition = Pointer.current.position.ReadValue();
            Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            float clickDistance = Vector2.Distance(mouseWorldPosition, transform.position);

            print(mouseScreenPosition);

            if (clickDistance > aimZoneRadius)
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

    public void AdjustAimZoneSize()
    {
        if (aimZoneRenderer != null)
        {
            float unscaledWidth = aimZoneRenderer.sprite.bounds.size.x;
            float unscaledRadius = unscaledWidth / 2f;
            float scaleFactor = aimZoneRadius / unscaledRadius;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }
}
