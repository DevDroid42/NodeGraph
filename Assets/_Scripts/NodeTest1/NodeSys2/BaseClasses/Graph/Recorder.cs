using SFB;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


public class Recorder
{
    class Recording
    {
        private Stopwatch timer = new Stopwatch();
        //stores a tuple containing millesecond and frame
        public List<(long, ColorVec[])> frames = new List<(long, ColorVec[])>();
        public Recording()
        {
            timer.Start();
        }

        public void RecordFrame(ColorVec[] frame)
        {
            frames.Add((timer.ElapsedMilliseconds, frame));
        }
    }

    public int FramesInMemory
    {
        get; private set;
    }
    private Dictionary<string, Recording> records = new Dictionary<string, Recording>();

    public void RecordFrame(string name, ColorVec[] frame)
    {
        FramesInMemory += 1;
        if (!records.ContainsKey(name))
        {
            records[name] = new Recording();

        }
        records[name].RecordFrame(frame);
    }

    public void ClearRecordings()
    {
        FramesInMemory = 0;
        records.Clear();
    }

    public void SaveRecordings()
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Save Recordings", "", "", "txt");
        if (path == "") return;
        StringBuilder sBuilder = new StringBuilder();
        foreach (string key in records.Keys)
        {
            sBuilder.Append("-");
            sBuilder.Append(key);
            GenerateSaveData(sBuilder, records[key]);
        }
        try
        {
            File.WriteAllText(path, sBuilder.ToString());
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogWarning("Error Writing File: " + e.ToString());
        }
    }

    private void GenerateSaveData(StringBuilder sBuilder, Recording recording)
    {
        foreach ((long, ColorVec[]) frame in recording.frames)
        {
            sBuilder.Append(frame.Item1);
            sBuilder.Append(",");
            foreach (ColorVec color in frame.Item2)
            {
                byte[] components = new byte[3];
                for (int i = 0; i < 3; i++)
                {
                    components[i] = (byte)(ColorOperations.ClampColor(color).GetComponent(i) * 255);
                }
                sBuilder.Append(BitConverter.ToString(components).Replace("-", ""));
            }
            sBuilder.Append("\n");
        }
    }
}
