using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;



public class Geometry2{
    public  GameObject thisGameObject;
    // constructor here:
    public Geometry2(){
    }
}

public class PlaneTry:Geometry2{
    public float type_index;
    public PlaneTry(float type_index) :
                     base()
    {
    this.thisGameObject=GameObject.CreatePrimitive(PrimitiveType.Plane);
    this.thisGameObject.transform.position=new Vector3(1f,2f,3f);
    this.type_index=type_index;
    }

}
public class Geometry{
    public  GameObject PlaneGameObj;
    public Geometry(){
    }
    }
public class Plane:Geometry {
    public Vector3 norm;public Vector3 centrePoint;public CGA.CGA centrePoint5D;public float DistToOrigin;
    public CGA.CGA Plane5D; public Vector3 a;public Vector3 b;public Vector3 c;
    public LineRenderer line;public int segments;

    public Plane():base(){
        this.PlaneGameObj=GameObject.CreatePrimitive(PrimitiveType.Plane);
    }

    public void FindPlane5DbyThreePoints(Vector3 pnt_a, Vector3 pnt_b, Vector3 pnt_c){
        a=pnt_a;b=pnt_b;c=pnt_c;
        Plane5D = up(a.x, a.y, a.z) ^ up(b.x, b.y, b.z) ^ up(c.x, c.y, c.z) ^ ei;
	}

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
        //above: previous 'SetRotParamforPlane'
        this.PlaneGameObj.transform.rotation = RotorToQuat(R);
        // centrePoint=a;
        this.PlaneGameObj.transform.position = centrePoint;
    }
    public void SetUpLineRenderOnPlaneObj(bool use_world_space){
        line = this.PlaneGameObj.AddComponent<LineRenderer>();

        Gradient gradient = new Gradient();
        float alpha=1.0f;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        line.colorGradient = gradient;

        Color c1 = Color.red;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);

        AnimationCurve curve = new AnimationCurve();
        float scale=0.2f;
        curve.AddKey(0, scale);
        curve.AddKey(1, scale);

        line.widthCurve=curve;
        line.useWorldSpace = use_world_space;}
    public void GameObjPlaneToPlane5D(){
        CGA.CGA norm5D = vector_to_pnt(this.PlaneGameObj.transform.up);
        DistToOrigin = (vector_to_pnt(this.PlaneGameObj.transform.position)|norm5D)[0];
        Plane5D = !(norm5D + DistToOrigin*ei);}
}

    public class Point:Geometry{
        public Vector3 Point3D= new Vector3(0,0,0); public CGA.CGA Point5D; public CGA.CGA PointTwoBlades; // PointTwoBlades is the result of intersection of a plane and a line
        public GameObject PointObj= GameObject.CreatePrimitive(PrimitiveType.Sphere);
        public Renderer PointObjRenderer;
        public void SetUpRenderer(){
            PointObjRenderer= PointObj.GetComponent<Renderer>();
        } 
        public void Extract5DPointfromTwoBlade(){
            Point5D=(PointTwoBlades^eo)|(ei^eo);}
        public void FindPoint3Dfrom5D(){
            Point3D=pnt_to_vector(down(Point5D));
        } 
        public void FindPoint5Dfrom3D(){
            Point5D=up(Point3D.x,Point3D.y,Point3D.z);
        }     
        public void SetupPointObject(float r=1.0f, float g=0, float b=0){ 
            SetUpRenderer();
            PointObj.transform.position=Point3D;
            PointObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            PointObjRenderer.material.color= new Color(r,g,b,0); //default colour is red
        }

        public void UpdatePoint3DfromObject(){
            Point3D=PointObj.transform.position;
        }
        public void UpdatePointObject(){
            PointObj.transform.position=Point3D;
        }
    }
