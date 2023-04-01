using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BatchEvaluation
{
    private static bool threaded = true;

    /*
     * Returns an array of colors across a set range
     */
    public static ColorVec[] EvaluateColorRange(IEvaluable data, int subdivisions, float start = 0, float end = 1)
    {
        ColorVec[] colors = new ColorVec[subdivisions];
        EvaluateColorRangeByRef(colors, data, start, end);
        return colors;
    }

    public static void EvaluateColorRangeByRef(ColorVec[] colors, IEvaluable data, float start = 0, float end = 1)
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

    private static void SequentialEvaluateRange(IEvaluable data, int subdivisions, float start, float end, ColorVec[] colors)
    {
        float evalRange = (end - (float)start);
        for (int i = 0; i < subdivisions; i++)
        {
            colors[i] = data.EvaluateColor(evalRange * (float)i / (subdivisions - 1));
        }
    }

    private static void ThreadedEvaluateRange(IEvaluable data, int subdivisions, float start, float end, ColorVec[] colors)
    {
        float evalRange = (end - (float)start);
        //break up the work into equal chunks by core count
        int threadRange = subdivisions / (Environment.ProcessorCount * 2); 
        var rangePartitioner = Partitioner.Create(0, subdivisions, threadRange);
        Parallel.ForEach(rangePartitioner, (range, loopState) =>
        {
            //IEvaluable copiedData = (IEvaluable)data.GetCopy();
            //Evaluable copiedData = data;
            // Loop over each range element without a delegate invocation.
            for (int i = range.Item1; i < range.Item2; i++)
            {
                colors[i] = data.EvaluateColor(evalRange * (float)i / (subdivisions - 1));
            }
        });
    }
}
