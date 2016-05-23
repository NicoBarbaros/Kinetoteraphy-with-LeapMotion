using UnityEngine;
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
      ensureGrabInfo();
    }

    protected virtual void ensureGrabInfo()
    {
      Hand hand = _handModel.GetLeapHand();
      var fingers = hand.Fingers;
      grabDetection(fingers);

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
        Debug.Log("Its a full finger grab!!!");
      }

      else
      {
        Debug.Log("Its not a full finger grab");
      }
    }
  }
}