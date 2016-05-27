using UnityEngine;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour
{

  private string jsonString;
  public JsonData itemData;
  public static ReadJson readjson;
  // Use this for initialization

  void Update()
  {
  }
  void Awake()
  {
    TextAsset text = Resources.Load("Items") as TextAsset;
    jsonString = text.text;
     itemData = JsonMapper.ToObject(jsonString);
  }

  public JsonData GetItem(string value, string type) 
  {
    for (int i = 0; i < itemData[type].Count; i++)
    {
      if (itemData[type][i]["value"].ToString() == value)
        return itemData[type][i];
    }
    return null;
  }

  public void Count() { }
}
