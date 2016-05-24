using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour {

  private string jsonString;
  public JsonData itemData;

	// Use this for initialization
	void Start () {

    jsonString = File.ReadAllText(Application.dataPath + "/Resources/Items.json");
    itemData = JsonMapper.ToObject(jsonString);
    Debug.Log(jsonString);
    Debug.Log(GetItem("Grab", "menu")["power"]);
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
}
