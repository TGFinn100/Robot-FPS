using UnityEngine;

public class SteamTools : MonoBehaviour
{
	//A script for steam tools

	//Creates a texture for a raw image made from the specific friend
	public static Texture2D GetTexture2DFromImage(Steamworks.Data.Image image)
	{
		Texture2D texture2D = new Texture2D((int)image.Width, (int)image.Height);

		for (int x = 0; x < image.Width; x++)
		{
			for (int y = 0; y < image.Height; y++)
			{
				var pixel = image.GetPixel(x, y);
				texture2D.SetPixel(x, (int)image.Height - y, new UnityEngine.Color(pixel.r / 255.0f, pixel.g / 255.0f, pixel.b / 255.0f, pixel.a / 255.0f));
			}
		}
		texture2D.Apply();
		return texture2D;
	}
}
