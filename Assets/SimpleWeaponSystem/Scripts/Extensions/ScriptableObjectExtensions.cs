using UnityEditor;
using UnityEngine;

namespace RolePoloGame.Extensions
{
    public static class ScriptableObjectExtensions
    {
        public static void AddNested<T>(this ScriptableObject scriptable, ScriptableObject toAdd)
        {
            AssetDatabase.AddObjectToAsset(toAdd, scriptable);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(scriptable);
        }
    }
}
