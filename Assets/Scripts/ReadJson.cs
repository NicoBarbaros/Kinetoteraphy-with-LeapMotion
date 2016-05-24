using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour {

  private string jsonString;
  public JsonData itemData;

  public Text textReference;
  public Image leftImage;
  public Image rightImage;

  private string pressedButton;
	// Use this for initialization
  void Awake()
  {
    textReference.text = "";
  }

  void Start () {
    jsonString = File.ReadAllText(Application.dataPath + "/Resources/Items.json");
    itemData = JsonMapper.ToObject(jsonString);
    Debug.Log(jsonString);
    Debug.Log(GetItem("Grab", "menu")["text"]);
	}

	// Update is called once per frame

	void Update () {
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

        break;
      case "Roll":
        break;
      case "Pinch":
        break;
      default:
        Debug.Log("Default case");
        break;
    }
  }
  private void ChangeInfo(string s)
  {

  }
  public void PushedButton(string s)
  {
    pressedButton = s;
  }
}
