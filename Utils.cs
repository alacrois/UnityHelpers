using UnityEngine;

namespace Helpers
{
    public static class Utils
	{
		private static Transform	camera = null;

		public static void SwitchAction(int actionIndex, params UnityEngine.Events.UnityAction[] actions)
		{
			if (actionIndex < actions.Length)
				actions[actionIndex].Invoke();
		}

		public static float Remap(float value, float minStart, float maxStart, float minEnd, float maxEnd)
		{
			if (minStart == maxStart)
				return minEnd;
			value = Mathf.Clamp(value, minStart, maxStart);
			float perc = (value - minStart) / (maxStart - minStart);
			float result = perc * (maxEnd - minEnd) + minEnd;
			return result;
		}

		public static Transform GetCamera()
		{
			if (camera == null)
				camera = Camera.main.transform;
			if (camera == null)
				Debug.Log("Failed to get camera transform !");
			return camera;
		}

		public static void Compute(ComputeShader cs, int numIterationsX, int numIterationsY = 1, int numIterationsZ = 1, int kernelIndex = 0)
		{
			uint groupSizeX, groupSizeY, groupSizeZ;
			cs.GetKernelThreadGroupSizes(kernelIndex, out groupSizeX, out groupSizeY, out groupSizeZ);
			int numGroupsX = Mathf.CeilToInt(numIterationsX / (float)groupSizeX);
			int numGroupsY = Mathf.CeilToInt(numIterationsY / (float)groupSizeY);
			int numGroupsZ = Mathf.CeilToInt(numIterationsZ / (float)groupSizeZ);
			cs.Dispatch(kernelIndex, numGroupsX, numGroupsY, numGroupsZ);
		}
	}
}