using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PCInputManager : MonoBehaviour
{
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            GlobalInputDelagates.InvokeScroll(scroll);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalInputDelagates.InvokeOpenMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalInputDelagates.InvokeBack();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GlobalInputDelagates.InvokeMove(Vector2.one);
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            GlobalInputDelagates.InvokeMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }
        
        if (Input.GetMouseButton((int)MouseButton.MiddleMouse))
        {
            GlobalInputDelagates.InvokePan();
        }

    }
}
