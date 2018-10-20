using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CombatLogPopulate : MonoBehaviour
{

	public GameObject textPrefab;

	public ScrollRect scrollRect;
	// Use this for initialization
	private void Start()
	{
		populate("Combat Started!", false);
	}

	// Update is called once per frame
	public void populate(string text, bool crit)
	{
		GameObject newText;
		newText = GameObject.Instantiate(textPrefab, transform);
		if (crit)
		{
			newText.GetComponent<TextMeshProUGUI>().color = Color.blue;
		}
		else
		{
			newText.GetComponent<TextMeshProUGUI>().color = Color.black;
		}
		newText.GetComponent<TextMeshProUGUI>().text = "<" + DateTime.Now.ToShortTimeString()+ ">" + text;
		StartCoroutine(forceScroll());
	}

	IEnumerator forceScroll()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}
