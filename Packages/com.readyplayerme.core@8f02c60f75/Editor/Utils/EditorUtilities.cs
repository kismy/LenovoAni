using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ReadyPlayerMe.Core.Editor
{
    public static class EditorUtilities
    {
        private const string TAG = nameof(EditorUtilities);
        private const string SHORT_CODE_REGEX = "^[A-Z0-9]{6}$";

        public static void CreatePrefab(GameObject source, string path)
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(source, path, InteractionMode.AutomatedAction, out var success);
            PrefabUtility.ApplyObjectOverride(source, path, InteractionMode.AutomatedAction);

            SDKLogger.Log(TAG, success ? $"Prefab created successfully at path: {path}" : "Prefab creation failed");

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(source);
        }

        public static string TextFieldWithPlaceholder(string text, string placeholder, params GUILayoutOption[] layoutOptions)
        {
            var newText = EditorGUILayout.TextField(text, layoutOptions);
            if (string.IsNullOrEmpty(text))
            {
                Color previousColor = GUI.color;
                GUI.color = Color.grey;
                EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), placeholder);
                GUI.color = previousColor;
            }
            return newText;
        }

        public static bool IsUrlShortcodeValid(string urlString)
        {
            return !string.IsNullOrEmpty(urlString) &&
                   (Regex.Match(urlString, SHORT_CODE_REGEX).Length > 0 || Uri.IsWellFormedUriString(urlString, UriKind.Absolute) && urlString.EndsWith(".glb"));
        }

        public static GameObject CreateAvatarPrefab(AvatarMetadata avatarMetadata, string path, string prefabPath = null)
        {
            var modelFilePath = $"{path}.glb";
            AssetDatabase.Refresh();
            Object avatarSource = AssetDatabase.LoadAssetAtPath(modelFilePath, typeof(GameObject));
            var newAvatar = (GameObject) PrefabUtility.InstantiatePrefab(avatarSource);
            var avatarProcessor = new AvatarProcessor();
            PrefabUtility.UnpackPrefabInstance(newAvatar, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            avatarProcessor.ProcessAvatar(newAvatar, avatarMetadata);
            var avatarData = newAvatar.AddComponent<AvatarData>();
            avatarData.AvatarMetadata = avatarMetadata;
            avatarData.AvatarId = newAvatar.name;
            CreatePrefab(newAvatar, prefabPath ?? $"{path}.prefab");
            return newAvatar;
        }
    }
}
