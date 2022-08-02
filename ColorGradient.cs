using UnityEngine;

namespace Helpers
{
	public class ColorGradient
	{
		private static int          _MaxKeys = 16;

		public int                  ColorKeyCount = 0;
		public float[]              ColorKeyTime;
		public Color[]              ColorKeyColor;

		public int                  AlphaKeyCount = 0;
		public float[]              AlphaKeyTime;
		public float[]              AlphaKeyAlpha;

		public      ColorGradient(Gradient gradient)
		{
			if (gradient == null || gradient.colorKeys.Length < 1)
			{
				Debug.LogWarning("ColorGradient not set. (Invalid gradient passed to constructor)");
				return;
			}
			SetColorKeys(gradient);
			SetAlphaKeys(gradient);
		}

		// =============================== PRIVATE METHODS ===============================
		#region Private Methods

		void		SetColorKeys(Gradient gradient)
		{
			ColorKeyCount = Mathf.Min(gradient.colorKeys.Length, _MaxKeys);
			ColorKeyTime = new float[ColorKeyCount];
			ColorKeyColor = new Color[ColorKeyCount];
			for (int i = 0; i < ColorKeyCount; i++)
			{
				ColorKeyTime[i] = gradient.colorKeys[i].time;
				ColorKeyColor[i] = gradient.colorKeys[i].color;
			}
		}

		void		SetAlphaKeys(Gradient gradient)
		{
			if (gradient.alphaKeys == null || gradient.alphaKeys.Length < 1)
			{
				AlphaKeyCount = 1;
				AlphaKeyTime[0] = 0.5f;
				AlphaKeyAlpha[0] = 1f;
				return ;
			}
			AlphaKeyCount = Mathf.Min(gradient.alphaKeys.Length, _MaxKeys);
			AlphaKeyTime = new float[AlphaKeyCount];
			AlphaKeyAlpha = new float[AlphaKeyCount];
			for (int i = 0; i < AlphaKeyCount; i++)
			{
				AlphaKeyTime[i] = gradient.alphaKeys[i].time;
				AlphaKeyAlpha[i] = gradient.alphaKeys[i].alpha;
			}
		}


		Vector4[]   ColorKeysArray(Color[] colorArray, float[] timeArray)
		{
			Vector4[] result = new Vector4[colorArray.Length];
			for (int i = 0; i < colorArray.Length; i++)
			{
				result[i].x = colorArray[i].r;
				result[i].y = colorArray[i].g;
				result[i].z = colorArray[i].b;
				result[i].w = timeArray[i];
			}
			return result;
		}

		Vector4[]   AlphaKeysArray(float[] alphaArray, float[] timeArray)
		{
			Vector4[] result = new Vector4[alphaArray.Length];
			for (int i = 0; i < alphaArray.Length; i++)
			{
				result[i].x = alphaArray[i];
				result[i].y = timeArray[i];
				result[i].z = 0f;
				result[i].w = 0f;
			}
			return result;
		}

		#endregion

		// ================================ PUBLIC METHODS ===============================
		#region  Public Methods

		public void SetMaterialProperties(MaterialPropertyBlock block, 
					string colorKeyCountProperty, string colorKeysProperty,
					string alphaKeyCountProperty, string alphaKeysProperty)
		{
			if (ColorKeyCount > 0)
			{
				block.SetInt(Shader.PropertyToID(colorKeyCountProperty), ColorKeyCount);
				block.SetVectorArray(Shader.PropertyToID(colorKeysProperty), ColorKeysArray(ColorKeyColor, ColorKeyTime));
				block.SetInt(Shader.PropertyToID(alphaKeyCountProperty), AlphaKeyCount);
				block.SetVectorArray(Shader.PropertyToID(alphaKeysProperty), AlphaKeysArray(AlphaKeyAlpha, AlphaKeyTime));
			}
		}

		public void SetMaterialProperties(Material material, 
					string colorKeyCountProperty, string colorKeysProperty,
					string alphaKeyCountProperty, string alphaKeysProperty)
		{
			if (ColorKeyCount > 0)
			{
				material.SetInt(Shader.PropertyToID(colorKeyCountProperty), ColorKeyCount);
				material.SetVectorArray(Shader.PropertyToID(colorKeysProperty), ColorKeysArray(ColorKeyColor, ColorKeyTime));
				material.SetInt(Shader.PropertyToID(alphaKeyCountProperty), AlphaKeyCount);
				material.SetVectorArray(Shader.PropertyToID(alphaKeysProperty), AlphaKeysArray(AlphaKeyAlpha, AlphaKeyTime));
			}
		}

		#endregion
	}
}
