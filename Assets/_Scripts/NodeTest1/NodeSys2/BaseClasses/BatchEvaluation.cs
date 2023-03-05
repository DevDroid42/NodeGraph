using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BatchEvaluation
{
    private static bool threaded = false;

    /*
     * Returns an array of colors across a set range
     */
    public static ColorVec[] EvaluateColorRange(Evaluable data, int subdivisions, float start = 0, float end = 1)
    {
        ColorVec[] colors = new ColorVec[subdivisions];
        EvaluateColorRangeByRef(colors, data, start, end);
        return colors;
    }

    public static void EvaluateColorRangeByRef(ColorVec[] colors, Evaluable data, float start = 0, float end = 1)
    {
        if (threaded)
        {
            ThreadedEvaluateRange(data, colors.Length, start, end, colors);
        }
        else
        {
            SequentialEvaluateRange(data, colors.Length, start, end, colors);
        }
    }

    private static void SequentialEvaluateRange(Evaluable data, int subdivisions, float start, float end, ColorVec[] colors)
    {
        for (int i = 0; i < subdivisions; i++)
        {
            colors[i] = data.EvaluateColor((float)i / subdivisions-1);
        }
    }

    private static void ThreadedEvaluateRange(Evaluable data, int subdivisions, float start, float end, ColorVec[] colors)
    {
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
    }
}
