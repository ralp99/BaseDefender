using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class connector : MonoBehaviour {


    public GameObject LeftHandNode, RightHandNode;
    public bool UseLocalPos;

	// Use this for initialization
	void Start () {





    }

    // Update is called once per frame
    void Update () {

        if (UseLocalPos)
        {
            LeftHandNode.transform.localPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
            RightHandNode.transform.localPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
        }

        else
        {
            LeftHandNode.transform.position = InputTracking.GetLocalPosition(XRNode.LeftHand);
            RightHandNode.transform.position = InputTracking.GetLocalPosition(XRNode.RightHand);
        }

       

        LeftHandNode.transform.localRotation = InputTracking.GetLocalRotation (XRNode.LeftHand);
        RightHandNode.transform.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);


    }
}
