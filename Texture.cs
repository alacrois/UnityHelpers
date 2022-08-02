using UnityEngine;
using UnityEditor;

namespace Helpers
{
    public static class Texture
	{
		public static Texture2D ToTexture2D(this RenderTexture source, TextureFormat format = TextureFormat.ARGB32)
		{
			Texture2D output = new Texture2D(source.width, source.height, format, false);
			output.wrapMode = source.wrapMode;
			output.filterMode = source.filterMode;
			output.name = source.name;

			RenderTexture.active = source;
			output.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
			output.Apply();
			return output;
		}

		public static Texture3D ToTexture3D(this RenderTexture source, TextureFormat format = TextureFormat.ARGB32)
		{
			Texture3D output = new Texture3D(source.width, source.height, source.volumeDepth, format, 0);
			output.wrapMode = source.wrapMode;
			output.filterMode = source.filterMode;
			output.name = source.name;

			ComputeShader slicer = GetSlicer();
			if (!slicer)
			{
				Debug.LogError("Failed to get slicer compute shader. Conversion to Texture3D aborted.");
				return null;
			}

			Texture2D[] texture2DSlices = new Texture2D[source.volumeDepth];
			for (int i = 0; i < source.volumeDepth; i++)
				texture2DSlices[i] = Copy3DSliceToRenderTexture(slicer, source, i).ToTexture2D();

			Color[] outputPixels = output.GetPixels();

			for (int z = 0; z < source.volumeDepth; z++)
			{
				Color[] slicePixels = texture2DSlices[z].GetPixels();
				for (int y = 0; y < source.height; y++)
				{
					for (int x = 0; x < source.width; x++)
					{
						outputPixels[z * source.height * source.width + y * source.width + x] = slicePixels[y * source.width + x];
					}
				}
			}

			output.SetPixels(outputPixels);
			output.Apply();

			return output;

			ComputeShader GetSlicer()
			{
				var guid = AssetDatabase.FindAssets("Texture3DSlicer");
				string slicerPath = AssetDatabase.GUIDToAssetPath(guid[0]);
				ComputeShader slicer = (ComputeShader)AssetDatabase.LoadAssetAtPath(slicerPath, typeof(ComputeShader));
				return slicer;
			}

			RenderTexture Copy3DSliceToRenderTexture(ComputeShader slicer, RenderTexture source, int zLayer)
			{
				RenderTexture render = new RenderTexture(source.width, source.height, 0, source.format);
				render.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
				render.enableRandomWrite = true;
				render.wrapMode = TextureWrapMode.Clamp;
				render.Create();
		
				slicer.SetTexture(0, "_InputTexture", source);
				slicer.SetTexture(0, "_OutputSlice", render);
				slicer.SetInt("_SizeX", source.width);
				slicer.SetInt("_SizeY", source.height);
				slicer.SetInt("_ZLayer", zLayer);

				Utils.Compute(slicer, source.width, source.height);
		
				return render;
			}
		}
	}
}