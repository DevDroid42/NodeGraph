using jsonTesting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jsonTesting
{
    public class JsonRunner : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            ClassStructure test = new ClassStructure();
            Debug.Log(JsonUtility.ToJson(test));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
