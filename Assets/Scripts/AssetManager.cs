using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
	private static AssetManager s_Instance;
	public static AssetManager Get() { return s_Instance; }

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
	}

	public Sprite m_UpImage;
	public Sprite m_DownImage;

	public Sprite m_Chip1;
	public Sprite m_Chip5;
	public Sprite m_Chip10;
	public Sprite m_Chip20;
	public Sprite m_Chip50;
	public Sprite m_Chip100;
	public Sprite m_Chip500;
	public Sprite m_Chip1k;
	public Sprite m_Chip5k;

	public Sprite m_CardBack;
	public Sprite m_JokerCard;
	public Sprite[] m_Cards;
}
