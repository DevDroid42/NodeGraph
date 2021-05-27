using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGroupButtonEnabler : MonoBehaviour
{
    public void SetEnabled(int depth)
    {
        gameObject.SetActive(depth > 0);
    }
}
