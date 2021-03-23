using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform myTransform;
    private Vector3 offset = new Vector3(0, 1.0f, -1.0f);

    public Transform target;

    [SerializeField] private LayerMask checkLayer;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float currentZoom;
    [SerializeField] private int minZoom;
    [SerializeField] private int maxZoom;
    [SerializeField] private float minPositionY;
    [SerializeField] private float maxPositionY;

    private void Start()
    {
        myTransform = GetComponent<Transform>();

        Managers.Input.mouseAction -= OnMouse1Pressed;
        Managers.Input.mouseAction += OnMouse1Pressed;
    }

    private void OnDisable()
    {
        if (Managers.instance != null)
            Managers.Input.mouseAction -= OnMouse1Pressed;
    }

    private void LateUpdate()
    {
        CameraZoom();
        ClampPositionY();
    }

    private void CameraRotation()
    {
        float rotX = Input.GetAxis("Mouse Y") * lookSensitivity;
        float rotY = Input.GetAxis("Mouse X") * lookSensitivity;

        myTransform.RotateAround(target.position, Vector3.right, rotX);
        myTransform.RotateAround(target.position, Vector3.up, rotY);

        offset = myTransform.position - target.position;
        offset.Normalize();
    }

    private void CameraZoom()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        if (Physics.Raycast(target.transform.position, offset, out RaycastHit hit, maxZoom, checkLayer))
        {
            float newZoom = (hit.point - target.transform.position).magnitude * 0.5f;
            currentZoom = Mathf.Lerp(currentZoom, newZoom, Time.deltaTime * zoomSpeed);
        }
    }

    private void ClampPositionY()
    {
        Vector3 newPos = target.position + offset * currentZoom;

        if (newPos.y < minPositionY)
            newPos.y = minPositionY;
        if (newPos.y > maxPositionY * (currentZoom / maxZoom))
            newPos.y = maxPositionY * (currentZoom / maxZoom);

        myTransform.position = newPos;
        myTransform.LookAt(target);
    }

    private void OnMouse1Pressed(Define.Mouse mouse, Define.MouseEvent evt)
    {
        if (mouse != Define.Mouse.Mouse_1 || evt != Define.MouseEvent.Press)
            return;

        CameraRotation();
    }
}
