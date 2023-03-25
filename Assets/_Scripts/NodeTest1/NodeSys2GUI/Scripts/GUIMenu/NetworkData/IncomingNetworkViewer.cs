using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nodeSys2;

public class IncomingNetworkViewer : MonoBehaviour
{

    public GameObject template;
    private List<NetworkMessageGUI> messages = new List<NetworkMessageGUI>();
    // Start is called before the first frame update
    void Start()
    {
        NodeNetReceive.AddMethodToNetReceiveDelegate(UpdateEntries);
    }

    private void UpdateEntries(NetworkMessage message)
    {
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i].GetMessage().CompareHeader(message))
            {
                messages[i].UpdateNetworkMessage(message);
                //messages[i].gameObject.transform.SetAsFirstSibling();
                return;
            }
        }
        NetworkMessageGUI tempGuiMsg = Instantiate(template, transform).GetComponent<NetworkMessageGUI>();
        tempGuiMsg.gameObject.SetActive(true);
        tempGuiMsg.UpdateNetworkMessage(message);
        messages.Add(tempGuiMsg);
    }

    public void ClearMessages()
    {
        for (int i = 0; i < messages.Count; i++)
        {
            Destroy(messages[i].gameObject);
        }
        messages.Clear();
    }
}