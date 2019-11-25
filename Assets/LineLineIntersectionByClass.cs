using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
using static ClassCircleTry;

public class LineLineIntersectionByClass : MonoBehaviour
{
    public static Line Line1;
    public static Line Line2;
    public static Point LineLineInersectPoint1;
    // Start is called before the first frame update
    public static CGA.CGA LLInersectPoint1_5D;
    public static float valve =0.3f;
    void Start()
    {
        Line1=new Line();
        Line1.SetUpVertices3Dand5D(new Vector3(4f, 11f, 2f),new Vector3(4f, 11f,-2f));
        Line1.SetupVertexObj();
        Line1.FindLine5DfromVertices5D();
        Line1.SetUpLineRenderer();

        Line2=new Line();
        Line2.SetUpVertices3Dand5D(new Vector3(4f, 13f, 1f),new Vector3(4f, 7f,1f));
        Line2.SetupVertexObj();
        Line2.FindLine5DfromVertices5D();
        Line2.SetUpLineRenderer();

        LineLineInersectPoint1=new Point();
        LineLineInersectPoint1.SetupPointObject(0, 1f, 0);

        LLInersectPoint1_5D=(!((!Line1.Line5D)^(!Line2.Line5D)));
        
        // var mag=LLInersectPoint1_5D[1]*LLInersectPoint1_5D[1]+LLInersectPoint1_5D[2]*LLInersectPoint1_5D[2]
        // +LLInersectPoint1_5D[3]*LLInersectPoint1_5D[3]+LLInersectPoint1_5D[4]*LLInersectPoint1_5D[4]
        // +LLInersectPoint1_5D[5]*LLInersectPoint1_5D[5];



        // if (mag<valve){
            LineLineInersectPoint1.Point5D=MidPointBetweenLines(Line1.Line5D,Line2.Line5D);
            LineLineInersectPoint1.FindPoint3Dfrom5D();
            LineLineInersectPoint1.UpdatePointObject();
            LineLineInersectPoint1.PointObj.active = true;
        // }
        // else{
        //     LineLineInersectPoint1.PointObj.active = false;
        // }


    }

    // Update is called once per frame
    void Update()
    {
        Line1.UpdateVerticesFromObject();
        Line1.UpdateLine5DfromVertices();
        Line1.UpdateLineObj();

        Line2.UpdateVerticesFromObject();
        Line2.UpdateLine5DfromVertices();
        Line2.UpdateLineObj();


        LLInersectPoint1_5D=(!((!Line1.Line5D)^(!Line2.Line5D)));
        // Debug.Log(LLInersectPoint1_5D*LLInersectPoint1_5D);
        // var mag=LLInersectPoint1_5D[1]*LLInersectPoint1_5D[1]+LLInersectPoint1_5D[2]*LLInersectPoint1_5D[2]
        // +LLInersectPoint1_5D[3]*LLInersectPoint1_5D[3]+LLInersectPoint1_5D[4]*LLInersectPoint1_5D[4]
        // +LLInersectPoint1_5D[5]*LLInersectPoint1_5D[5];


        // if (mag<valve){
            LineLineInersectPoint1.Point5D=MidPointBetweenLines(Line1.Line5D,Line2.Line5D);
            var simplifiedPoint5D=LineLineInersectPoint1.Point5D[1]*e1+LineLineInersectPoint1.Point5D[2]*e2+LineLineInersectPoint1.Point5D[3]*e3
                                +LineLineInersectPoint1.Point5D[4]*e4+LineLineInersectPoint1.Point5D[5]*e5;
            LineLineInersectPoint1.Point5D=simplifiedPoint5D;
            LineLineInersectPoint1.FindPoint3Dfrom5D();
            LineLineInersectPoint1.UpdatePointObject();
            LineLineInersectPoint1.PointObj.active = true;

            Debug.Log(Line1.Line5D);
            Debug.Log(Line2.Line5D);
            Debug.Log(LineLineInersectPoint1.Point5D);
        // }
        // else{
        //     LineLineInersectPoint1.PointObj.active = false;
        // }


    }

    }
