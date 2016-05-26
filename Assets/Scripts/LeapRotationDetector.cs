using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Leap.Unity
{
  public class LeapRotationDetector : MonoBehaviour
  {

    [SerializeField]
    public IHandModel _handModel;
    public Text text;
    protected bool canGrow = false;
    protected int grabCounter = 0;
    protected bool canRoll = false;
    protected virtual void OnValidate()
    {
      if (_handModel == null)
      {
        _handModel = GetComponentInParent<IHandModel>();
      }
    }

    protected virtual void Awake()
    {
      if (GetComponent<IHandModel>() != null)
      {
        Debug.LogWarning("LeapPinchDetector should not be attached to the IHandModel's transform. It should be attached to its own transform.");
      }
      if (_handModel == null)
      {
        Debug.LogWarning("The HandModel field of LeapPinchDetector was unassigned and the detector has been disabled.");
        enabled = false;
      }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if (canRoll)
      {
        ensureRollInfo();
      }
    }

    protected virtual void ensureRollInfo()
    {
      Hand hand = _handModel.GetLeapHand();
      float roll = -hand.PalmNormal.Roll;
      float rollDegrees = ToDegrees(roll);
      Debug.Log(rollDegrees);
      if((rollDegrees > 85 || rollDegrees < -85))
      {
        if(canGrow)
          SetCounter();
      }

      else
      {
        canGrow = true;
      }
    }

    protected float ToDegrees(float Radian)
    {
      float Degrees;
      Degrees = Radian * 180 / Mathf.PI;
      return Degrees;
    }

    public void CanRoll()
    {
      canRoll = true;
      grabCounter = 0;
      text.text = grabCounter.ToString();
    }

    public void CannotRoll()
    {
      canRoll = false;
    }

    protected void SetCounter()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      canGrow = false;
      Debug.Log(canGrow);

    }
  }
}
