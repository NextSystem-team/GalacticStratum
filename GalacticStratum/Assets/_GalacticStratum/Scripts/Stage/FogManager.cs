using UnityEngine;
using UnityEngine.Rendering;

public class FogManager : MonoBehaviour
{
    [SerializeField] private RenderTexture temporaryMap;
    [SerializeField] private RenderTexture persistentMap;
    [SerializeField] private Material mapAccumulator;

    void Start()
    {
        ClearMap();
    }

    void LateUpdate()
    {
        Graphics.Blit(temporaryMap, persistentMap, mapAccumulator);
    }

    private void ClearMap()
    {
        RenderTexture.active = persistentMap;
        GL.Clear(false, true, Color.black);
    }

}
