using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScreenPoint
{
    public int id;

    public ScreenPoint(int newId)
    {
        id = newId;
    }
}

public class CameraMovement : MonoBehaviour
{
    [Header("Imports:")]
    [SerializeField] Transform bonderie;

    [Header("Camera values:")]
    [SerializeField] private float sensibilityZoom = 0.01f;
    [SerializeField] private float zoomOutMin = 1;
    [SerializeField] private float zoomOutMax = 8;

    private Camera cameraWorld;

    private Vector2 maxPosClamp; // (x,y)
    private Vector2 minPosClamp; // (-x,-y)


    //NEW THINGS
    private const int MAX_FNGERS_CAM = 2;
    private Vector2 lastCenterPoint;
    private float lastMagnitude;
    private List<ScreenPoint> currentPoints;


    void Start()
    {
        SelectionEvent.instance.onSetDragBackground += SetDragBackground;
        SelectionEvent.instance.onDragBackground += DragBackgroud;

        cameraWorld = Camera.main;
        currentPoints = new List<ScreenPoint>();

        CalculateMaxMinPos();
    }

    private void SetDragBackground(int id, bool isAdded)
    {
        if (isAdded)
        {
            if (currentPoints.Count >= MAX_FNGERS_CAM || currentPoints.Contains(getPointByID(id))) { return; }
            currentPoints.Add(new ScreenPoint(id));
            lastCenterPoint = getCenterPosition(getFingerPosByID(id));
        }
        else
        {
            if (currentPoints.Count <= 0 || !currentPoints.Contains(getPointByID(id))) { return; }
            currentPoints.Remove(getPointByID(id));
            lastCenterPoint = getCenterPosition(getFingerPosByID(id), false);
        }

        if (currentPoints.Count == 2)
            lastMagnitude = Vector2.Distance(getFingerPosByID(currentPoints[0].id), getFingerPosByID(currentPoints[1].id));
    }

    private void DragBackgroud(int id)
    {
        if (!currentPoints.Contains(getPointByID(id))) { return; }
        
        Vector2 newCenterPoint = new Vector2();
        Vector2 posFinger_1 = getFingerPosByID(currentPoints[0].id);
      
        switch (currentPoints.Count)
        {
            case 2:
                Vector2 posFinger_2 = getFingerPosByID(currentPoints[1].id);
                float newMagnitude = Vector2.Distance(posFinger_1, posFinger_2);
                newCenterPoint = getCenterPosition(posFinger_1);

                Zoom((newMagnitude - lastMagnitude) * sensibilityZoom);
                break;

            case 1:
                newCenterPoint = getCenterPosition(posFinger_1);
                break;
        }

        Vector2 direction = lastCenterPoint - newCenterPoint;
        float newValueX = Mathf.Clamp(cameraWorld.transform.position.x + direction.x, minPosClamp.x, maxPosClamp.x);
        float newValueY = Mathf.Clamp(cameraWorld.transform.position.y + direction.y, minPosClamp.y, maxPosClamp.y);

        cameraWorld.transform.position = new Vector3(newValueX, newValueY, cameraWorld.transform.position.z);
    }
    private void OnDrawGizmos()
    {
        if (currentPoints == null)
            return;

        Gizmos.color = Color.red;

        if (currentPoints.Count >= 1)
            Gizmos.DrawSphere(getFingerPosByID(currentPoints[0].id), 0.1f);
        if (currentPoints.Count >= 2)
            Gizmos.DrawSphere(getFingerPosByID(currentPoints[1].id), 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastCenterPoint, 0.1f);
    }

    // PRIVATE LOGIC METHODES
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

    private Vector2 calculateCenter(Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }

    private Vector2 getCenterPosition(Vector2 dragPos, bool isAdded = true)
    {
        Vector2 centerPoint = new Vector2();

        switch ((currentPoints.Count, isAdded))
        {
            case (2, true):
                centerPoint = calculateCenter(getFingerPosByID(currentPoints[0].id), getFingerPosByID(currentPoints[1].id));
                break;
            case (1, false):
                centerPoint = getFingerPosByID(currentPoints[0].id);
                break;
            case (1, true):
                centerPoint = dragPos;
                break;
            case (0, false):
                // NOTHING NEEDS TO BE DONE
                break;
            default:
                Debug.LogError("DragPos: " + dragPos + " is added; " + isAdded + "nb fingers: " + currentPoints.Count);
                break;
        }

        return centerPoint;
    }

    private ScreenPoint getPointByID(int id)
    {
        foreach (ScreenPoint item in currentPoints)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }
    private Vector2 getFingerPosByID(int id)
    {
        return cameraWorld.ScreenToWorldPoint(Input.GetTouch(id).position);
    }
}
