using UnityEngine;
using UnityEngine.UI;
using System;

public class SetData : MonoBehaviour {

  public Text textReference;
  public Text counter;
  public Text message;
  public Image leftImage;
  public Image rightImage;

  private string selectedText;
  private string leftImageName;
  private string rightImageName;
  private string selectedMessage;
  private string pressedButton;
  private int value = 0;
  private static ReadJson j;

	// Update is called once per frame
	void Update () {
    Message();
	}

  void Awake()
  {
    j = this.GetComponent<ReadJson>();
    textReference.text = "";
  }
  private void SetInfo()
  {
    switch (pressedButton)
    {
      case "Grab":
        selectedText = j.GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = j.GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = j.GetItem(pressedButton, "tips")["img2"].ToString();
        break;

      case "Roll":
        selectedText = j.GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = j.GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = j.GetItem(pressedButton, "tips")["img2"].ToString();
        break;
      case "Pinch":
        selectedText = j.GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = j.GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = j.GetItem(pressedButton, "tips")["img2"].ToString();
        break;
      default:
        Debug.Log("Default case");
        break;
    }

    ChangeInfo(selectedText, leftImageName, rightImageName);
  }

  private void Message()
  {
    int c = Int32.Parse(counter.text);
    Debug.Log(c);

    if (c == 0)
    {
      value = 0;
      selectedMessage = j.GetItem(value.ToString(), "scoring")["text"].ToString();
    }
    if (c <= 5 && c > 0)
    {
      value = 1;
      selectedMessage = j.GetItem(value.ToString(), "scoring")["text"].ToString();
    }

    if (c >= 5 && c < 10)
    {
      value = 2;
      selectedMessage = j.GetItem(value.ToString(), "scoring")["text"].ToString();
    }
    if(c >= 10)
    {
      value = 3;
      selectedMessage = j.GetItem(value.ToString(), "scoring")["text"].ToString();
    }
    message.text = selectedMessage;
  }
  private void ChangeInfo(string s, string image1, string image2)
  {
    textReference.text = s;
    leftImage.sprite = Resources.Load<Sprite>("Images/" + image1);
    rightImage.sprite = Resources.Load<Sprite>("Images/" + image2);

  }
  public void PushedButton(string s)
  {
    pressedButton = s;
    SetInfo();
  }
}
