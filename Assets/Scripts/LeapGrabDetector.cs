using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leap;

namespace Leap.Unity
{
  public class LeapGrabDetector : MonoBehaviour
  {

    [SerializeField]
    public IHandModel _handModel;
    public Text text;
    protected bool canGrab = false;
    protected bool canGrow = false;
    protected int grabCounter = 0;

    public int GrabCounter
    {
      get
      {
        ensureGrabInfo();
        return grabCounter;
      }
    }


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

    // Update is called once per frame
    void Update()
    {
      if (canGrab)
      {
        Debug.Log("Can Grab");
        ensureGrabInfo();
      }
    }

    protected virtual void ensureGrabInfo()
    {
      Hand hand = _handModel.GetLeapHand();
      var fingers = hand.Fingers;
      grabDetection(fingers);
    }


    public void CanGrab()
    {
      canGrab = true;
      grabCounter = 0;
      text.text = grabCounter.ToString() ;
    }

    public void CannotGrab()
    {
      canGrab = false;
    }

    /*
    * Detect a close hand
    * by checking if all fingers are wide open (extended)
    * !Note. It's not going to detect if the finger is super extended.
    * Takes as a parameter a list of fingers
    * Returns a bool
    */
    protected virtual void grabDetection(List<Finger> fingers)
    {
      if (!fingers.Any(o => o.IsExtended))
      {
        //StartCoroutine(DoTheDance());
        if(canGrow)
          SetCounter();
        Debug.Log("Its a full finger grab!!!");
      }
      else
      {
        Debug.Log("Its not a full finger grab");
        canGrow = true;
      }
    }

    protected void SetCounter()
    {
      grabCounter++;
      text.text = grabCounter.ToString();
      Debug.Log(grabCounter);
      canGrow = false;
    }
  }
}