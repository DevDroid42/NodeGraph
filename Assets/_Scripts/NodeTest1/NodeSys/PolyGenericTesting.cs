using System;
using UnityEngine;

public abstract class DataM { }

public class Data<T> : DataM
{
    T data;
    public Data(T data)
    {
        this.data = data;
    }

    public T GetData()
    {
        return data;
    }
}

public class CustomDataType
{
    public Color color;
    public CustomDataType()
    {
        color = Color.white;
    }
}

public class PolyGenericTesting : MonoBehaviour
{
    public DataM[] dataArray = { new Data<int>(1),
        new Data<string>("This is test data"),
        new Data<CustomDataType>(new CustomDataType()),
        new Data<float>(0.4f)}; 

    private void Start()
    {
        foreach (DataM data in dataArray)
        {
            switch (data)
            {
                case Data<int> data1:
                    {
                        int test = (int)Convert.ChangeType(data1.GetData(), typeof(int));
                        Debug.Log(test);
                        break;
                    }

                case Data<string> data2:
                    {
                        string test2 = (string)Convert.ChangeType(data2.GetData(), typeof(string));
                        Debug.Log(test2);
                        break;
                    }
                case Data<CustomDataType> data3:
                    {
                        CustomDataType test3 = (CustomDataType)Convert.ChangeType(data3.GetData(), data3.GetType());
                        Debug.Log(test3.color);
                        break;
                    }
                default:
                    {
                        
                        Debug.LogError("Could not down-Cast to any supported dataType");
                        break;
                    }
                    
            }
        }
    }
}
