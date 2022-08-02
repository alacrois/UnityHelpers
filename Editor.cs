namespace Helpers
{
	#if UNITY_EDITOR

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public static class Editor
	{
		#region Style

			public static class Style
			{
				public static GUIStyle settingsButton1 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fixedWidth = 28,
					fixedHeight = 28,
					margin = new RectOffset(5, 5, 5, 5),
					padding = new RectOffset(5, 5, 5, 5),
				};

				public static GUIStyle backButton1 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					margin = new RectOffset(3, 3, 3, 3),
					padding = new RectOffset(10, 10, 3, 3),
					fixedWidth = 100
				};

				public static GUIStyle smallButton1 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					margin = new RectOffset(5, 5, 5, 5),
					padding = new RectOffset(5, 5, 5, 5)
				};

				public static GUIStyle smallButton2 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					margin = new RectOffset(5, 5, 5, 5),
					padding = new RectOffset(10, 10, 3, 3),
					fixedHeight = 28
				};

				public static GUIStyle smallButton3 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 12,
					margin = new RectOffset(5, 5, 1, 1),
					// padding = new RectOffset(10, 10, 3, 3),
					fixedWidth = 70
				};

				public static GUIStyle enum1 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 13,
					margin = new RectOffset(3, 3, 5, 5),
					padding = new RectOffset(10, 10, 0, 0)
				};

				public static GUIStyle bigButton1 = new GUIStyle(GUI.skin.button) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 35,
					margin = new RectOffset(15, 15, 15, 15),
					padding = new RectOffset(15, 15, 15, 15),
					border = new RectOffset(5, 5, 5, 5)
				};

				public static GUIStyle labelSmall = new GUIStyle(GUI.skin.label) {
					alignment = TextAnchor.MiddleLeft,
					fontStyle = FontStyle.Normal,
					fontSize = 13,
					margin = new RectOffset(10, 10, 10, 10)
				};

				public static GUIStyle label1 = new GUIStyle(GUI.skin.label) {
					alignment = TextAnchor.MiddleLeft,
					fontStyle = FontStyle.Bold,
					fontSize = 12,
					margin = new RectOffset(0, 0, 0, 0),
					padding = new RectOffset(0, 0, 0, 0)
				};

				public static GUIStyle centeredLabel1  = new GUIStyle(GUI.skin.label) {
					alignment = TextAnchor.MiddleCenter,
					fontStyle = FontStyle.Bold,
					fontSize = 14,
				};

				public static GUIStyle mainContainer1 = new GUIStyle(GUI.skin.box) {
					alignment = TextAnchor.MiddleCenter,
					stretchWidth = true,
					margin = new RectOffset(0, 0, 3, 3),
					padding = new RectOffset(5, 5, 6, 6)
				};

				public static GUIStyle labelContainer1 = new GUIStyle(GUI.skin.box) {
					alignment = TextAnchor.MiddleRight,
					fixedWidth = 150,
					margin = new RectOffset(0, 0, 0, 0),
					padding = new RectOffset(3, 3, 3, 3),
					normal = new GUIStyleState() {background = Texture2D.blackTexture}
				};

				public static GUIStyle labelContainer2 = new GUIStyle(GUI.skin.box) {
					alignment = TextAnchor.MiddleRight,
					margin = new RectOffset(0, 0, 0, 0),
					padding = new RectOffset(6, 0, 3, 3),
					normal = new GUIStyleState() {background = Texture2D.blackTexture}
				};
			}

		#endregion

		public static string GetScriptFolderPath(UnityEditor.Editor script)
		{
			MonoScript ms = MonoScript.FromScriptableObject(script);
			string scriptPath = AssetDatabase.GetAssetPath(ms);
			string[] pathParts = scriptPath.Split('/');
			List<string> l = new List<string>(pathParts);
			l.RemoveAt(l.Count - 1);
			scriptPath = string.Join("/", l);
			l.Clear();
			pathParts = null;
			return scriptPath;
		}

		public static string GetAssetFolderPath(Object asset)
		{
			string scriptPath = AssetDatabase.GetAssetPath(asset);
			string[] pathParts = scriptPath.Split('/');
			List<string> l = new List<string>(pathParts);
			l.RemoveAt(l.Count - 1);
			scriptPath = string.Join("/", l);
			l.Clear();
			pathParts = null;
			return scriptPath;
		}

		public static void SelectAsset(Object asset)
		{
			EditorGUIUtility.PingObject(asset);
			Selection.activeObject=asset;
		}

		public static void SelectAssetButton(string text, Object asset, GUIStyle style)
		{
			if (GUILayout.Button(text, style))
			{
				EditorGUIUtility.PingObject(asset);
				Selection.activeObject=asset;
			}
		}

		public static void SelectAssetButton(string text, Object asset)
		{
			SelectAssetButton(text, asset, GUI.skin.button);
		}

		public static int Tabs(int tabSelected, string[] names, params UnityEngine.Events.UnityAction[] content)
		{
			tabSelected = GUILayout.Toolbar(tabSelected, names);
			Helpers.Utils.SwitchAction(tabSelected, content);
			return tabSelected;
		}

		public static void InsideHorizontalContainer(UnityEngine.Events.UnityAction action, string label = null, int labelFontSize = 12)
		{
			GUIStyle labelStyle = new GUIStyle(Style.label1);
			labelStyle.fontSize = labelFontSize;

			GUILayout.BeginHorizontal(Style.mainContainer1);
				if (label != null)
				{
					GUILayout.BeginHorizontal(Style.labelContainer2);
						GUILayout.BeginVertical();
							GUILayout.FlexibleSpace();
							EditorGUILayout.LabelField(label, labelStyle);
							GUILayout.FlexibleSpace();
						GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
				action.Invoke();
			GUILayout.EndHorizontal();
		}

		public static void CenteredSection(UnityEngine.Events.UnityAction action)
		{

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			action.Invoke();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		public static GUIStyle OnOffButtonStyle(bool isOnButton, bool toggleValue, int buttonFontSize = 13)
		{
			GUIStyle onOffButtonStyle = new GUIStyle(Style.smallButton2);
			Color buttonColor = isOnButton ? (toggleValue ? Color.green : Color.white) : (toggleValue ? Color.white : Color.red);
			onOffButtonStyle.fontSize = buttonFontSize;
			onOffButtonStyle.normal.textColor = buttonColor;
			onOffButtonStyle.hover.textColor = buttonColor;
			onOffButtonStyle.active.textColor = buttonColor;
			// onOffButtonStyle.fixedWidth = 150;

			return onOffButtonStyle;
		}

		public static bool ToggleOnOff(string label, bool toggleValue,
			UnityEngine.Events.UnityAction<int> settingsButton = null, int settingsButtonPanel = -1)
		{
			InsideHorizontalContainer(
				() => {
					GUIStyle onButtonStyle = OnOffButtonStyle(true, true, 17);
					GUIStyle offButtonStyle = OnOffButtonStyle(false, false, 17);
					onButtonStyle.fixedWidth = 70;
					offButtonStyle.fixedWidth = 70;
					GUILayout.FlexibleSpace();
					if (toggleValue && GUILayout.Button("ON", onButtonStyle))
						toggleValue = false;
					else if (!toggleValue && GUILayout.Button("OFF", offButtonStyle))
						toggleValue = true;
					if (settingsButton != null && settingsButtonPanel != -1)
						settingsButton(settingsButtonPanel);
				}, label, 13
			);
			return toggleValue;
		}

		public static int EnumPopup(string label, System.Enum enumValue)
		{
			System.Enum result = enumValue;
			InsideHorizontalContainer(
				() => {
					GUIStyle enumStyle = new GUIStyle(Style.smallButton2);
					enumStyle.fontSize = 16;
					enumStyle.fixedHeight = 24;
					result = EditorGUILayout.EnumPopup(enumValue, enumStyle);
				}, label, 13
			);
			return (int)(object)result;
		}
	}

	#endif
}
