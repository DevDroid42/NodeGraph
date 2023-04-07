using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetReceivable
{
    void ReceiveData(NetworkMessage message);
}
