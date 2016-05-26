using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Leap.Unity {

  /// <summary>
  /// A basic utility class to aid in creating pinch based actions.  Once linked with an IHandModel, it can
  /// be used to detect pinch gestures that the hand makes.
  /// </summary>
  public class LeapPinchDetector : MonoBehaviour {

    protected const float MM_TO_M = 0.001f;
    
    [SerializeField]
    public IHandModel _handModel;

    [SerializeField]
    protected float _activatePinchDist = 0.03f;

    [SerializeField]
    protected float _deactivatePinchDist = 0.04f;

    protected int _lastUpdateFrame = -1;

    protected bool _isPinching = false;
    protected bool _didChange = false;

    protected float _lastPinchTime = 0.0f;
    protected float _lastUnpinchTime = 0.0f;

    protected Vector3 _pinchPos;
    protected Quaternion _pinchRotation;
    protected Hand handType;

    protected bool canPinch = false;
    protected virtual void OnValidate() {
      if (_handModel == null) {
        _handModel = GetComponentInParent<IHandModel>();
      }

      _activatePinchDist = Mathf.Max(0, _activatePinchDist);
      _deactivatePinchDist = Mathf.Max(0, _deactivatePinchDist);

      //Activate distance cannot be greater than deactivate distance
      if (_activatePinchDist > _deactivatePinchDist) {
        _deactivatePinchDist = _activatePinchDist;
      }
    }

    protected virtual void Awake() {
      if (GetComponent<IHandModel>() != null) {
        Debug.LogWarning("LeapPinchDetector should not be attached to the IHandModel's transform. It should be attached to its own transform.");
      }
      if (_handModel == null) {
        Debug.LogWarning("The HandModel field of LeapPinchDetector was unassigned and the detector has been disabled.");
        enabled = false;
      }
    }

    protected virtual void Update() {
      //We ensure the data is up to date at all times because
      //there are some values (like LastPinchTime) that cannot
      //be updated on demand
      if (canPinch)
      {
        ensurePinchInfoUpToDate();
        Debug.Log("can Pinch");
      }
      else
        Debug.Log("cannot pinch");
    }

    /// <summary>
    /// Returns whether or not the dectector is currently detecting a pinch.
    /// </summary>
    public bool IsPinching {
      get {
        ensurePinchInfoUpToDate();
        return _isPinching;
      }
    }

    /// <summary>
    /// Returns whether or not the value of IsPinching is different than the value reported during
    /// the previous frame.
    /// </summary>
    public bool DidChangeFromLastFrame {
      get {
        ensurePinchInfoUpToDate();
        return _didChange;
      }
    }

    /// <summary>
    /// Returns whether or not the value of IsPinching changed to true between this frame and the previous.
    /// </summary>
    public bool DidStartPinch {
      get {
        ensurePinchInfoUpToDate();
        return DidChangeFromLastFrame && IsPinching;
      }
    }

    /// <summary>
    /// Returns whether or not the value of IsPinching changed to false between this frame and the previous.
    /// </summary>
    public bool DidEndPinch {
      get {
        ensurePinchInfoUpToDate();
        return DidChangeFromLastFrame && !IsPinching;
      }
    }

    /// <summary>
    /// Returns the value of Time.time during the most recent pinch event.
    /// </summary>
    public float LastPinchTime {
      get {
        ensurePinchInfoUpToDate();
        return _lastPinchTime;
      }
    }

    /// <summary>
    /// Returns the value of Time.time during the most recent unpinch event.
    /// </summary>
    public float LastUnpinchTime {
      get {
        ensurePinchInfoUpToDate();
        return _lastUnpinchTime;
      }
    }

    /// <summary>
    /// Returns the position value of the detected pinch.  If a pinch is not currently being
    /// detected, returns the most recent pinch position value.
    /// </summary>
    public Vector3 Position {
      get {
        ensurePinchInfoUpToDate();
        return _pinchPos;
      }
    }

    /// <summary>
    /// Returns the rotation value of the detected pinch.  If a pinch is not currently being
    /// detected, returns the most recent pinch rotation value.
    /// </summary>
    public Quaternion Rotation {
      get {
        ensurePinchInfoUpToDate();
        return _pinchRotation;
      }
    }

   public Hand HandType
    {
      get
      {
        ensurePinchInfoUpToDate();
        return handType;
      }
    }
    
    protected virtual void ensurePinchInfoUpToDate() {
      if (Time.frameCount == _lastUpdateFrame) {
        return;
      }
      _lastUpdateFrame = Time.frameCount;

      _didChange = false;

      Hand hand = _handModel.GetLeapHand();
      handType = hand;

      float pitch = hand.Direction.Pitch;
      float yaw = hand.Direction.Yaw;
      ////Debug.Log("pitc" + pitch);
      ////Debug.Log("yaw" + yaw);





      //Vector handXBasis = hand.PalmNormal.Cross(hand.Direction).Normalized;
      //Debug.Log("handXBasis" + handXBasis);

      //Debug.Log("PalmNorma"+hand.PalmNormal);
      if (hand == null || !_handModel.IsTracked) {
        changePinchState(false);
        return;
      }

     // handType(hand);

      float pinchDistance = hand.PinchDistance * MM_TO_M;
      //transform.rotation = hand.Basis.CalculateRotation();

      //transform.position = Vector3.zero;

      if (_isPinching) {
        if (pinchDistance > _deactivatePinchDist) {
          changePinchState(false);
                    Debug.Log("is't a pinch!");
          return;
        }
      } else {
        if (pinchDistance < _activatePinchDist) {
          changePinchState(true);
                    Debug.Log("is a pinch!");
        }
      }

      if (_isPinching) {
        _pinchPos = transform.position;
       // _pinchRotation = transform.rotation;
      }
    }




    protected virtual void changePinchState(bool shouldBePinching) {
      if (_isPinching != shouldBePinching) {
        _isPinching = shouldBePinching;

        if (_isPinching) {
          _lastPinchTime = Time.time;
        } else {
          _lastUnpinchTime = Time.time;
        }

        _didChange = true;
      }
    }


    public void CanPinch()
    {
      canPinch = true;
    }

    public void CannotPinch()
    {
      canPinch = false;
    }
    //protected virtual void handType()
    //{
    //  Hand h = _handModel.GetLeapHand();
    //  if (h.IsLeft)
    //  {
    //    Debug.Log("This hand is a lefty");
    //  }

    //  else if (h.IsRight)
    //  {
    //    Debug.Log("This hand is a righty");
    //  }

    //  else
    //  {
    //    Debug.Log("There are no hands");
    //  }
    //}
  }
}
