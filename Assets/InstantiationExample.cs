using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;

public class InstantiationExample : MonoBehaviour 
{
    // create a sphere
    public CGA.CGA Create5DSphere(CGA.CGA A, CGA.CGA B,CGA.CGA C,CGA.CGA D){
        // eab, ecd, efg, ehi are all bivectors
        var sigma=A^B^C^D;
        return sigma;
    }
    public float findRadius(CGA.CGA sigma){
        return Mathf.Sqrt(((!sigma)*(!sigma))[0]);
    }

    public Vector3 findCentre(CGA.CGA sigma){
        var CGAVector=sigma*(eo+ei)*sigma;
        return new Vector3 (CGAVector[1], CGAVector[2], CGAVector[3]);
    }

    // This script will simply instantiate the objects when the game starts.
    void Start()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // use four points to define a sphere
        var a=new Vector3(0, 1.5f, 0);
        var b=new Vector3(1f, 1.5f, 0);
        var c=new Vector3(0, 0, 3f);
        var d=new Vector3(1.2f, 1.0f, 2.3f);
        var A= up (a.x, a.y, a.z);
        var B= up (b.x, b.y, b.z);
        var C= up (c.x, c.y, c.z);
        var D= up (d.x, d.y, d.z);
        var sigma=Create5DSphere(A,B,C,D);
        var centre= findCentre(sigma);
        var radius=findRadius(sigma);
        sphere.transform.position = centre;
        sphere.transform.localScale = new Vector3 (1, 1, 1)*radius;

        var : GameObject  cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);
        cube.transform.localScale = new Vector3 (1.25f, 1.5f, 1f);
    }
}

