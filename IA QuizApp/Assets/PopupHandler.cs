using UnityEngine;
using Doozy.Engine.UI;

public class PopupHandler : MonoBehaviour
{
    [SerializeField]
    UIPopup popup;

    public void ShowPopup()
    {
        popup.Show();
    }

    public void HidePopup()
    {
        popup.Hide();
    }
}
