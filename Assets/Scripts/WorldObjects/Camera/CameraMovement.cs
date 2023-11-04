using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

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
    [SerializeField] private int MAX_FNGERS_CAM = 2;

    [Header("Sliding values:")]
    [SerializeField] private float k1 = 2f;
    [SerializeField] private float c = 2f;
    [SerializeField] private float f = 8f;

    [Header("Zooming values:")]
    [SerializeField] private float differenceZoom = 2f;
    [SerializeField] private float speedZoom = 2f;



    private List<ScreenPoint> currentPoints;
    private FixedQueue<int> unResolvedIndex;
    private Vector2 lastCenterPoint;
    private float lastMagnitude;

    private Vector2 maxPosClamp; // (x,y)
    private Vector2 minPosClamp; // (-x,-y)

    //Reference vers instance:
    private Camera cameraWorld;
    private PrefabHolder prefabHolder;
    private ArenaEvent arenaEvent;


    void Start()
    {
        SelectionEvent.instance.onSetDragBackground += SetDragBackground;
        SelectionEvent.instance.onDragBackground += DragBackgroud;
        ArenaEvent.instance.onChangeArena += centerAroundCamera;

        cameraWorld = Camera.main;
        prefabHolder = PrefabHolder.instance;
        arenaEvent = ArenaEvent.instance;

        currentPoints = new List<ScreenPoint>();
        unResolvedIndex = new FixedQueue<int>(3);

        centerAroundCamera(0, false);
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
    private void centerAroundCamera(int newArenaIndex, bool callback)
    {
        if (callback) { return; }

        // Add to buffer and stop until the last move is done
        if (!unResolvedIndex.getResetForcedFlag())
        {
            if (!unResolvedIndex.IsEmpty())
            {
                unResolvedIndex.Enqueue(newArenaIndex + 1);
                if (unResolvedIndex.IsFull()) { arenaEvent.BufferFullChangeArena(true); }
                return;
            }
            unResolvedIndex.Enqueue(newArenaIndex + 1);
        }

        // Get container around island (camera bonderie)
        bonderie = prefabHolder.getGameObjectFromArena(newArenaIndex, 0).GetComponent<Transform>();

        // Calculate max and min zoom value;
        float aspectRation = (float)Screen.width / Screen.height;
        float orthoValueInit = (bonderie.localScale.x * 0.5f) / aspectRation;

        zoomOutMax = orthoValueInit;

       // Throw IEnumerator functions
        IEnumerator coroutineTransl = incrementTranslation(bonderie.position.x, cameraWorld.transform.position, newArenaIndex);
        if (!Utils.countainsInterval(cameraWorld.orthographicSize, orthoValueInit, differenceZoom))
            StartCoroutine(incrementZoom(orthoValueInit, cameraWorld.orthographicSize, coroutineTransl));
        else
            StartCoroutine(coroutineTransl);
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

        if(maxPosClamp.y < minPosClamp.y)
        {
            maxPosClamp.y = (maxPosClamp.y + minPosClamp.y) / 2;
            minPosClamp.y = maxPosClamp.y;
        }
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
   
    private Vector2 getFingerPosByID(int id)
    {
        try
        {
            return cameraWorld.ScreenToWorldPoint(Input.GetTouch(id).position);
        }
        catch (Exception)
        {
            return new Vector2(0, 0);

        }

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

    //IENUMERATOR CHANGING VALUES:
    private IEnumerator incrementZoom(float targetZoomValue, float initialValue, IEnumerator coroutineTransl)
    {
        const float PRECISION = 0.01f;

        float distance = Mathf.Abs(targetZoomValue - initialValue);
        bool isGrowing = (targetZoomValue - initialValue) > 0;

        float func_integral = speedZoom * distance;
        float func_temps = (float)Math.Pow(distance, 2) / func_integral;

        float xVal = 0;
        float tempsVal = 0;
        float newVitesse = 0;

        if (func_temps < 0) { Debug.LogError("func_temps value not supported"); }

        while (xVal < distance - PRECISION)
        {
            xVal = distance * tempsVal / func_temps;

            if (xVal >= 0 && xVal < distance)
                newVitesse = speedZoom;

            else
                break;

            float depVal = isGrowing ? newVitesse * Time.deltaTime : -newVitesse * Time.deltaTime;
            cameraWorld.orthographicSize += depVal;

            tempsVal += Time.deltaTime;

            yield return null;
        }

        cameraWorld.orthographicSize = targetZoomValue;
        StartCoroutine(coroutineTransl);
    }

    private IEnumerator incrementTranslation(float targetXvalue, Vector3 initialvalue, int indexArena)
    {
        const float PRECISION = 0.01f;

        float distance = Mathf.Abs(targetXvalue - initialvalue.x);
        float k2 = distance - 2 * k1;
        bool isRight = (targetXvalue - initialvalue.x) > 0;

        float quad_a = (-c * 4) / Mathf.Pow(k2, 2);
        float quad_h = k1 + (k2 / 2);
        float quad_k = f * k1 + c;
        float linDown_b = 2 * k1 + k2;

        float func_integral = f * Mathf.Pow(k1, 2) + ((3 * f * k1 + 2 * c) * k2) / 3;
        float func_temps = (float)Math.Pow(distance, 2) / func_integral;

        float xVal = 0;
        float tempsVal = 0;
        float newVitesse = 0;

        if (k2 < 0) { Debug.LogError("K2 value not supported"); }
        if (func_temps < 0) { Debug.LogError("func_temps value not supported"); }

        while (xVal < distance - PRECISION)
        {
            xVal = distance * tempsVal / func_temps;

            if (xVal >= 0 && xVal < k1)
                newVitesse = f * xVal;

            else if (xVal >= k1 && xVal < (k1 + k2))
                newVitesse = quad_a * Mathf.Pow(xVal - quad_h, 2) + quad_k;

            else if (xVal >= (k1 + k2) && xVal <= (2 * k1 + k2))
                newVitesse = f * (-xVal + linDown_b);

            else
                break;

            float depVal = isRight ? newVitesse * Time.deltaTime : -newVitesse * Time.deltaTime;
            cameraWorld.transform.position += Vector3.right * depVal;

            tempsVal += Time.deltaTime;

            yield return null;
        }

        cameraWorld.transform.position = new Vector3(targetXvalue, cameraWorld.transform.position.y, cameraWorld.transform.position.z);

        unResolvedIndex.Dequeue();
        if (!unResolvedIndex.IsEmpty())
        {
            unResolvedIndex.setForcedFlag();
            centerAroundCamera(unResolvedIndex.seeFirstElem() - 1, false);
        }

        arenaEvent.ChangeArena(indexArena, callback: true);
        arenaEvent.BufferFullChangeArena(false);

        CalculateMaxMinPos();
    }
}


