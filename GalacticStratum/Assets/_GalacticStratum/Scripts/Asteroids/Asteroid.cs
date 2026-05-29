using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Dados do Asteroide")]
    [SerializeField] private SpriteRenderer brush;
    public AsteroidData data;

    [Header("Banco de dados das sprites de Asteroide")]
    [SerializeField] private AsteroidVisualData spritesDatabase;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        brush.enabled = false;

        AsteroidVisualData.AsteroidVisual visual = spritesDatabase.GetRandomSprite();
        spriteRenderer.sprite = visual.Sprite;
        brush.sprite = visual.BrushSprite;

        transform.localScale = Vector3.one * Random.Range(data.Size.MinSize, data.Size.MaxSize);
    }

    public void RevealAsteroid()
    {
        //Colocar efeitos visuais e sonoros aqui...

        brush.enabled = true;
    }

    public void Explode()
    {
        //Colocar efeitos visuais e sonoros aqui...

        Destroy(gameObject);
    }
}
