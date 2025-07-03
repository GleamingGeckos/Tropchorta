using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PNGconverter : MonoBehaviour
{
    public Texture2D texture;

    private void Start()
    {
        SaveTexture(texture);
    }


    private void SaveTexture(Texture2D texture)
    {
        Texture2D modifiedTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            Color original = pixels[i];

            float green = original.g;
            float red = original.r;

            // Opacity based on how much more green than red there is
            // You can clamp it between 0 and 1 to ensure valid alpha
            float alpha = Mathf.Clamp01(green - red + 0.5f); // tweak the +0.5f as bias for midpoint

            // Set final pixel to white with computed alpha
            pixels[i] = new Color(1f, 1f, 1f, alpha);
        }

        modifiedTexture.SetPixels(pixels);
        modifiedTexture.Apply();

        byte[] bytes = modifiedTexture.EncodeToPNG();
        var dirPath = Application.dataPath + "/PNGconverterOutput";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/R_" + Random.Range(0, 100000) + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }




}
