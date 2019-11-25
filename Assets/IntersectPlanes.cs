using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class IntersectPlanes : MonoBehaviour
{
    
    private static CGA.CGA Plane5D1;
    private static CGA.CGA Plane5D2;
    private static GameObject PlaneObj1;
    private static GameObject PlaneObj2;

    // use four points to define a sphere
    private static Vector3 pnt_a = new Vector3(1f, 10f, -10f);
    private static Vector3 pnt_b = new Vector3(0, 11f, -10f);
    private static Vector3 pnt_c = new Vector3(0, 10f, -9f);

    // another four points to define another sphere
    private static Vector3 pnt_e = new Vector3(1f, 10f, -9.5f);
    private static Vector3 pnt_f = new Vector3(0, 11f, -9.5f);
    private static Vector3 pnt_g = new Vector3(0, 10f, -7f);

    public CGA.CGA GameObjPlaneToPlane5D(GameObject PlaneObj){
        var norm = vector_to_pnt(PlaneObj.transform.up);
        var d = (vector_to_pnt(PlaneObj.transform.position)|norm)[0];
        CGA.CGA Plane5D = !(norm + d*ei);
        return Plane5D;
    }

    public Quaternion SetRotParamforPlane(Vector3 n_roof){
        //rotation from old plane normal (0,1,0) to n_roof
        //rotation angle = the angle between (0,1,0) and (A,B,C)
        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        float theta= (float) Math.Acos(n_roof[1]/scale_of_norm);
        //rotation plane= the plane spaned by (0,1,0) and (A,B,C)
        var rot_plane=(e2^(n_roof[0]*e1+n_roof[1]*e2+n_roof[2]*e3)).normalized();
        CGA.CGA R = GenerateRotationRotor(theta,rot_plane);
        var new_Q=RotorToQuat(R);
        return new_Q;
    }  
    public void UpdateGameObjPlane(GameObject p, Vector3 new_n_roof, Vector3 new_CentrePntOnPlane){
        p.transform.rotation = SetRotParamforPlane(new_n_roof);
        p.transform.position = new_CentrePntOnPlane;
    }
    LineRenderer line;
    private int segments = 1;
    // Start is called before the first frame update
    void Start()
    {
        Plane5D1 = Create5DPlane(pnt_a, pnt_b, pnt_c);
        Plane5D2 = Create5DPlane(pnt_e, pnt_f, pnt_g);
        PlaneObj1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        PlaneObj2 = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Vector3 new_n_roof1=GetPlaneNormal(Plane5D1);
        Vector3 new_n_roof2=GetPlaneNormal(Plane5D2);
        UpdateGameObjPlane(PlaneObj1, new_n_roof1, pnt_a);
        UpdateGameObjPlane(PlaneObj2, new_n_roof2, pnt_e);
        
        //Debug.Log(PointA3D);

        // Add a line render 
        line = PlaneObj1.AddComponent<LineRenderer>();
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = true;


    }

    // Update is called once per frame
    void Update()
    {
        
        Plane5D1 = GameObjPlaneToPlane5D(PlaneObj1);
        Plane5D2 = GameObjPlaneToPlane5D(PlaneObj2);
        CGA.CGA IntersectLine5D = LineByTwoPlanes(Plane5D1, Plane5D2);
        var Direction=ExtractDirectLine(IntersectLine5D);
        var PointonLine=ExtractPointOnLine(IntersectLine5D);
        
        line.SetPosition (0,PointonLine-200*Direction );
        line.SetPosition (1,PointonLine+200*Direction );
    }
}
