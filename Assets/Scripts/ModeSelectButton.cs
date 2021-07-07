using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModeSelectButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private MenuOptionBox msm;
    [SerializeField] private int index;
    
        
    public void OnPointerEnter(PointerEventData eventData)
    {
        msm.UpdateBox(index);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        msm.UpdateBox(0);
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        msm.UpdateBox(index);
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        msm.UpdateBox(0);
    }
}
