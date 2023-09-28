using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Imports:")]
    [SerializeField] Transform bonderie;

    [Header("Camera values:")]
    [SerializeField] private float sensibilityZoom = 0.01f;
    [SerializeField] private float zoomOutMin = 1;
    [SerializeField] private float zoomOutMax = 8;

    private Camera cameraWorld;

    private Vector3 touchStart;
    private Vector2 maxPosClamp; // (x,y)
    private Vector2 minPosClamp; // (-x,-y)

    private bool allowCameraMov;


    // Start is called before the first frame update
    void Start()
    {
        SelectionEvent.instance.onDragBackground += SetDragBackground;
        cameraWorld = Camera.main;
        CalculateMaxMinPos();
    }

    private void SetDragBackground(Vector2 dragPos, bool isDragable)
    {
        touchStart = new Vector3(dragPos.x, dragPos.y, transform.position.z);
        allowCameraMov = isDragable;
    }



    // Update is called once per frame
    void Update()
    {

        if (!allowCameraMov)
            return;



        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * sensibilityZoom);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - cameraWorld.ScreenToWorldPoint(Input.mousePosition);
            cameraWorld.transform.position += direction;
        }
        Zoom(Input.GetAxis("Mouse ScrollWheel") * 3);

        float newValueX = Mathf.Clamp(cameraWorld.transform.position.x, minPosClamp.x, maxPosClamp.x);
        float newValueY = Mathf.Clamp(cameraWorld.transform.position.y, minPosClamp.y, maxPosClamp.y);
        cameraWorld.transform.position = new Vector3(newValueX, newValueY, cameraWorld.transform.position.z);
    }

    private void Zoom(float increment)
    {
        cameraWorld.orthographicSize = Mathf.Clamp(cameraWorld.orthographicSize - increment, zoomOutMin, zoomOutMax);
        CalculateMaxMinPos();
    }
    private void CalculateMaxMinPos()
    {
        Vector2 posBonderie = bonderie.position;
        Vector2 scalBonderie = bonderie.localScale;

        float vertExtent = cameraWorld.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        maxPosClamp = new Vector2(posBonderie.x + (scalBonderie.x / 2 - horzExtent), posBonderie.y + (scalBonderie.y / 2 - vertExtent));
        minPosClamp = new Vector2(posBonderie.x - (scalBonderie.x / 2 - horzExtent), posBonderie.y - (scalBonderie.y / 2 - vertExtent));
    }
}
