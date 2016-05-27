using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Intro : MonoBehaviour {

  public static Intro intro;
  public Image panel;
  public Text text1;
  public Text text2;
  public float fadeSpeed = 1.5f;
  public float waitingTime = 2f;
  public bool canPieMenu = false;
  // Use this for initialization
  void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
   
   StartCoroutine( FadeToClear());
    Debug.Log(canPieMenu);
	}


  IEnumerator FadeToClear()
  {

    yield return new WaitForSeconds(waitingTime);
    text1.color = Color.clear;
    text2.color = Color.clear;
    panel.color = Color.Lerp(panel.color, Color.clear, fadeSpeed * Time.deltaTime);
    yield return new WaitForSeconds(waitingTime);
    canPieMenu = true;
    Debug.Log("adadae");
    panel.raycastTarget = false;
    text1.raycastTarget = false;
    text2.raycastTarget = false;
  }
}
