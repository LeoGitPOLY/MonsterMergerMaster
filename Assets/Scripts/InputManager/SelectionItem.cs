using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Finger
{
    const int NULL_ID = -10;
    int id;
    Vector2 screenPos;

    Finger(int newId, Vector2 newScreenPos)
    {
        id = newId;
        screenPos = newScreenPos;
    }
}

public class SelectionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI log;

    private const string TAG_CLICK = "Clickable";
    private const string TAG_BACK = "Background";
    private const int NULL_ID = -10;

    private SelectionEvent instance;
    private MonsterScript lastMonsterScript;

    private bool isOnDrag;
    private bool isMonsterClick;
    private bool isBackClick; // Not really important right now
    private int pointerDownID;
    private Finger[] fingers;

    private void Start()
    {
        instance = SelectionEvent.instance;
        isOnDrag = false;
        pointerDownID = NULL_ID;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Verification que "no finger is currently pressed" 
        if (pointerDownID == NULL_ID) { pointerDownID = eventData.pointerId; } else { return; }

        RaycastHit2D[] hits = GetRaycast(eventData);
        Collider2D colliderClick = GetFirstClickable(hits);
        Collider2D colliderBack = GetFirstBack(hits);

        if (colliderClick != null)
        {
            lastMonsterScript = colliderClick.gameObject.GetComponent<MonsterScript>();
            instance.SetDragMonster(lastMonsterScript);

            isMonsterClick = true;
            // printMobile("pointerDownMonster");
        }
        else if (colliderBack != null)
        {
            instance.DragBackground(GetCameraPos2D(eventData.position), true);
            // printMobile("pointerDownBack");
            isBackClick = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Verification que fonctionne seulement sur le premier doigt est drag
        if (pointerDownID != eventData.pointerId) { return; }

        if (isMonsterClick)
        {
            //DRAG THE MONSTER
            instance.DragMonster(GetCameraPos2D(eventData.position));
        }
        else if (isBackClick)
        {
            //MOVE THE BACKGROUND (CAMERA)
        }

        isOnDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerExitUp(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitUp(eventData);
    }

    //PRIVATE FONCTIONS:
    private void PointerExitUp(PointerEventData eventData)
    {
        // Verification que fonctionne seulement sur le premier doigt est exit
        if (pointerDownID != eventData.pointerId) { return; }

        RaycastHit2D[] hits = GetRaycast(eventData);
        Collider2D colliderClick = GetFirstClickable(hits);

        if (colliderClick != null && !isOnDrag)
        {
            //STATE: Le doigt est leve sans avoir ete drag (selection d'un monstre)
            MonsterScript monsterScript = colliderClick.gameObject.GetComponent<MonsterScript>();
            // printMobile("pointerUpSansDrag");

            if (lastMonsterScript == monsterScript)
                instance.SelectionMonster(monsterScript);
        }
        else
        {
            //STATE: Le doigt est leve et a ete drag (sois deplacement camera ou drag monstre)
            instance.DragBackground(GetCameraPos2D(eventData.position), false);
            // printMobile("pointerUpAvecDrag");
            lastMonsterScript = null;
        }

        isOnDrag = false;
        isBackClick = false;
        isMonsterClick = false;
        pointerDownID = NULL_ID;
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

    private Vector2 GetCameraPos2D(Vector2 clickPosition)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(clickPosition);
        return new Vector2(mousePos.x, mousePos.y);
    }

    private void printMobile(string text)
    {
        log.text = text + "   \n" + log.text;
    }
}
