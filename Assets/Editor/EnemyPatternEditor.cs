
using System;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyPattern))]
public class EnemyPatternEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyPattern pattern = (EnemyPattern)target;
        if(pattern)
        {
            UpdatePreview(pattern);
            ProccessInput(pattern);
        }
    }
    void UpdatePreview(EnemyPattern pattern)
    {
        Vector2 endOfLastStep= pattern.transform.position;
        foreach (EnemyStep step in pattern.steps)
        {
            switch (step.movement)
            {
                case EnemyStep.MovementType.direction:
                    {
                        Handles.DrawDottedLine(endOfLastStep, endOfLastStep + step.direction, 1);
                        endOfLastStep = endOfLastStep + step.direction;
                        break;
                    }
                    case EnemyStep.MovementType.spline:
                    {
                        endOfLastStep =DrawSpline(step.spline, endOfLastStep,step.movementSpeed);
                        break;

                    }
            };
        }
    }
    void ProccessInput(EnemyPattern pattern)
    {
        Event guiEvent=Event.current;
        Vector2 mousePos =HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Spline path = pattern.steps[0].spline;
            Vector2 offset=pattern.transform.position;
            path.AddSegment(mousePos-offset);
            path.CalculatePoints(pattern.steps[0].movementSpeed);
        }
    }
    Vector2 DrawSpline(Spline spline,Vector2 endOfLastStep,float speed)
    {  // draw controll handle lines
        Handles.color = Color.black;
        for (int s = 0; s < spline.NoOfSegMents(); s++)
        {
            Vector2[] points = spline.GetSegmentPoints(s,endOfLastStep);
            Handles.DrawLine(points[0], points[1]);
            Handles.DrawLine(points[2], points[3]);
        }
        // drawsplineLine
        Handles.color = Color.white;
        for (int p = 1;p<spline.linearPoints.Count;p++)
        {
            Handles.CircleHandleCap(0, spline.linearPoints[p]+endOfLastStep,Quaternion.identity,0.5f,EventType.Repaint);
        }

        //Draw control Handles
        Handles.color = Color.green;
        for(int point = 0; point < spline.controlPoints.Count; point++)
        {
            Vector2 pos =spline.controlPoints[point]+endOfLastStep;
            float size = 1f;
            if (point % 3 == 0) size = 2f;
            
            Vector2 newPos =  Handles.FreeMoveHandle(pos, Quaternion.identity, size,Vector2.zero, Handles.CylinderHandleCap);

            if (point > 0 && pos != newPos)
            {
                spline.SetPoint(point,newPos-endOfLastStep);
                spline.CalculatePoints(speed);
            }
        }
        Handles.color = Color.white;
        return endOfLastStep;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EnemyPattern pattern = (EnemyPattern)target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Stationary"))
        {

            pattern.AddStep(EnemyStep.MovementType.none);
        }
        if (GUILayout.Button("Add Directional"))
        {
            pattern.AddStep(EnemyStep.MovementType.direction);


        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Add Homing"))
        {
            pattern.AddStep(EnemyStep.MovementType.homing);


        }
        if (GUILayout.Button("Add Spline"))
        {
            pattern.AddStep(EnemyStep.MovementType.spline);


        }
        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();
    }
}
