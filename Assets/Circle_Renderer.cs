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
    public CGA.CGA GenerateRotationRotor(float theta,CGA.CGA ea ,CGA.CGA eb){
        var eab=ea^eb;
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*eab;
    }
    public Quaternion RotorToQuat(CGA.CGA R){
        return new Quaternion(R[10], R[7], R[6], R[0]);
    }
    public CGA.CGA QuatToRotor(Quaternion q){
        return q.w + q.x*(e2^e3) + q.y*(e1^e3) + q.z*(e1^e2);
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
        Debug.Log(line.transform.position);
        Debug.Log(line.transform.rotation);

        CreatePoints ();
    }
    void Update()
    {
        float theta = 0.2f;
        CGA.CGA R =  GenerateRotationRotor(theta,e3,e2);
        CGA.CGA currentRoter=QuatToRotor(line.transform.rotation);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
        line.transform.rotation=new_Q;
        
        }

}