public class Line:Geometry{
    public Point Vertex1=new Point();public Point Vertex2=new Point(); public CGA.CGA Line5D; public Vector3 LineDirection;
    public LineRenderer lineRenderer; public int segments=1;
    public void SetUpVertices3Dand5D(Vector3 pnt_a,Vector3 pnt_b){
        Vertex1.Point3D=pnt_a;
        Vertex2.Point3D=pnt_b;
        Vertex1.FindPoint5Dfrom3D();
        Vertex2.FindPoint5Dfrom3D();
    }

    public void FindLine5DfromVertices5D(){
        Line5D = Vertex1.Point5D ^ Vertex2.Point5D ^ ei;
    }
    // public void ExtractDirectLine(){
	// 	LineDirection=-1f*(!Line5D)*I3;
	// }
    public void SetupVertexObj(){

        Vertex1.SetupPointObject();
        Vertex2.SetupPointObject();
    }
    public void SetUpLineRenderer(){
        lineRenderer=Vertex1.PointObj.AddComponent<LineRenderer>(); //attach the line renderer to one of the vertices' game object.
        //lineRenderer.material.color= new Color(0,1.0f,0,0);
        lineRenderer.SetVertexCount (segments + 1);
        //Color c1 = Color.white;
        //line.SetColors(c1, c1);

        AnimationCurve curve = new AnimationCurve();
        float scale=0.2f;
        curve.AddKey(0, scale);
        curve.AddKey(1, scale);
        
        lineRenderer.widthCurve = curve;
        

        Gradient gradient = new Gradient();
        float alpha=1.0f;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        lineRenderer.useWorldSpace = true;
    }
    public void UpdateLine5DfromVertices(){
        var a=Vertex1.Point3D;
        var b=Vertex2.Point3D;
        Vertex1.FindPoint5Dfrom3D();
        Vertex2.FindPoint5Dfrom3D();
		FindLine5DfromVertices5D();
    }

    public void UpdateVerticesFromObject(){
        Vertex1.UpdatePoint3DfromObject();
        Vertex2.UpdatePoint3DfromObject();
    }
    public void UpdateLineObj(){

        lineRenderer.SetPosition(0,Vertex1.PointObj.transform.position);
        lineRenderer.SetPosition(1,Vertex2.PointObj.transform.position);
    }

    }
public class Circle:Geometry{
    public Vector3 Centre;public float Radius;
    public Vector3 PointA;public Vector3 PointB;public Vector3 PointC;
    public CGA.CGA Circle5D;public CGA.CGA Ic; //plane on which the circle lies
    public Plane PlaneForCircle=new Plane();public int segments=50;
    // member function or method
    public void CreateCircle5DusingThreePoints(){   //or should it be called "Update"?
        CGA.CGA A = up(PointA.x, PointA.y, PointA.z);
        CGA.CGA B = up(PointB.x, PointB.y, PointB.z);
        CGA.CGA C = up(PointC.x, PointC.y, PointC.z);
        Circle5D = A ^ B ^ C;}
    public void UpdateCircleCentre(){   
        CGA.CGA CGAVector2 = down(Circle5D * ei * Circle5D);
        Centre= new Vector3(CGAVector2[1], CGAVector2[2], CGAVector2[3]);
    }
    public void UpdateCircleRadius(){  // in need of Ic
        var Circle5D_star2 = normalise_pnt_minus_one(Circle5D*Ic);
        float CircleRadiusSqr = (Circle5D_star2 * Circle5D_star2)[0];
        Radius= Mathf.Sqrt(Math.Abs(CircleRadiusSqr));
    }
    public void FindIcUsingCircle5D(){
        //find the plane on which the circle lies
        Ic = (ei ^ Circle5D).normalized();
    }
    public void UpdateCircleandCircle5DfromCircleObj(){
        Centre=PlaneForCircle.PlaneGameObj.transform.position;
        Radius=PlaneForCircle.PlaneGameObj.transform.localScale.x; // need to make sure same x,y,z values for localScale    
        PlaneForCircle.GameObjPlaneToPlane5D();
        Ic=PlaneForCircle.Plane5D;
        CGA.CGA SpherebyCircle=Generate5DSpherebyCandRou(Centre, Radius);
        Circle5D=Intersection5D(SpherebyCircle, Ic);
    }


