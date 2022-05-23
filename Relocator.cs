using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

public class RelocatorWindow : EditorWindow
{
//properties
    private static GameObject _source = null;
    private bool _wasMissingDetected = false;

    private const string Version = "0.1.0";
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
        RelocatorWindow window = GetWindow<RelocatorWindow>("PB Relocator v" + Version);
    }

    private void OnGUI()
    {
        if (GUILayout.Button(Texts[_lang]["langSwitch"]))
        {
            if (_lang == "en-US")
            {
                _lang = "ja-JP";
            }
            else
            {
                _lang = "en-US";
            }
        }

        EditorGUILayout.LabelField(Texts[_lang]["summary"], EditorStyles.wordWrappedLabel);

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
                int undoIndex = Undo.GetCurrentGroup();

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
        VRCPhysBone[] physBones = _source.GetComponents<VRCPhysBone>();
        foreach (VRCPhysBone physBone in physBones)
        {
            Transform rootTransform = physBone.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                SerializedProperty serializedProperty = new SerializedObject(physBone).FindProperty("rootTransform");
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
                UnityEditorInternal.ComponentUtility.CopyComponent(physBone);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(rootTransform.gameObject);
                Undo.DestroyObjectImmediate(physBone);
            }
        }
    }

    private void RelocatePhysBoneColliderComponent()
    {
        VRCPhysBoneCollider[] physBoneColliders = _source.GetComponents<VRCPhysBoneCollider>();
        foreach (VRCPhysBoneCollider physBoneCollider in physBoneColliders)
        {
            Transform rootTransform = physBoneCollider.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                SerializedProperty serializedProperty =
                    new SerializedObject(physBoneCollider).FindProperty("rootTransform");
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
