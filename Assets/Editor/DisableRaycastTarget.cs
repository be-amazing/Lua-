using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DisableRaycastTarget : MonoBehaviour
{
    [MenuItem("Assets/Disable Raycast Target ", false, 20)]
    public static void Clear()
    {

        if (Selection.activeGameObject != null)
        {
            SetDisableRaycast(Selection.activeGameObject.transform);
            DisableRaycastTargetRecursive(Selection.activeGameObject.transform);
            return;
        }


        if (Selection.assetGUIDs.Length != 0)
        {
            foreach (string guid in Selection.assetGUIDs)
            {
                string folderPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(folderPath) && System.IO.Directory.Exists(folderPath))
                {
                    string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
            
                    foreach (string prefabGUID in prefabGUIDs)
                    {
                        string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            
                        // 处理每个 Prefab
                        SetDisableRaycast(prefab.transform);
                        DisableRaycastTargetRecursive(prefab.transform);

                    }
                }
            } 
        }
    }
    
    [MenuItem("Tools/Disable All Raycast Target ", false, 20)]
    public static void ClearAll()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            DisableRaycastTargetRecursive(prefab.transform);
        }
    }

    private static void DisableRaycastTargetRecursive(Transform parent)
    {
        SetDisableRaycast(parent.transform);

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            SetDisableRaycast(child);
            DisableRaycastTargetRecursive(child);
        }
    }

    private static void SetDisableRaycast(Transform trans)
    {
        Graphic graphic = trans.GetComponent<Graphic>();

        if (graphic != null)
        {
            graphic.raycastTarget = false;
            EditorUtility.SetDirty(graphic);
        }
    }
}