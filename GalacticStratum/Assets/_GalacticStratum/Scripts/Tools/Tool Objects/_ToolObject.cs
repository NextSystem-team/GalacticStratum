using UnityEngine;

public abstract class _ToolObject : MonoBehaviour
{
    public abstract bool UseAim { get; }
    public abstract float AimRadius { get; }

    public virtual void OnUse(Vector2 targetPosition, Player player)
    {

    }
}
