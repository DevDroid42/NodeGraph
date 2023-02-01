using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EvaluableThreading
{
    public static ColorVec[] ThreadedEvaluateRange(Evaluable data, int subdivisions)
    {
        return ThreadedEvaluateRange(data, subdivisions, 0, 1);
    }

    public static ColorVec[] ThreadedEvaluateRange(Evaluable data, int subdivisions, float start, float end)
    {
        ColorVec[] colors = new ColorVec[subdivisions];
        /*
        Parallel.For(0, subdivisions, pos =>
        {
            float position = (end - (float)start) * ((float)pos / subdivisions);
            colors[pos] = data.EvaluateColor(position);
        });
        */
        var rangePartitioner = Partitioner.Create(0, subdivisions, subdivisions / 2);
        Parallel.ForEach(rangePartitioner, (range, loopState) =>
        {
            Evaluable copiedData = (Evaluable)data.GetCopy();
            //Evaluable copiedData = data;
            // Loop over each range element without a delegate invocation.
            for (int i = range.Item1; i < range.Item2; i++)
            {
                colors[i] = copiedData.EvaluateColor((end - (float)start) * ((float)i / subdivisions));
            }
        });
        return colors;
    }
}
