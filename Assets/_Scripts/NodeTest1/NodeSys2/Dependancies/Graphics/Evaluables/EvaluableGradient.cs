using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluableGradient : IEvaluable
{
    class Key
    {
        public float position;
        public ColorVec color;

        public Key(float position, ColorVec color)
        {
            this.position = position;
            this.color = color;
        }

        public Key()
        {
            this.position = 0;
            this.color = 0;
        }

        public override string ToString()
        {
            return position.ToString() + "," + color.ToString();
        }
    }
    
    //used to access the keys by index
    [JsonProperty] private List<Key> keys = new List<Key>();
    //used to generate the colors
    [JsonProperty] private List<Key> keysSorted = new List<Key>();

    //brightness mutliplier
    public EvaluableColorTable.InterpolationType interType = EvaluableColorTable.InterpolationType.linear;

    public EvaluableGradient(int keyCount)
    {
        for (int i = 0; i < keyCount; i++)
        {
            AddKey(0, 0);
        }
    }

    public void SetKeyColor(int index, ColorVec color)
    {
        keys[index].color = color;
    }

    private void Sort()
    {
        keysSorted.Sort((a, b) => a.position.CompareTo(b.position));
    }

    public void SetKeyPositon(int index, float position)
    {
        keys[index].position = position;
        Sort();
    }

    public void AddKey(float position, ColorVec color)
    {
        Key newKey = new Key(position, color);
        keys.Add(newKey);
        keysSorted.Add(newKey);
        Sort();
    }

    /*
    public void RemoveKey(int index)
    {
        Key keyToRemove = keys[index];
        keys.Remove(keyToRemove);
        keysSorted.Remove(keyToRemove);
        Sort();
    }
    */

    public int GetkeyAmt()
    {
        return keys.Count;
    }

    public float GetKeyPosition(int i)
    {
        return keys[i].position;
    }

    public ColorVec GetKeyColor(int i)
    {
        return keys[i].color;
    }


    private int KeyCompare(Key k1, Key k2, float pos)
    {
        if(k1.position < pos && k2.position < pos)
        {
            return 1;
        }
        else if(k1.position > pos && k2.position > pos)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    //binary search for the two keys that are at the specificed location
    private (Key, Key) GetKeysAtLocation(float loc, int currentPos, int currentSpeed, int levelsDeep = 0)
    {
        if (levelsDeep > keys.Count)
        {
            Debug.LogWarning("couldn't find key combo");
            return (new Key(), new Key());
        }
        int result = KeyCompare(keysSorted[currentPos], keysSorted[currentPos + 1], loc);
        if (result == 0)
        {
            return (keysSorted[currentPos], keysSorted[currentPos + 1]);
        }

        currentSpeed = Mathf.Max((int)(Mathf.Abs(currentSpeed * 0.5f)), 1);
        if (result < 0) // result is to the left
        {
            return GetKeysAtLocation(loc, currentPos - currentSpeed, currentSpeed, levelsDeep + 1);
        }
        else // result is to the right
        {
            return GetKeysAtLocation(loc, currentPos + currentSpeed, currentSpeed, levelsDeep + 1);
        }
    }

    private ColorVec Interpolate(float x)
    {
        if (keys.Count == 1)
        {
            return keysSorted[0].color;
        }
        if(x < keysSorted[0].position)
        {
            return keysSorted[0].color;
        }
        if(x > keysSorted[keysSorted.Count - 1].position)
        {
            return keysSorted[keysSorted.Count - 1].color;
        }

        if (x == 1)
        {
            x = 0.99999f;
        }
        else if (x == 0)
        {
            x = 0.00001f;
        }

        (Key k1, Key k2) = GetKeysAtLocation(x, (keysSorted.Count-1) / 2, Mathf.Max(keysSorted.Count / 4, 1));
        ColorVec clr1 = k1.color, clr2 = k2.color;
        // percentage of way x is between k1 and k2
        float g = (x - k1.position) / (k2.position - k1.position);

        switch (interType)
        {
            case EvaluableColorTable.InterpolationType.linear:
                return ColorOperations.lerp(clr1, clr2, g);
            case EvaluableColorTable.InterpolationType.closest:
                if (g < 0.5)
                {
                    return clr1;
                }
                else
                {
                    return clr2;
                }

            default:
                Debug.Log("Error=====Invalid Interpolation Type======Error");
                return new ColorVec(0, 0, 255);
        }
    }

    public ColorVec EvaluateColor(float vector)
    {
        return Interpolate(vector);
    }

    public float EvaluateValue(float vector)
    {
        return (float)EvaluateColor(vector);
    }

    public object GetCopy()
    {
        EvaluableGradient gradient = new EvaluableGradient(0);
        foreach (Key key in keys)
        {
            gradient.AddKey(key.position, key.color);
        }
        return gradient;
    }

    public int GetResolution()
    {
        return keys.Count;
    }

}
