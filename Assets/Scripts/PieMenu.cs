using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PieMenu : MonoBehaviour {

	public int numPoints = 20;                       
	public Vector3 centerPos = new Vector3(0,0,32);   
	public Button[] pieButton;
	public Button clickButton;
	public Button settings;
	public float radiusX,radiusY;                 
	public float smoothingPos = 5.0f;
	public float smoothingScale = 5.0f;
	public bool isCircular = false;                 
	public bool vertical = true; 
	private bool moved = false;
	Vector3 pointPos;                        
	
	public float frequency = 2.0f;  // Speed of sine movement
	public float magnitude = 1f;   // Size of sine movement
	public float Amplitude = 1.0f;
	public float phaseAngle = 2.0f;

	private Vector3 axis;
	public enum Radius
	{
		Pi,
		PiOver2,
		PiTimes2
	}
	//This is what you need to show in the inspector.
	public Radius AngleCalculation;
	void Start()
	{
		if(isCircular){
			radiusY = radiusX;
		}
		for(int i = 0; i < numPoints; i++)
		{
			pieButton[i].GetComponent<RectTransform>().localScale = Vector3.zero;
		}
	}

	public void Update()
	{
		float angle = RadiusSet (AngleCalculation);
		if (moved) 
		{
		
			float i = - 10* Time.deltaTime;
			//settings.GetComponent<RectTransform>().Rotate(0f, 0f, i);
			ScaleButton();
			Move (smoothingScale, smoothingPos, angle);
		}

	}
	 
	private float RadiusSet(Radius rad)
	{
		float angle = 0;
		if (rad == Radius.Pi)
			angle = -Mathf.PI;
		else if (rad == Radius.PiOver2)
			angle = Mathf.PI/2;
		else if (rad == Radius.PiTimes2)
			angle = Mathf.PI * 2;

		return angle;
	}

	private void ScaleButton()
	{
		//clickButton.GetComponent<RectTransform>().localScale = Vector3.Slerp(clickButton.GetComponent<RectTransform>().localScale, Vector3.zero, smoothingScale * Time.deltaTime);
	}

	private void Move(float smoothScale, float smoothingPos, float a)
	{
		for(int i = 0; i<numPoints;i++){
			//multiply 'i' by '1.0f' to ensure the result is a fraction
			float pointNum = (i*1.0f)/numPoints;
			
			//angle along the unit circle for placing points
			float angle = pointNum * a;
			
			float x = Mathf.Sin (angle)*radiusX;
			float y = Mathf.Cos (angle)*radiusY;
			
			//position for the point prefab
			if(vertical)
				pointPos = new Vector3(x, y)+centerPos;
			else if (!vertical){
				pointPos = new Vector3(x, 0, y)+centerPos;
			}
			//place the prefab at given position
			axis = pieButton[i].GetComponent<RectTransform>().localPosition - pointPos;
			axis.Normalize();

			pieButton[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(pieButton[i].GetComponent<RectTransform>().localPosition, pointPos + DampedHarmonicOscilation(Amplitude, Time.time, magnitude, frequency, phaseAngle) * axis, smoothingPos * Time.deltaTime);
			//pieButton[i].GetComponent<RectTransform>().localPosition = pointPos+ DampedHarmonicOscilation(Amplitude, Time.time, magnitude, frequency, phaseAngle) * axis ;
			pieButton[i].GetComponent<RectTransform>().localScale = Vector3.Slerp(pieButton[i].GetComponent<RectTransform>().localScale, Vector3.one, smoothingScale * Time.deltaTime);
			//yield return new WaitForSeconds(2f);
		}
	}

	//toogle the moved boolean
	public void Toogle()
	{
		moved = !moved;
	}

	//var are constants
	private float DampedHarmonicOscilation(float amplitued, float time, float magnitude, float freq, float phaseAngle)
	{
		return amplitued * Mathf.Exp ((-magnitude * time)/2) * Mathf.Cos (freq * time - phaseAngle);
	}
}
