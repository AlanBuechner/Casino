using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButton : Button
{
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		image.sprite = AssetManager.Get().m_DownImage;
	}

	// Button is released
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		image.sprite = AssetManager.Get().m_UpImage;
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		OnClicked();
	}

	protected virtual void OnClicked() { }

}
