using UnityEngine;
using System.Collections;

namespace Leap.Unity
{
  public class LeapRotationDetector : MonoBehaviour
  {

    [SerializeField]
    public IHandModel _handModel;
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
        Debug.Log("Can roll");
        ensureRollInfo();
      }
    }

    protected virtual void ensureRollInfo()
    {
      Hand hand = _handModel.GetLeapHand();
      float roll = -hand.PalmNormal.Roll;
      ////Debug.Log("pitc" + pitch);
      ////Debug.Log("yaw" + yaw);
      Debug.Log("roll" + roll);
      float rollDegrees = ToDegrees(roll);
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
      
    }

    public void CannotRoll()
    {
      canRoll = false;
    }
  }
}
