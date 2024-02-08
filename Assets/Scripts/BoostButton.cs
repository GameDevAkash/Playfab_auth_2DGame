using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoostButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Singleton == null) { return; }
        GameManager.Singleton.inGamePlayer.GetComponent<Player>().OnBoostClick();
    }
}
