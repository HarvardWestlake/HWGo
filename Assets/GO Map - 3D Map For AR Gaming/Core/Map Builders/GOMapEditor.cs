﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using GoShared;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GoMap
{
    [System.Serializable]
    public class CustomCoordinatesBounds
    {
        public double minLatitude;
        public double minLongitude;
        public double maxLatitude;
        public double maxLongitude;

        public CustomCoordinatesBounds(double minLat, double minLon, double maxLat, double maxLon)
        {
            minLatitude = minLat;
            minLongitude = minLon;
            maxLatitude = maxLat;
            maxLongitude = maxLon;
        }
    }

    public class GOMapEditor : MonoBehaviour
    {
        public float portionSize = 100.0f; // You can adjust this value based on your requirements

        public void BuildMapPortionInsideEditor(Coordinates location, Coordinates portionBounds)
        {
            GOMap map = GetComponent<GOMap>();

            if (map == null)
            {
                Debug.LogError("[GOMap Editor] GOMap script not found! GOMapEditor and GOMap have to be attached to the same GameObject");
                return;
            }

            // Build the map portion
            map.BuildMapPortionInsideEditor(location, portionBounds);
        }

        public void DestroyCurrentMap()
        {
            GOMap map = GetComponent<GOMap>();
            if (map == null)
            {
                Debug.LogError("[GOMap Editor] GOMap script not found");
                return;
            }

            while (map.transform.childCount > 0)
            {
                foreach (Transform child in map.transform)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }

            GOEnvironment env = GameObject.FindObjectOfType<GOEnvironment>();
            if (env == null)
            {
                return;
            }

            while (env.transform.childCount > 0)
            {
                foreach (Transform child in env.transform)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
            }
        }

        // Other methods...

        public void LoadInsideEditor()
        {
            GOMap map = GetComponent<GOMap>();
            if (map == null)
            {
                Debug.LogError("[GOMap Editor] GOMap script not found");
                return;
            }

            map.BuildInsideEditor();
        }

        public void TestUnityWebRequestInEditor()
        {
            GOMap map = GetComponent<GOMap>();
            if (map == null)
            {
                Debug.LogError("[GOMap Editor] GOMap script not found! GOMapEditor and GOMap have to be attached to the same GameObject");
                return;
            }
            map.TestEditorUnityWebRequest();
        }

        IEnumerator ClearConsole()
        {
            // wait until console visible
            while (!Debug.developerConsoleVisible)
            {
                yield return null;
            }
            yield return null; // this is required to wait for an additional frame, without this clearing doesn't work (at least for me)
            Debug.ClearDeveloperConsole();
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(GOMapEditor))]
    public class GOMapInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUIStyle style = new GUIStyle();
            style.fontSize = 12;
            style.normal.textColor = Color.white;
            GUILayout.Space(10);

            GOMapEditor editor = (GOMapEditor)target;
            GUILayout.Label("Use this while the application is Not Running!", style);
            GUILayout.Space(20);

            GUILayout.Label("This script allows you to load the map directly\nin the scene.\n" +
                "In this way you can edit it, save it and it will\nbe available offline.");
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("It might take some time depending on how\nbig is the tile buffer set on GoMap component.", MessageType.Info);
            if (GUILayout.Button("Load Map in Editor"))
            {
                editor.LoadInsideEditor();
            }
            GUILayout.Space(20);
            EditorGUILayout.HelpBox("This destroys everything in the map hierarchy,\nuse this before loading another map inside the editor.", MessageType.Info);
            if (GUILayout.Button("Destroy Map in Editor"))
            {
                editor.DestroyCurrentMap();
            }
        }
    }

#endif
}