using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action keyAction;
    public Action<Define.Mouse, Define.MouseEvent> mouseAction;

    private bool isMouse0Pressed = false;
    private bool isMouse1Pressed = false;
    private bool isMouse2Pressed = false;

    private float mouse0PressedTime = 0f;
    private float mouse1PressedTime = 0f;
    private float mouse2PressedTime = 0f;

    public void OnUpdate()
    {
        if (Input.anyKey && keyAction != null)
            keyAction.Invoke();

        if (mouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if(!isMouse0Pressed)
                {
                    mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Down);
                    mouse0PressedTime = Time.time;
                }

                mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Press);
                isMouse0Pressed = true;
            }
            else
            {
                if (isMouse0Pressed)
                {
                    if (Time.time < mouse0PressedTime + 0.2f)
                        mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Click);

                    mouseAction.Invoke(Define.Mouse.Mouse_0, Define.MouseEvent.Up);
                }
                
                isMouse0Pressed = false;
                mouse0PressedTime = 0f;
            }

            if (Input.GetMouseButton(1))
            {
                if (!isMouse1Pressed)
                {
                    mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Down);
                    mouse1PressedTime = Time.time;
                }

                mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Press);
                isMouse1Pressed = true;
            }
            else
            {
                if (isMouse1Pressed)
                {
                    if (Time.time < mouse1PressedTime + 0.2f)
                        mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Click);

                    mouseAction.Invoke(Define.Mouse.Mouse_1, Define.MouseEvent.Up);
                }

                isMouse1Pressed = false;
                mouse1PressedTime = 0f;
            }

            if (Input.GetMouseButton(2))
            {
                if (!isMouse2Pressed)
                {
                    mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Down);
                    mouse2PressedTime = Time.time;
                }

                mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Press);
                isMouse2Pressed = true;
            }
            else
            {
                if (isMouse2Pressed)
                {
                    if (Time.time < mouse2PressedTime + 0.2f)
                        mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Click);

                    mouseAction.Invoke(Define.Mouse.Mouse_2, Define.MouseEvent.Up);
                }

                isMouse2Pressed = false;
                mouse2PressedTime = 0f;
            }
        }
    }
}
