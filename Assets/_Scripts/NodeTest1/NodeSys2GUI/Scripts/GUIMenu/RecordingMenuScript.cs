using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;
using UnityEngine.UI;

public class RecordingMenuScript : MonoBehaviour
{
    public Text statusText;

    public void Start()
    {
        StartCoroutine(UpdateStatusText());
    }

    public void Save()
    {
        Graph.recorder.SaveRecordings();
    }

    public void ClearFrames()
    {
        Graph.recorder.ClearRecordings();
    }

    public void SetRecording(bool status)
    {
        foreach (RecordingNode node in Graph.globalNodeCollection.GetRecordingNodes())
        {
            node.SetRecording(status);
        }
    }

    IEnumerator UpdateStatusText()
    {
        while (true)
        {
            string txt = GetRecordingNodeCount() + " nodes recording, " + Graph.recorder.FramesInMemory + " frames in memory";
            statusText.text = txt;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private int GetRecordingNodeCount()
    {
        int count = 0;
        foreach (RecordingNode node in Graph.globalNodeCollection.GetRecordingNodes())
        {
            if (node.Recording)
            {
                count += 1;
            }
        }
        return count;
    }
}
