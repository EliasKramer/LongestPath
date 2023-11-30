using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class controller_script : MonoBehaviour
{
    [SerializeField]
    GameObject u_point;
    [SerializeField]
    GameObject connection;

    List<GameObject> gs = new List<GameObject>();

    IPathSolver solver = new Solver_2();

    List<int> path = new List<int>();
    public int n = 40;
    void Start()
    {
        //setup points in a circle around this object as the center
        for (int i = 0; i < n; i++)
        {
            float angle = i * Mathf.PI * 2f / n;
            Vector3 newPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 5f;
            GameObject newPoint = Instantiate(u_point, newPos, Quaternion.identity, this.gameObject.transform);
            newPoint.transform.parent = transform;
            newPoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            gs.Add(newPoint);
        }

        var result = solver.CalculatePath(n);
        path = result.Path;

        PathHelper.PathErrorCheck(path,n);

        Debug.Log($"optimal: {PathHelper.OptimalConnectionCount(n)} calculated: {result.ConnectionCount}");

        StartCoroutine(connectingPointsCoroutine());
        //n = 2;
    }

    public IEnumerator connectingPointsCoroutine()
    {
        
        /*
        while (true)
        {
            var r = solver.CalculatePath(n);
            PathHelper.PathErrorCheck(r.Path, n);
            if (r.ConnectionCount == PathHelper.OptimalConnectionCount(n))
            {
                Debug.Log(n);
            }
            else
            {
               // Debug.Log($"{n} not valid. calc: {r.ConnectionCount} optimal: {PathHelper.OptimalConnectionCount(n)}");
            }
            n++;

            yield return new WaitForSeconds(.01f);
        }
        */
        for (int i = 1; i < path.Count; i++)
        {
            ConnectTwoPoints(path[i - 1], path[i]);
            yield return new WaitForSeconds(.5f);
        }
    }
    public void ConnectTwoPoints(int first_idx, int second_idx)
    {
        GameObject first = gs.ElementAt(first_idx);
        GameObject second = gs.ElementAt(second_idx);

        Vector2 first_pos = first.transform.position;
        Vector2 second_pos = second.transform.position;

        float dist_between = Mathf.Sqrt(Mathf.Pow(first_pos.x - second_pos.x, 2) + Mathf.Pow(first_pos.y - second_pos.y, 2));

        //pos between
        Vector2 pos_between = (first_pos + second_pos) / 2;
        GameObject newConnection = Instantiate(connection, pos_between, Quaternion.identity, this.gameObject.transform);

        //set scale 
        newConnection.transform.localScale = new Vector3(dist_between, 0.01f, 1);

        //set rotation

        float angle = Mathf.Atan2(second_pos.y - first_pos.y, second_pos.x - first_pos.x) * Mathf.Rad2Deg;

        newConnection.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}