    public void CreateCirclePlaneObjOnScene(){
        //create the plane object on which the circle lie
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
    public CGA.CGA CircletoSphere(){
        FindIcUsingCircle5D();
        var Sphere5D = Circle5D * Ic * I5;
        return Sphere5D;
    }
    public void initialiseCircle(){
        CreateCircle5DusingThreePoints();
        UpdateCircleCentre();
        FindIcUsingCircle5D();
        UpdateCircleRadius();
        CreateCirclePlaneObjOnScene();
        drawCircleOnPlaneOnScene();
        }

}

public class Sphere:Geometry{
    public Vector3 Centre;public float Radius;
    public Vector3 PointA;public Vector3 PointB;public Vector3 PointC;public Vector3 PointD; // the initial defining points may need to be updated later! or just remove them..
    public CGA.CGA Sphere5D; public GameObject SphereObj=GameObject.CreatePrimitive(PrimitiveType.Sphere);

    public void FindSphere5Dfrom4Points(Vector3 a,Vector3 b,Vector3 c,Vector3 d){
        PointA=a;PointB=b;PointC=c;PointD=d;
        var A = up(a.x, a.y, a.z);var B = up(b.x, b.y, b.z);var C = up(c.x, c.y, c.z);var D = up(d.x, d.y, d.z);
        Sphere5D = A ^ B ^ C ^ D;
    }
    public void FindSphere5DbyCandRou(){
        CGA.CGA Centre5D=up(Centre.x, Centre.y, Centre.z);
        Sphere5D= !(Centre5D+(-0.5f)*Radius*Radius*ei);
    }

