using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUI_DebugLog : MonoBehaviour
{
    uint qsize = 10;  // number of messages to keep
    Queue myLogQueue = new Queue();
    GUIStyle myStyle=new GUIStyle();
  
   
    void Start()
    {
        myStyle.fontSize = 22;
        myStyle.normal.textColor = Color.white;
        Debug.Log("Started up logging.");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }
    void OnGUI()
    {
        
        GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height),myStyle);
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(50, 50, 500, 400 ), "Spawn aircraft Key: Alpha0\n" +
            "Destroy aircraft Key: B\n\n" +
            "When I use mouse \n"+
            "Menu buttons disables :) ", myStyle);

       
        

        GUILayout.EndArea();
    }


}
