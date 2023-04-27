using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverSelect : MonoBehaviour
{

    public GameObject stroke;
    
    public void ShowStrokeAtPointer()
    {

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            stroke.SetActive(true);
            stroke.transform.position = raycastResults[0].gameObject.transform.position;
        }


    } 

    public void HideStroke()
    {
        stroke.SetActive(false);
    }


}
