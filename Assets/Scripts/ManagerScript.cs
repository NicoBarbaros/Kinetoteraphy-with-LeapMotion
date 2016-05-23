using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leap;
using LeapInternal;

namespace Leap.Unity
{
  public class ManagerScript : MonoBehaviour
  {

    public enum Pinch
    {
      Only_Left,
      Only_Right,
      Left_and_Right
    }

    [SerializeField]
    private LeapPinchDetector leftHand;
    public LeapPinchDetector LeftHand
    {
      get
      {
        return leftHand;
      }

      set
      {
        leftHand = value;
      }
    }

    [SerializeField]
    private LeapPinchDetector rightHand;
    public LeapPinchDetector RightHand {
      get {
        return rightHand;
      }
      set {
        rightHand = value;
      }
    }

    
    // Use this for initialization
    void Start()
    {
      if(leftHand == null || rightHand == null)
      {
        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        enabled = false;
      }
    }

    // Update is called once per frame
    void Update()
    {
 
      //Hand leftType = leftHand.HandType;
      //if (leftType != null)
      //{
      //  Debug.Log(leftType.IsLeft);
      //}

      //Hand rightType = rightHand.HandType;
      //if(rightType != null)
      //{
      //  Debug.Log(rightType.IsRight);
      //}

    }

    protected virtual void handType(Hand h)
    {
      if (h.IsLeft)
      {
        Debug.Log("This hand is a lefty");
      }

      else if (h.IsRight)
      {
        Debug.Log("This hand is a righty");
      }
    }
  }
}