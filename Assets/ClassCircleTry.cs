using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
public class Plane{
    public Vector3 norm;
    public Vector3 centrePoint;
    public CGA.CGA centrePoint5D;
    public float DistToOrigin;
    public CGA.CGA Plane5D;
    public GameObject PlaneGameObj=GameObject.CreatePrimitive(PrimitiveType.Plane);

    public LineRenderer line;
    public int segments;

    public void GetPlaneNormal(){
        CGA.CGA n_roof=(!(Plane5D.normalized()))-((!Plane5D.normalized())|eo)*ei;
		norm= pnt_to_vector(n_roof);
    }
    public void GetPlaneDist(){
		DistToOrigin= (float) ((!Plane5D.normalized())|eo)[0];
	}
    public void UpdateGameObjPlane(){
        //rotation from old plane normal (0,1,0) to norm
        //rotation angle = the angle between (0,1,0) and (A,B,C)
        float scale_of_norm=Mathf.Sqrt(norm[0]*norm[0]+norm[1]*norm[1]+norm[2]*norm[2]);
        float theta= (float) Math.Acos(norm[1]/scale_of_norm);
        //rotation plane= the plane spaned by (0,1,0) and (A,B,C)
        var rot_plane=(e2^(norm[0]*e1+norm[1]*e2+norm[2]*e3)).normalized();
        CGA.CGA R = GenerateRotationRotor(theta,rot_plane);
        PlaneGameObj.transform.rotation = RotorToQuat(R);
        PlaneGameObj.transform.position = centrePoint;
    }
    public void SetUpLineRenderOnPlaneObj(bool use_world_space){
        line = PlaneGameObj.AddComponent<LineRenderer>();
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = use_world_space;}
}

public class Circle{
    public Vector3 Centre;public float Radius;
    public Vector3 PointA;public Vector3 PointB;public Vector3 PointC;
    public CGA.CGA Circle5D;public CGA.CGA Ic; //plane on which the circle lies
    public Plane PlaneForCircle;
    public int segments=50;

    // member function or method
    public void CreateCircle5DusingThreePoints(){   //or should it be called "Update"?
        CGA.CGA A = up(PointA.x, PointA.y, PointA.z);
        CGA.CGA B = up(PointB.x, PointB.y, PointB.z);
        CGA.CGA C = up(PointC.x, PointC.y, PointC.z);
        Circle5D = A ^ B ^ C;
    }


    public void CreateCircle5DusingCentreAndRadius(){ // may not work coz the plane on which it lies is not encoded
        CGA.CGA Centre5D=up(Centre.x, Centre.y, Centre.z);
        Circle5D= !(Centre5D+(-0.5f)*Radius*Radius*ei);}

    public void UpdateCircleCentre(){   
        CGA.CGA CGAVector2 = down(Circle5D * ei * Circle5D);
        Centre= new Vector3(CGAVector2[1], CGAVector2[2], CGAVector2[3]);
    }
    public void FindIcUsingCircle5D(){
        //find the plane on which the circle lies
        Ic = (ei ^ Circle5D).normalized();
        }
    public void UpdateCircleRadius(){  
        var Circle5D_star2 = normalise_pnt_minus_one(Circle5D*Ic);
        float CircleRadiusSqr = (Circle5D_star2 * Circle5D_star2)[0];
        Radius= Mathf.Sqrt(Math.Abs(CircleRadiusSqr));
    }
    public void CreateCirclePlaneObjOnScene(){
        //create the plane object on which the circle lie
        PlaneForCircle=new Plane();
        PlaneForCircle.Plane5D=Ic;
        PlaneForCircle.GetPlaneNormal();
        PlaneForCircle.centrePoint=Centre;
        PlaneForCircle.UpdateGameObjPlane();
    }

    public void drawCircleOnPlaneOnScene(){
        
        //draw the circle on the plane it lies on.
        PlaneForCircle.segments=segments;
        PlaneForCircle.SetUpLineRenderOnPlaneObj(false);
        PlaneForCircle.line = PlaneForCircle.PlaneGameObj.GetComponent<LineRenderer>();
        float x;
        float z;
        float angle = 2f;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * Radius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * Radius;
            PlaneForCircle.line.SetPosition (i,new Vector3(x,0,z) );
            angle += (360f / segments);
        }
    }

}
public class ClassCircleTry : MonoBehaviour
{   public static Circle Circle1;
    // Start is called before the first frame update
    void Start()
    {   
        Circle1=new Circle();
        Circle1.PointA=new Vector3(1f, 0, 0);
        Circle1.PointB=new Vector3(0, 1f, 0);
        Circle1.PointC=new Vector3(0, 0, 3f);
        Circle1.CreateCircle5DusingThreePoints();
        Circle1.FindIcUsingCircle5D();
        Circle1.UpdateCircleRadius();
        Circle1.UpdateCircleCentre();
        Circle1.CreateCirclePlaneObjOnScene();
        Circle1.drawCircleOnPlaneOnScene();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
