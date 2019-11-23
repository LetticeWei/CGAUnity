using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class PlaneandLineIntersect : MonoBehaviour
{
    private static Vector3 pnt_a = new Vector3(1f, 0, 4f);
    private static Vector3 pnt_b = new Vector3(0, 1f, 4f);
    private static Vector3 pnt_c = new Vector3(0, 0, 5f);
    private static Vector3 pnt_e = new Vector3(0, 3.5f, 0);
    private static Vector3 pnt_f = new Vector3(0, 0, -6f);

    private static CGA.CGA Plane5D1;
    private static GameObject PlaneObj1;
    private static CGA.CGA Line5D1;
    private static GameObject LineVertix1;
    private static GameObject LineVertix2;
    Renderer LineVertix1Renderer;
    Renderer LineVertix2Renderer;
    LineRenderer line;
    private static GameObject IntersectPointObj;
    Renderer IntersectPointObjRenderer;
    private int segments = 1;

    // Start is called before the first frame update
    void Start()
    {
        // Plane5D1 = Create5DPlane(pnt_a, pnt_b, pnt_c);
        // PlaneObj1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // Vector3 new_n_roof1=GetPlaneNormal(Plane5D1);
        // UpdateGameObjPlane(PlaneObj1, new_n_roof1, pnt_a);

        // Line5D1=Create5DLine(pnt_e,pnt_f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
