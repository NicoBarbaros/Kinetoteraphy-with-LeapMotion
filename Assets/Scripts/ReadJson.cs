using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour
{

  private string jsonString;
  public JsonData itemData;

  public Text textReference;
  public Image leftImage;
  public Image rightImage;

  private string selectedText;
  private string leftImageName;
  private string rightImageName;

  private string pressedButton;
  // Use this for initialization
  void Awake()
  {
    textReference.text = "";
  }

  void Start()
  {
    jsonString = File.ReadAllText(Application.dataPath + "/Resources/Items.json");
    itemData = JsonMapper.ToObject(jsonString);
  }

  JsonData GetItem(string value, string type)
  {

    for (int i = 0; i < itemData[type].Count; i++)
    {
      if (itemData[type][i]["value"].ToString() == value)
        return itemData[type][i];
    }

    return null;
  }

  private void SetInfo()
  {
    switch (pressedButton)
    {
      case "Grab":
        selectedText = GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = GetItem(pressedButton, "tips")["img2"].ToString();
        break;

      case "Roll":
        selectedText = GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = GetItem(pressedButton, "tips")["img2"].ToString();
        break;
      case "Pinch":
        selectedText = GetItem(pressedButton, "tips")["text"].ToString();
        leftImageName = GetItem(pressedButton, "tips")["img1"].ToString();
        rightImageName = GetItem(pressedButton, "tips")["img2"].ToString();
        break;
      default:
        Debug.Log("Default case");
        break;
    }

    ChangeInfo(selectedText, leftImageName, rightImageName);
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
