
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(EnemyPattern))]
public class EnemyPatternEditor : Editor
{
    Mesh previewMesh = null;

    float editorDeltaTime = 0;
    float timer = 0;
    double lastTimeSinceStartUp = 0;
    private void OnEnable()
    {
        if (previewMesh == null)
        {
            previewMesh = new Mesh();
            Vector3[] verts = new Vector3[4];
            Vector2[] uvs = new Vector2[4];
            int[] tris = new int[6];
            const float halfSize = 8f;
            verts[0] = new Vector3(-halfSize, halfSize);
            verts[1] = new Vector3(halfSize, halfSize);
            verts[2] = new Vector3(-halfSize, -halfSize);
            verts[3] = new Vector3(halfSize, -halfSize);

            uvs[0] = new Vector2(0, 1);
            uvs[1] = new Vector2(1, 1);
            uvs[2] = new Vector2(0, 0);
            uvs[3] = new Vector2(1, 0);

            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;
            tris[3] = 2;
            tris[4] = 1;
            tris[5] = 3;

            previewMesh.vertices=verts;
            previewMesh.uv = uvs;
            previewMesh.triangles=tris;
        }
    }

    [System.Obsolete]
    private void OnSceneGUI()
    {
        UpdateEditorTime();
        EnemyPattern pattern = (EnemyPattern)target;
        if(pattern)
        {
            UpdatePreview(pattern);
            ProccessInput(pattern);

            //Force Scene to repaint
            if(Event.current.type == EventType.Repaint)
            {
                SceneView.RepaintAll();
            }
        }
       
    }
    void UpdateEditorTime()
    {
        if (lastTimeSinceStartUp == 0)
        {
            lastTimeSinceStartUp = EditorApplication.timeSinceStartup;
        }
        editorDeltaTime = (float)(EditorApplication.timeSinceStartup - lastTimeSinceStartUp) * 60f;
        lastTimeSinceStartUp = EditorApplication.timeSinceStartup;
    }

    [System.Obsolete]
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
                    case EnemyStep.MovementType.homing:
                    {
                        if (GameManager.Instance && GameManager.Instance.playerCrafts[0])
                        {
                            Handles.DrawDottedLine(endOfLastStep, GameManager.Instance.playerCrafts[0].transform.position, 1);
                            endOfLastStep = GameManager.Instance.playerCrafts[0].transform.position;
                        }
                        break;
                    }
            };
        }
        //Draw animated Preview
        timer += editorDeltaTime;
        if (timer >= pattern.TotalTime())
            timer = 0;

        SpriteRenderer renderer=pattern.enemyPrefab.GetComponentInChildren<SpriteRenderer>();
        if (renderer)
        {
            Texture texture=renderer.sprite.texture;
            Material mat = renderer.sharedMaterial;

            Vector3 pos = pattern.CalculatePosition(timer);
            Matrix4x4 transMat = Matrix4x4.Translate(pos);

            Quaternion rot= pattern.CalculateRotation(timer);
            Matrix4x4 rotMat=Matrix4x4.Rotate(rot);  

            Matrix4x4 matrix=transMat*rotMat;

            mat.SetPass(0);
            Graphics.DrawMeshNow(previewMesh, matrix);
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

    [System.Obsolete]
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
