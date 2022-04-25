using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

public class BF_TerrainEditor : Editor
{
    [CustomEditor(typeof(BF_Terrain))]
    class DecalMeshHelperEditor : Editor
    {
        private GUIStyle style;
        public override void OnInspectorGUI()
        {
            BF_Terrain myTarget = (BF_Terrain)target;
            myTarget.terrainToCopy = EditorGUILayout.ObjectField("Terrain To Copy (Data Override)", myTarget.terrainToCopy, typeof(Terrain), true) as Terrain;
            if(style == null)
            {
                style = new GUIStyle(GUI.skin.button);
            }
            if (myTarget.terrainToCopy != null)
            {
                if (GUILayout.Button("Sync Terrain Data (Data Override)", style))
                {
                    myTarget.CopyTerrainData();
                    myTarget.MoveTerrainSync();
                    style.normal.background = Texture2D.linearGrayTexture;
                }
                if (GUILayout.Button("Revert Terrain Data"))
                {
                    myTarget.RevertTerrainData();
                    style.normal.background = Texture2D.whiteTexture;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
