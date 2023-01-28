using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using nodeSys2;
using System.Collections;
using System.Collections.Generic;

//handles conversion of evaluable Types to an editor
public class EditorTypeManagement
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Editor { nonEvaluable, boolean, number, color, table }

    //The default editor to display for different basic Evaluable types
    public static Editor GetEditorByType(Object data)
    {
        if(!(data is Evaluable)) return Editor.nonEvaluable;

        switch (data)
        {
            case EvaluableBool ebool:
                {
                    return Editor.boolean;
                }
            case EvaluableFloat efloat:
                {
                    return Editor.number;
                }
            case EvaluableColorVec ecolor:
                {
                    return Editor.color;
                }
            default:
                {
                    return Editor.table;
                }
        }
    }
}
