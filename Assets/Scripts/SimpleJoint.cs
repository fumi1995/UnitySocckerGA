using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleJoint : MonoBehaviour {

    [SerializeField]
    private List<SimpleJointData> _jointObjectList = new List<SimpleJointData>();

    private void Reset()
    {
        HoldJointPosition();
    }
    
    void Update()
    {
        HoldJointPosition();
    }

    private void HoldJointPosition()
    {
        _jointObjectList.ForEach(jointData => jointData.transform.position = this.transform.position - jointData.jointPoint);
    }
}
