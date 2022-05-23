using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

public class RelocatorWindow : EditorWindow
{
//properties
    private static GameObject source = null;
    private bool wasMissingDetected = false;

    private static readonly string version = "0.1.0";
    private static string lang = "en-US";

    private static readonly Dictionary<string, Dictionary<string, string>> texts =
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
        RelocatorWindow window = GetWindow<RelocatorWindow>("PB Relocator v" + version);
    }

    private void OnGUI()
    {
        if (GUILayout.Button(texts[lang]["langSwitch"]))
        {
            if (lang == "en-US")
            {
                lang = "ja-JP";
            }
            else
            {
                lang = "en-US";
            }
        }

        EditorGUILayout.LabelField(texts[lang]["summary"], EditorStyles.wordWrappedLabel);

        source = (GameObject)EditorGUILayout.ObjectField(texts[lang]["source"], source, typeof(GameObject), true);
        if (source == null)
        {
            EditorGUILayout.LabelField(texts[lang]["noSource"]);
        }
        else if (GUILayout.Button("Relocate!"))
        {
            try
            {
                Undo.IncrementCurrentGroup();
                Undo.SetCurrentGroupName("PB Relocation");
                int undoIndex = Undo.GetCurrentGroup();

                relocatePhysBoneComponent();
                relocatePhysBoneColliderComponent();

                Undo.CollapseUndoOperations(undoIndex);

                if (wasMissingDetected)
                {
                    EditorUtility.DisplayDialog("Result", texts[lang]["Succeeded-Missing"], "OK");
                    wasMissingDetected = false;
                }
                else
                {
                    EditorUtility.DisplayDialog("Result", texts[lang]["Succeeded"], "OK");
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", e.Message + texts[lang]["msgWhenError"], "OK");
                Debug.LogError(e);
            }
        }
    }

    private void relocatePhysBoneComponent()
    {
        VRCPhysBone[] physbones = source.GetComponents<VRCPhysBone>();
        foreach (VRCPhysBone physbone in physbones)
        {
            Transform rootTransform = physbone.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                SerializedProperty serializedProperty = new SerializedObject(physbone).FindProperty("rootTransform");
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

                wasMissingDetected = true;
                Debug.LogWarning("Missing detected!");
            }
            else
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(physbone);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(rootTransform.gameObject);
                Undo.DestroyObjectImmediate(physbone);
            }
        }
    }

    private void relocatePhysBoneColliderComponent()
    {
        VRCPhysBoneCollider[] physboneColliders = source.GetComponents<VRCPhysBoneCollider>();
        foreach (VRCPhysBoneCollider physboneCollider in physboneColliders)
        {
            Transform rootTransform = physboneCollider.rootTransform;
            if (rootTransform == null)
            {
                //Missing or None
                SerializedProperty serializedProperty =
                    new SerializedObject(physboneCollider).FindProperty("rootTransform");
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

                wasMissingDetected = true;
                Debug.LogWarning("Missing detected!");
            }
            else
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(physboneCollider);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(rootTransform.gameObject);
                Undo.DestroyObjectImmediate(physboneCollider);
            }
        }
    }
}
