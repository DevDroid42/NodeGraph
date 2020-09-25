using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class GlobalData
{
    private List<string> recentProjects = new List<string>();

    public List<string> GetRecentlyOpened()
    {
        return recentProjects;
    }
    public void AddRecentlyOpened(string path)
    {
        recentProjects.Insert(0, path);
        if(recentProjects.Count > 5)
        {
            recentProjects.RemoveAt(recentProjects.Count - 1);
        }
    }

}