    public void findSphereCentrefromSphere5D(){
        CGA.CGA CGAVector = Sphere5D * ei * Sphere5D;
		CGA.CGA CGAVector2 = down(CGAVector);
		Centre= new Vector3(CGAVector2[1], CGAVector2[2], CGAVector2[3]);
    }
    public void findSphereRadius(){
        CGA.CGA Sphere5D_nD = normalise_pnt_minus_one(!Sphere5D);
        float SphereRadiusSqr= (Sphere5D_nD * Sphere5D_nD)[0];
        Radius= Mathf.Sqrt(SphereRadiusSqr);
    }
    public void GenerateGameObjSphere(){
        SphereObj.transform.position = Centre;
        SphereObj.transform.localScale = new Vector3(1, 1, 1) * Radius * 2;
    }
    public void UpdateSphereFromObj(){
        Centre=SphereObj.transform.position;
        Radius=SphereObj.transform.localScale.x/2;
        FindSphere5DbyCandRou();
    }

}

public class ClassCircleTry : MonoBehaviour
{   public static Circle Circle1;
    public static Line Line1;
    public static Point IntersectPoint1;
    public static Point IntersectPoint2;
    public static CGA.CGA CircleAndLineIntersect;
    // Start is called before the first frame update
    public static float valve=0.3f;
    void Start()
    {   //intersect line and circle
        Line1=new Line();
        Line1.Vertex1.Point3D=new Vector3(-2f, 1.0f, 7f);
        Line1.Vertex2.Point3D=new Vector3(-2f, 1.0f,3f);
        Line1.SetupVertexObj();
        Line1.UpdateLine5DfromVertices();
        Line1.SetUpLineRenderer();

        Circle1=new Circle();
        Circle1.PointA=new Vector3(-1f, 0, 5f);
        Circle1.PointB=new Vector3(-2f, 1f, 5f);
        Circle1.PointC=new Vector3(-3f, 0, 5f);
        Circle1.initialiseCircle();

        IntersectPoint1=new Point();
        IntersectPoint1.SetupPointObject(0, 1f, 0);
        IntersectPoint2=new Point();
        IntersectPoint2.SetupPointObject(0, 1f, 0);
        
        

        CircleAndLineIntersect=(!((!Line1.Line5D)^(!Circle1.Circle5D)));
        // Debug.Log(CircleAndLineIntersect*CircleAndLineIntersect);
        var mag=CircleAndLineIntersect[1]*CircleAndLineIntersect[1]+CircleAndLineIntersect[2]*CircleAndLineIntersect[2]
        +CircleAndLineIntersect[3]*CircleAndLineIntersect[3]+CircleAndLineIntersect[4]*CircleAndLineIntersect[4]
        +CircleAndLineIntersect[5]*CircleAndLineIntersect[5];
        if (mag<valve){
            //two intersections
            CGA.CGA Sphere1byCircle1=Circle1.CircletoSphere();
            CGA.CGA IntersectPointPair5D = Intersection5D(Sphere1byCircle1, Line1.Line5D);
            IntersectPoint1.PointObj.active = true;
            IntersectPoint2.PointObj.active = true;
            IntersectPoint1.Point5D=ExtractPntAfromPntPairs(IntersectPointPair5D);
            IntersectPoint2.Point5D=ExtractPntBfromPntPairs(IntersectPointPair5D);
            IntersectPoint1.FindPoint3Dfrom5D();
            IntersectPoint2.FindPoint3Dfrom5D();
            IntersectPoint1.UpdatePointObject();
            IntersectPoint2.UpdatePointObject();
        }
        else if (pnt_to_scalar_pnt(CircleAndLineIntersect*CircleAndLineIntersect)<valve){
            //One intersection
            IntersectPoint1.PointObj.active = true;
            IntersectPoint2.PointObj.active = false;
            IntersectPoint1.Point5D=CircleAndLineIntersect;
            IntersectPoint1.FindPoint3Dfrom5D();
            IntersectPoint1.UpdatePointObject();}
            
        else{
            //no intersection points
            IntersectPoint1.PointObj.active = false;
            IntersectPoint2.PointObj.active = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {   Line1.UpdateVerticesFromObject();
        Line1.UpdateLine5DfromVertices();
        Line1.UpdateLineObj();
        Circle1.UpdateCircleandCircle5DfromCircleObj();
        CircleAndLineIntersect=(!((!Line1.Line5D)^(!Circle1.Circle5D))); // without normalising!       
        Debug.Log(CircleAndLineIntersect);

        var mag=CircleAndLineIntersect[1]*CircleAndLineIntersect[1]+CircleAndLineIntersect[2]*CircleAndLineIntersect[2]
                +CircleAndLineIntersect[3]*CircleAndLineIntersect[3]+CircleAndLineIntersect[4]*CircleAndLineIntersect[4]
                +CircleAndLineIntersect[5]*CircleAndLineIntersect[5];
        Debug.Log(mag);
        if (mag<valve){//two intersections
            CGA.CGA Sphere1byCircle1=Circle1.CircletoSphere();
            CGA.CGA IntersectPointPair5D = Intersection5D(Sphere1byCircle1, Line1.Line5D);            
            IntersectPoint1.PointObj.active = true;
            IntersectPoint2.PointObj.active = true;
            IntersectPoint1.Point5D=ExtractPntAfromPntPairs(IntersectPointPair5D);
            IntersectPoint2.Point5D=ExtractPntBfromPntPairs(IntersectPointPair5D);
            IntersectPoint1.FindPoint3Dfrom5D();
            IntersectPoint2.FindPoint3Dfrom5D();
            IntersectPoint1.UpdatePointObject();
            IntersectPoint2.UpdatePointObject();
        }

        else if (Math.Abs(pnt_to_scalar_pnt(CircleAndLineIntersect*CircleAndLineIntersect))<valve/5){
            //One intersection
            IntersectPoint1.PointObj.active = true;
            IntersectPoint2.PointObj.active = false;
            IntersectPoint1.Point5D=CircleAndLineIntersect;
            IntersectPoint1.FindPoint3Dfrom5D();
            IntersectPoint1.UpdatePointObject();
            }
        else{
            IntersectPoint1.PointObj.active = false;
            IntersectPoint2.PointObj.active = false;
        }

    }
}

