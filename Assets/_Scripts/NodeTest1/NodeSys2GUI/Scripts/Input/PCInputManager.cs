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

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GlobalInputDelagates.InvokeBack();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            GlobalInputDelagates.InvokeDelete();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalInputDelagates.InvokeEscape();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GlobalInputDelagates.InvokeMove(-Vector2.one);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GlobalInputDelagates.InvokeSelect();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            GlobalInputDelagates.InvokeMove(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }

        if (Input.GetMouseButtonDown((int)MouseButton.MiddleMouse))
        {
            GlobalInputDelagates.InvokePanStart();
        }

        if (Input.GetMouseButton((int)MouseButton.MiddleMouse))
        {
            GlobalInputDelagates.InvokePan();
        }

    }
}
