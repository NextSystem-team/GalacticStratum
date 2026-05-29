using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidVisualData", menuName = "Scriptable Objects/Asteroids/Asteroid Visual Data")]
public class AsteroidVisualData : ScriptableObject
{
    [System.Serializable]
    public struct AsteroidVisual
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite brushSprite;

        public readonly Sprite Sprite => sprite;
        public readonly Sprite BrushSprite => brushSprite;

        public AsteroidVisual(Sprite sprite, Sprite brushSprite)
        {
            this.sprite = sprite;
            this.brushSprite = brushSprite;
        }
    }

    [Header("Asteroid Sprites")]
    public AsteroidVisual[] sprites;

    public AsteroidVisual GetRandomSprite()
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("No sprites in AsteroidVisualData.");
            return new AsteroidVisual(null, null);
        }

        return sprites[Random.Range(0, sprites.Length)];
    }
}
