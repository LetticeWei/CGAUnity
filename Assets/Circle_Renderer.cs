using UnityEngine;
using System.Collections;
using CGA;
using System;
using static CGA.CGA;

[RequireComponent(typeof(LineRenderer))]
public class Circle_Renderer : MonoBehaviour {
    public int segments = 10;
    public float xradius = 3;
    public float yradius = 3;
    LineRenderer line;
    
    void CreatePoints ()
    {
        float x;
        float y;
        float z;
        float angle = 20f;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            line.SetPosition (i,new Vector3(x,0,z) );
            angle += (360f / segments);
        }
    }


    void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();
        Color c1 = Color.red;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.SetColors(c1, c1);
        line.SetWidth(0.1f, 0.1f);
        line.useWorldSpace = false;

        CreatePoints ();
    }
    void Update()
    {
        float theta = 0.2f;
        CGA.CGA R =  GenerateRotationRotor(theta,e3^e2);
        CGA.CGA currentRoter=QuatToRotor(line.transform.rotation);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
        line.transform.rotation=new_Q;
        
        }

}