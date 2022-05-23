using CustomLocalization4EditorExtension;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

public class RelocatorWindow : EditorWindow
{
//properties
    private static GameObject _source;
    private bool _wasMissingDetected;

    private Localization _localization = new Localization(
        "6384038aaded94ab598437c5a398be61", "ja");

    private const string Version = "0.1.0";

    //methods
    [MenuItem("Tools/PB Relocator", false, 1)]
    public static void ShowWindow()
    {
        GetWindow<RelocatorWindow>("PB Relocator v" + Version);
    }

    private void OnGUI()
    {
        _localization.Setup();
        _localization.DrawLanguagePicker();

        EditorGUILayout.LabelField(_localization.Tr("summary"), EditorStyles.wordWrappedLabel);

        _source = (GameObject)EditorGUILayout.ObjectField(_localization.Tr("source"), _source, typeof(GameObject),
            true);
        if (_source == null)
        {
            EditorGUILayout.LabelField(_localization.Tr("noSource"));
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
                    EditorUtility.DisplayDialog("Result", _localization.Tr("Succeeded-Missing"), "OK");
                    _wasMissingDetected = false;
                }
                else
                {
                    EditorUtility.DisplayDialog("Result", _localization.Tr("Succeeded"), "OK");
                }
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Error", e.Message + _localization.Tr("msgWhenError"), "OK");
                Debug.LogError(e);
            }
        }
    }

    private void RelocatePhysBoneComponent()
    {
        foreach (var physBone in _source.GetComponents<VRCPhysBone>())
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
        foreach (var physBoneCollider in _source.GetComponents<VRCPhysBoneCollider>())
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
