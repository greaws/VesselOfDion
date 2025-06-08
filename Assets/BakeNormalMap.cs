using UnityEngine;
using System.IO;

public class SaveRenderTextureToPNG : MonoBehaviour
{
    public RenderTexture renderTexture; // Assign your NormalMapRT here
    public string outputFileName = "BakedNormalMap.png";

    [ContextMenu("Save RenderTexture to PNG")]
    void Save()
    {
        if (renderTexture == null)
        {
            Debug.LogError("No RenderTexture assigned.");
            return;
        }

        // Backup the currently active RenderTexture
        RenderTexture currentRT = RenderTexture.active;

        // Set the active RenderTexture to the one we want to save
        RenderTexture.active = renderTexture;

        // Create a Texture2D with the same dimensions and linear color space
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false, true); // linear = true
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        // Save to PNG
        string path = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllBytes(path, tex.EncodeToPNG());

        // Restore previous active RenderTexture
        RenderTexture.active = currentRT;

        Debug.Log($"Saved RenderTexture to {path}");
    }
}
