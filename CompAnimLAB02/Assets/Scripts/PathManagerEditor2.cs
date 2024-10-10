using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathManager2))]
public class PathManagerEditor2 : Editor
{
    [SerializeField] PathManager2 pathManager2;

    [SerializeField] List<waypoint2> thePath;
    List<int> toDelete;

    waypoint2 selectedPoint = null;
    bool doRepaint = true;

    private void OnSceneGUI()
    {
        thePath = pathManager2.GetPath();
        DrawPath(thePath);
    }

    private void OnEnable()
    {
        pathManager2 = target as PathManager2;
        toDelete = new List<int>();
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pathManager2.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        //Button for adding a point to the path
        if (GUILayout.Button("Add Point to Path"))
        {
            pathManager2.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (thePath != null && thePath.Count > 0)
        {
            for (int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                waypoint2 p = thePath[i];

                Color c = GUI.color;
                if (selectedPoint == p) GUI.color = Color.green;

                Vector3 oldPos = p.GetPos();
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);

                if (EditorGUI.EndChangeCheck()) p.SetPos(newPos);

                //the delete button
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i); // add the index to our delete list
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();
            }
        }

        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
                thePath.RemoveAt(i); // remove from the path
            toDelete.Clear(); // clear the delete list for the next time

        }
    }

    public void DrawPath(List<waypoint2> path)
    {
        if (path != null)
        {
            int current = 0;
            foreach (waypoint2 wp in path)
            {
                //draw current point
                doRepaint = DrawPoint(wp);
                int next = (current + 1) % path.Count;
                waypoint2 wpnext = path[next];

                DrawPathLine(wp, wpnext);

                //advance counter
                current += 1;
            }
            if (doRepaint) Repaint();
        }

    }

    public void DrawPathLine(waypoint2 p1, waypoint2 p2)
    {
        //Draw a line between current and next point
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1.GetPos(), p2.GetPos());
        Handles.color = c;
    }

    public bool DrawPoint(waypoint2 p)
    {
        bool isChanged = false;

        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = p.GetPos();
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);
            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.4f * handleSize
                , EventType.Repaint);

            if (EditorGUI.EndChangeCheck())
            {
                p.SetPos(newPos);
            }

            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.GetPos();
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize
                , 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }

        return isChanged;
    }
}
