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

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift)) && Input.GetKeyDown(KeyCode.A))
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

        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            //doing unity's job for them
            if (!Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    GlobalInputDelagates.InvokeUndo();
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    GlobalInputDelagates.InvokeRedo();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    GlobalInputDelagates.InvokeSave();
                }
            }
            else
            {
                //this is stupid and unity devs are incompetent
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {                    
                    GlobalInputDelagates.InvokeUndo();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    GlobalInputDelagates.InvokeRedo();
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    GlobalInputDelagates.InvokeSave();
                }
            }
        }
    }
}
