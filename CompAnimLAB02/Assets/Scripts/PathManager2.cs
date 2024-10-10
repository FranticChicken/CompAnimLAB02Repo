using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager2 : MonoBehaviour
{
    [HideInInspector]
    [SerializeField] public List<waypoint2> path;

    public GameObject prefab;
    int currentPointIndex = 0;

    public List<GameObject> prefabPoints;

    public List<waypoint2> GetPath()
    {
        if (path == null)
            path = new List<waypoint2>();

        return path;
    }

    public void CreateAddPoint()
    {
        waypoint2 go = new waypoint2();
        path.Add(go);
    }

    public waypoint2 GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % (path.Count);
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }

    private void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (waypoint2 p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.pos;
            prefabPoints.Add(go);
        }
    }

    public void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            waypoint2 p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.pos;
        }
    }
}
