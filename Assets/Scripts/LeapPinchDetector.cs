using UnityEngine;
using UnityEngine.UI;
using System;
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
    protected float _activatePinchDist = 0.3f;

    [SerializeField]
    protected float _deactivatePinchDist = 0.4f;

    protected int _lastUpdateFrame = -1;

    protected bool _isPinching = false;
    protected bool _didChange = false;

    protected float _lastPinchTime = 0.0f;
    protected float _lastUnpinchTime = 0.0f;

    protected Vector3 _pinchPos;
    protected Quaternion _pinchRotation;
    protected Hand handType;

    protected bool canPinch = false;
    protected List<Finger> fingers;

    public Text text;
    protected bool canGrowIndex = true;
    protected bool canGrowMiddle = true;
    protected bool canGrowRing = true;
    protected bool canGrowPinky = true;
    protected int grabCounter = 0;

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
    }

    /// Returns whether or not the dectector is currently detecting a pinch.
    public bool IsPinching {
      get {
        ensurePinchInfoUpToDate();
        return _isPinching;
      }
    }

    /// Returns whether or not the value of IsPinching is different than the value reported during
    /// the previous frame.
    public bool DidChangeFromLastFrame {
      get {
        ensurePinchInfoUpToDate();
        return _didChange;
      }
    }

    /// Returns whether or not the value of IsPinching changed to true between this frame and the previous.
    public bool DidStartPinch {
      get {
        ensurePinchInfoUpToDate();
        return DidChangeFromLastFrame && IsPinching;
      }
    }

    /// Returns whether or not the value of IsPinching changed to false between this frame and the previous.
    public bool DidEndPinch {
      get {
        ensurePinchInfoUpToDate();
        return DidChangeFromLastFrame && !IsPinching;
      }
    }

    /// Returns the value of Time.time during the most recent pinch event.
    public float LastPinchTime {
      get {
        ensurePinchInfoUpToDate();
        return _lastPinchTime;
      }
    }

    /// Returns the value of Time.time during the most recent unpinch event.
    public float LastUnpinchTime {
      get {
        ensurePinchInfoUpToDate();
        return _lastUnpinchTime;
      }
    }

    /// Returns the position value of the detected pinch.  If a pinch is not currently being
    /// detected, returns the most recent pinch position value.
    public Vector3 Position {
      get {
        ensurePinchInfoUpToDate();
        return _pinchPos;
      }
    }

    /// Returns the rotation value of the detected pinch.  If a pinch is not currently being
    /// detected, returns the most recent pinch rotation value.
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


      ////Trying to pinch each finger with thumb
      handType = hand;

      //Vector handXBasis = hand.PalmNormal.Cross(hand.Direction).Normalized;
      //Debug.Log("handXBasis" + handXBasis);

      //Debug.Log("PalmNorma"+hand.PalmNormal);
      if (hand == null || !_handModel.IsTracked) {
        changePinchState(false);
        return;
      }


      FingerTips(hand);
      // handType(hand);


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
      grabCounter = 0;
      text.text = grabCounter.ToString();
    }

    public void CannotPinch()
    {
      canPinch = false;
    }


    protected void FingerTips(Hand h)
    {
      Dictionary<string, Vector3> fingerInfo = new Dictionary<string, Vector3>();
      List<Finger> fingers = h.Fingers;

      foreach (Finger finger in fingers)
      {
        fingerInfo.Add(finger.Type.ToString(), finger.TipPosition.ToVector3());
      }
      Debug.Log(fingerInfo.Count);

      var toIndexDist = GetDistanceToThumb(fingerInfo, 0, 1);
      var toMiddleDist = GetDistanceToThumb(fingerInfo, 0, 2);
      var toRingDist = GetDistanceToThumb(fingerInfo, 0, 3);
      var toPinkyDist = GetDistanceToThumb(fingerInfo, 0, 4);


      if (_isPinching)
      {
        if (toIndexDist > _deactivatePinchDist)
        {
          canGrowIndex = true;
          changePinchState(false);
          //Debug.Log("isn't a pinch! with index");
          return;
        }

       else if (toMiddleDist > _deactivatePinchDist)
        {
          canGrowMiddle = true;
          //Debug.Log("isn't a pinch! with index");
          return;
        }

       else if (toRingDist > _deactivatePinchDist)
        {
          canGrowRing = true;
          //Debug.Log("isn't a pinch! with index");
          return;
        }

       else if (toPinkyDist > _deactivatePinchDist)
        {
          canGrowPinky = true;
          //Debug.Log("isn't a pinch! with index");
          return;
        }



      }
      else
      {
        if (toIndexDist < _activatePinchDist)
        {
          if (canGrowIndex)
            SetCounterIndex();
          changePinchState(true);
        }
        else if (toMiddleDist < _activatePinchDist)
        {
          if (canGrowMiddle)
            SetCounterMiddle();
          changePinchState(true);
        }
        else if (toRingDist < _activatePinchDist)
        {
          if (canGrowRing)
            SetCounterRing();

          changePinchState(true);
        }
        else if (toPinkyDist < _activatePinchDist)
        {
          if (canGrowPinky)
            SetCounterPinky();
          changePinchState(true);
        }
      }

    }

    protected float GetDistanceToThumb(Dictionary<string, Vector3> fingers, int firstPosition, int secondPosition)
    {
      float distance = (float)Math.Round(Vector3.Distance(fingers.ElementAt(firstPosition).Value, fingers.ElementAt(secondPosition).Value), 4) * 10.0f;
      return distance;
    }

    protected void SetCounterIndex()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      canGrowIndex = false;
    }

    protected void SetCounterMiddle()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      canGrowMiddle = false;
    }

    protected void SetCounterRing()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      canGrowRing = false;
    }

    protected void SetCounterPinky()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      canGrowPinky = false;
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
