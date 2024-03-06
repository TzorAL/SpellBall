using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake(){
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector2 startPoint, Vector3 endPoint){
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = endPoint;

        lr.SetPositions(points);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndLine(){
        lr.positionCount = 0;
    }
}
