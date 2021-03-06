using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class BetEvent : UnityEvent<int>
{
}

public class Beter : MonoBehaviour
{
	private static Beter s_Instance;
	public static Beter Get() { return s_Instance; }

	[SerializeField]
	private GameObject m_SelectArrow;

	[SerializeField]
	private Text m_ChipsDisplay;

	[SerializeField]
	private BetEvent m_BetClickEvent;

	private int m_BetAmount = 0;

	public int GetBet() { return m_BetAmount; }

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
	}

	private void Start()
	{
		m_ChipsDisplay.text = string.Format("{0:n0}", Player.Get().m_Chips);
	}

	public void OnChipClick(Chip chip)
	{
		m_BetClickEvent.Invoke(chip.m_Amount);
		Vector3 newSelectorPos = m_SelectArrow.transform.position;
		newSelectorPos.x = chip.transform.position.x;
		m_SelectArrow.transform.position = newSelectorPos;
	}

	public void UpdateDisplay()
	{
		m_ChipsDisplay.text = string.Format("{0:n0}", Player.Get().m_Chips);
	}

}
