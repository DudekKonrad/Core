using UnityEditor;
using UnityEngine;

namespace Application.Core.Editor
{
    /// <summary>
    /// Editor window to find and select assets by GUID.
    /// Keyboard shortcut: Alt+G
    /// Menu: Tools > Find Asset by GUID
    /// </summary>
    public class FindAssetByGuidWindow : EditorWindow
    {
        private string _guidInput = "";
        private string _statusMessage = "";
        private MessageType _messageType = MessageType.Info;
        private Object _foundAsset;

        [MenuItem("Tools/Find Asset by GUID &g")]
        public static void ShowWindow()
        {
            var window = GetWindow<FindAssetByGuidWindow>("Find Asset");
            window.minSize = new Vector2(400, 150);
        }

        private void OnGUI()
        {
            GUILayout.Label("Find Asset by GUID", EditorStyles.boldLabel);
            GUILayout.Space(10);

            // GUID input field
            GUILayout.Label("Enter or paste GUID:", EditorStyles.label);
            _guidInput = EditorGUILayout.TextField(_guidInput, GUILayout.Height(30));

            GUILayout.Space(10);

            // Search button
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Find Asset", GUILayout.Height(35)))
                {
                    Find(_guidInput);
                }

                if (GUILayout.Button("Clear", GUILayout.Width(80), GUILayout.Height(35)))
                {
                    _guidInput = "";
                    _statusMessage = "";
                    _foundAsset = null;
                }
            }

            GUILayout.Space(15);

            // Status message
            if (!string.IsNullOrEmpty(_statusMessage))
            {
                EditorGUILayout.HelpBox(_statusMessage, _messageType);
            }

            // Show found asset
            if (_foundAsset != null)
            {
                GUILayout.Space(10);
                GUILayout.Label("Found Asset:", EditorStyles.boldLabel);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(_foundAsset, typeof(Object), false);
                EditorGUI.EndDisabledGroup();
            }
        }

        private void Find(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                SetStatus("Please enter a valid GUID.", MessageType.Warning);
                _foundAsset = null;
                return;
            }

            guid = guid.Trim();
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(path))
            {
                SetStatus($"No asset found with GUID:\n{guid}", MessageType.Error);
                _foundAsset = null;
                return;
            }

            var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (obj == null)
            {
                SetStatus($"Asset exists at:\n{path}\n\nBut couldn't load it.", MessageType.Error);
                _foundAsset = null;
                return;
            }

            _foundAsset = obj;
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
            SetStatus($"✓ Found: {path}", MessageType.Info);
            Debug.Log($"✓ Found asset: {path}", obj);
        }

        private void SetStatus(string message, MessageType type)
        {
            _statusMessage = message;
            _messageType = type;
        }
    }
}

