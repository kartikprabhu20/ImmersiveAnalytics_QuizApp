using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class PopupHandler : MonoBehaviour
{
    [SerializeField]
    UIPopup popup;

    [SerializeField]
    UIButton showButton;

    Color opaqueColor, transparentColor;

    private void Awake()
    {
        Image image = showButton.GetComponent<Image>();
        opaqueColor = new Color(image.color.r, image.color.g, image.color.b, 1f);
        transparentColor = new Color(image.color.r, image.color.g, image.color.b, 0f);
    }

    public void ShowPopup()
    {
        popup.Show();
        showButton.GetComponent<Image>().color = transparentColor;
    }

    public void HidePopup()
    {
        popup.Hide();
        Invoke("ActivateShowButton", 0.5f);
    }

    private void ActivateShowButton()
    {
        showButton.GetComponent<Image>().color = opaqueColor;
    }

}
