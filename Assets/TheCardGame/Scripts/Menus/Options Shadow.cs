using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonShadowEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image shadowImage;
    private Vector3 defaultShadowScale;
    public Vector3 enlargedShadowScale = new Vector3(1.5f, 1.5f, 1); // Adjust size when pressed

    void Start()
    {
        defaultShadowScale = shadowImage.transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (shadowImage != null)
        {
            shadowImage.transform.localScale = enlargedShadowScale;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (shadowImage != null)
        {
            shadowImage.transform.localScale = defaultShadowScale;
        }
    }
}