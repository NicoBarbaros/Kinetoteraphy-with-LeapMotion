using UnityEngine;
using System.Collections;

//using Random = UnityEngine.Random;
public class MoveUI : MonoBehaviour {

	public float smooth;
	public float step;
	public bool moved = false;
  public float waitingTime = 2.0f;
  private bool firstMove = false;
  private Vector3 oldPosition;

  void Start()
	{

		oldPosition = transform.position;
}
	
	// Update is called once per frame
	void Update ()                                                 
	{
    if (moved)
      PositionChanging(step);
   
  }

  //position change in relation to the new distance
  public void PositionChanging(float distance)
  {
    if (!firstMove) { 
      Vector3 newPosition = new Vector3(oldPosition.x + distance, oldPosition.y,
                                         oldPosition.z);
      transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smooth);
    }
  }

  //toogle the moved boolean
  public void Toogle()
	{
		moved = true;
	}
}