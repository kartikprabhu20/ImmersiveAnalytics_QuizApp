using UnityEngine;
using UnityEngine.EventSystems;

public class SelectButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private RayCastSelector m_rayCastSelector;
    public GameObject selector;

    void Start()
    {
        selector.SetActive(true);
        m_rayCastSelector = selector.GetComponent<RayCastSelector>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       Debug.Log("Name of button:" + eventData.selectedObject.name);

        m_rayCastSelector.OnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_rayCastSelector.OnPointerUp();

    }

    internal void setRayCastSelector(RayCastSelector rayCastSelector)
    {
        m_rayCastSelector = rayCastSelector;
    }
}
