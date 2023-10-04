using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

[Serializable]
public class Finger
{
    const int NULL_ID = -10;

    public int id;
    public bool isDrag;

    public EnumClikable typeClick;
    public Vector2 lastPos;
    public MonsterScript monsterSelected;

    public Finger(int newId, Vector2 screenPos)
    {
        id = newId;
        lastPos = screenPos;
        typeClick = EnumClikable.Other;
        isDrag = false;
    }
}

public class SelectionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI log;

    private const string TAG_CLICK = "Clickable";
    private const string TAG_BACK = "Background";
    private const int MAX_FINGERS = 3;
    private float DRAG_SENSIBILITY = 1.5f;

    private SelectionEvent instance;
    private List<Finger> fingers;

    private void Start()
    {
        instance = SelectionEvent.instance;
        fingers = new List<Finger>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Verification: Finger not already pressed or too many fingers
        Finger fingerDown = getFingerFromList(eventData.pointerId);
        if (fingerDown != null || fingers.Count >= MAX_FINGERS) { return; }

        RaycastHit2D[] hits = GetRaycast(eventData);

        Collider2D colliderClick = GetFirstClickable(hits);
        Collider2D colliderBack = GetFirstBack(hits);
        Finger newFingerPressed = new Finger(eventData.pointerId, eventData.position);

        if (colliderClick != null)
        {
            MonsterScript lastMonsterScript = colliderClick.gameObject.GetComponent<MonsterScript>();

            newFingerPressed.monsterSelected = lastMonsterScript;
            newFingerPressed.typeClick = EnumClikable.Monster;
        }
        else if (colliderBack != null)
        {
            instance.SetDragBackground(eventData.pointerId, true);
            newFingerPressed.typeClick = EnumClikable.Background;
        }

        fingers.Add(newFingerPressed);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Verification: Finger is already pressed
        Finger fingerDown = getFingerFromList(eventData.pointerId);
        if (fingerDown == null) { return; }

        //Verification dragSensibility
        if (isInThreeshold(eventData.position, fingerDown.lastPos)) { return; }

        switch (fingerDown.typeClick)
        {
            case EnumClikable.Monster:
                //FIRST TIME DRAG MONSTER:
                if (!fingerDown.isDrag)
                    instance.SetDragMonster(fingerDown.monsterSelected, eventData.pointerId, true);

                //DRAG THE MONSTER:
                instance.DragMonster(eventData.position, fingerDown.id);
                break;
            case EnumClikable.Background:
                //MOVE THE BACKGROUND (CAMERA)
                instance.DragBackground(fingerDown.id);
                print("DRAGG: (finger: " + fingerDown.id + "; delta: " + (eventData.position - fingerDown.lastPos) + ")");
                break;
            case EnumClikable.Other:
                break;
            default:
                break;
        }

        fingerDown.lastPos = eventData.position;
        fingerDown.isDrag = true;
    }

    private void PointerExitUp(PointerEventData eventData)
    {
        // Verification: Finger is already pressed
        Finger fingerUp = getFingerFromList(eventData.pointerId);
        if (fingerUp == null) { return; }

        switch (fingerUp.typeClick)
        {
            case EnumClikable.Monster:
                //SELECTION MONSTER (end)
                if (!fingerUp.isDrag)
                    instance.SelectionMonster(fingerUp.monsterSelected);
                //DRAG MONSTER (end)
                else
                    instance.SetDragMonster(fingerUp.monsterSelected, fingerUp.id, false);
                break;

            case EnumClikable.Background:
                instance.SetDragBackground(fingerUp.id, false);
                break;

            case EnumClikable.Other:
                break;
        }

        fingers.Remove(fingerUp);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerExitUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitUp(eventData);
    }

    //PRIVATE LOGIC METHODES:
    private Finger getFingerFromList(int newFingerID)
    {
        foreach (Finger item in fingers)
        {
            if (item.id == newFingerID)
                return item;
        }
        return null;
    }

    private RaycastHit2D[] GetRaycast(PointerEventData eventData)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        return Physics2D.RaycastAll(mousePos2D, Vector2.zero);
    }

    private Collider2D GetFirstClickable(RaycastHit2D[] raycastHits)
    {
        foreach (RaycastHit2D item in raycastHits)
        {
            if (item.collider != null && item.collider.gameObject.tag == TAG_CLICK)
                return item.collider;

        }
        return null;
    }

    private Collider2D GetFirstBack(RaycastHit2D[] raycastHits)
    {
        foreach (RaycastHit2D item in raycastHits)
        {
            if (item.collider != null && item.collider.gameObject.tag == TAG_BACK)
                return item.collider;

        }
        return null;
    }

    private bool isInThreeshold(Vector2 newPosition, Vector2 oldPosition)
    {
        return (MathF.Abs(newPosition.x - oldPosition.x) < DRAG_SENSIBILITY)
            && (MathF.Abs(newPosition.y - oldPosition.y) < DRAG_SENSIBILITY);
    }

    private void printMobile(string text)
    {
        log.text = text + "   \n" + log.text;
    }
}
