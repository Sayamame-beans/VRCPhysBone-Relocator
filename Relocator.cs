using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

public class RelocatorWindow : EditorWindow
{
//properties
    private static GameObject _source;
    private static bool _doNotCheckChildren;
    private bool _wasMissingDetected;

    private const string Version = "0.2.0";
    private static string _lang = "en-US";

    private static readonly Dictionary<string, Dictionary<string, string>> Texts =
        new Dictionary<string, Dictionary<string, string>>()
        {
            {
                "en-US", new Dictionary<string, string>()
                {
                    { "langSwitch", "Switch language to Japanese/日本語にする" },
                    {
                        "summary",
                        "Relocate the PhysBone and PhysBoneCollider Components to the object set in the \"Root Transform\".\nSet a GameObject which has the target PhysBone or PhysBoneCollider Components, and press \"Relocate!\"."
                    },
                    { "doNotCheckChildren", "Don't check child objects"},
                    { "msgWhenError", "\n\nIf this error is unexpected, please contact the author of this extension." },
                    { "source", "Target GameObject" },
                    { "noSource", "You need to set a GameObject." },
                    { "Succeeded-Missing", "Succeeded, but missing was detected.\nIt is recommended to check." },
                    { "Succeeded", "Succeeded!" }
                }
            },
            {
                "ja-JP", new Dictionary<string, string>()
                {
                    { "langSwitch", "Switch language to English/英語にする" },
                    {
                        "summary",
                        "PhysBoneコンポーネント類を\"Root Transform\"に設定されているゲームオブジェクトに再配置します。\n対象のPhysBoneコンポーネント類を含むゲームオブジェクトをセットし、\"Relocate!\"ボタンを押してください。"
                    },
                    { "doNotCheckChildren", "子オブジェクトを確認しない"},
                    { "msgWhenError", "\n\nこのエラーに心当たりが無い場合は、この拡張の作者に連絡してみてください。" },
                    { "source", "対象のゲームオブジェクト" },
                    { "noSource", "ゲームオブジェクトをセットする必要があります。" },
                    { "Succeeded-Missing", "成功しましたが、Root Transformがmissingになっているオブジェクトが見つかりました。\n確認することをお勧めします。" },
                    { "Succeeded", "成功！" }
                }
            }
        };

//methods
    [MenuItem("Tools/PB Relocator", false, 1)]
    public static void ShowWindow()
    {
        GetWindow<RelocatorWindow>("PB Relocator v" + Version);
    }

    private void OnGUI()
    {
        if (GUILayout.Button(Texts[_lang]["langSwitch"]))
        {
            _lang = _lang == "en-US" ? "ja-JP" : "en-US";
        }

        EditorGUILayout.LabelField(Texts[_lang]["summary"], EditorStyles.wordWrappedLabel);

        _doNotCheckChildren = EditorGUILayout.Toggle(Texts[_lang]["doNotCheckChildren"], _doNotCheckChildren);
        _source = (GameObject)EditorGUILayout.ObjectField(Texts[_lang]["source"], _source, typeof(GameObject), true);
        if (_source == null)
        {
            EditorGUILayout.LabelField(Texts[_lang]["noSource"]);
        }
        else if (GUILayout.Button("Relocate!"))
        {
            try
            {
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("PB Relocation");
                var undoIndex = Undo.GetCurrentGroup();

                RelocatePhysBoneComponent();
                RelocatePhysBoneColliderComponent();

                Undo.CollapseUndoOperations(undoIndex);

                if (_wasMissingDetected)
                {
                    EditorUtility.DisplayDialog("Result", Texts[_lang]["Succeeded-Missing"], "OK");
                    _wasMissingDetected = false;
                }
                else
                {
                    EditorUtility.DisplayDialog("Result", Texts[_lang]["Succeeded"], "OK");
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", e.Message + Texts[_lang]["msgWhenError"], "OK");
                Debug.LogError(e);
            }
        }
    }

    private void RelocatePhysBoneComponent()
    {
        foreach (var physBone in _doNotCheckChildren ? _source.GetComponents<VRCPhysBone>() : _source.GetComponentsInChildren<VRCPhysBone>(true))
        {
            var rootTransform = physBone.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                var serializedProperty = new SerializedObject(physBone).FindProperty("rootTransform");
                if (serializedProperty.propertyType != SerializedPropertyType.ObjectReference ||
                    serializedProperty.objectReferenceValue != null)
                {
                    continue;
                }

                var fileID = serializedProperty.FindPropertyRelative("m_FileID");
                if (fileID == null || fileID.intValue == 0)
                {
                    continue;
                }

                _wasMissingDetected = true;
                Debug.LogWarning("Missing detected!");
            }
            else
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(physBone);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(rootTransform.gameObject);
                Undo.DestroyObjectImmediate(physBone);
            }
        }
    }

    private void RelocatePhysBoneColliderComponent()
    {
        foreach (var physBoneCollider in _doNotCheckChildren ? _source.GetComponents<VRCPhysBoneCollider>() : _source.GetComponentsInChildren<VRCPhysBoneCollider>(true))
        {
            var rootTransform = physBoneCollider.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                var serializedProperty = new SerializedObject(physBoneCollider).FindProperty("rootTransform");
                if (serializedProperty.propertyType != SerializedPropertyType.ObjectReference ||
                    serializedProperty.objectReferenceValue != null)
                {
                    continue;
                }

                SerializedProperty fileID = serializedProperty.FindPropertyRelative("m_FileID");
                if (fileID == null || fileID.intValue == 0)
                {
                    continue;
                }

                _wasMissingDetected = true;
                Debug.LogWarning("Missing detected!");
            }
            else
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(physBoneCollider);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(rootTransform.gameObject);
                Undo.DestroyObjectImmediate(physBoneCollider);
            }
        }
    }
}
