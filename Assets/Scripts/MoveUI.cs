using UnityEngine;
using System.Collections;

//using Random = UnityEngine.Random;
public class MoveUI : MonoBehaviour {

	public float smooth;
	public float step;
	private Vector3 oldPosition;
	public bool moved = false;
  private bool firstMove = false;

	void Start()
	{
		oldPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update ()                                                 
	{
		if(moved)
			PositionChanging (step);
		//else
		//	PositionChanging (0.0f);
    Debug.Log(moved);
	}

  //position change in relation to the new distance
  public void PositionChanging(float distance)
  {
    if (!firstMove) { 
      Vector3 newPosition = new Vector3(oldPosition.x + distance, oldPosition.y,
                                         oldPosition.z);
      transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smooth);
      
    }
    firstMove = true;
  }

	//toogle the moved boolean
	public void Toogle()
	{
		moved = !moved;
	}

}