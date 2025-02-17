using UnityEngine;
using QRCoder;
using System.IO;

public class QRCodeGeneratorScript : MonoBehaviour
{
    public Renderer qrCodeRenderer;  // Attach the Quad's Renderer here

    void Start()
    {
        string localIP = IPAddressHelper.GetLocalIPAddress();
        GenerateQRCode(localIP);
    }

    void GenerateQRCode(string text)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20); // 20 = pixel size

        // Convert byte array to Texture2D
        Texture2D qrTexture = new Texture2D(256, 256);
        qrTexture.LoadImage(qrCodeImage);
        qrTexture.Apply();

        // Set the transparent background (set all white pixels to transparent)
        MakeBackgroundTransparent(qrTexture);

        // Assign texture to material
        qrCodeRenderer.material.mainTexture = qrTexture;
    }

    // Function to set all white pixels to transparent
    void MakeBackgroundTransparent(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            // If the pixel is white (or near white), make it transparent
            if (pixels[i].r > 0.9f && pixels[i].g > 0.9f && pixels[i].b > 0.9f)
            {
                pixels[i] = new Color(0, 0, 0, 0);  // Set to transparent
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }
}

