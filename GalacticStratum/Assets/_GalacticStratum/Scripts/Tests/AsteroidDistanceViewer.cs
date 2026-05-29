using UnityEngine;

public class AsteroidDistanceViewer : MonoBehaviour
{
    [SerializeField] private Transform asteroidTransform;
    [SerializeField] private Transform asteroid2Transform;

    private float distance;

    private void Start()
    {
        distance = Vector3.Distance(asteroidTransform.position, asteroid2Transform.position);
    }

    private void Update()
    {
        if (distance != Vector3.Distance(asteroidTransform.position, asteroid2Transform.position))
        {
            distance = Vector3.Distance(asteroidTransform.position, asteroid2Transform.position);
            print($"Distância: {distance}");
        }
    }
}
