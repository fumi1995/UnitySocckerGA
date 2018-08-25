using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class LowerLegController : MonoBehaviour {

    #region SerializedFiled

    [SerializeField]
    private HingeJoint _hingeJoint;

    [SerializeField]
    private float _motorForce = 500;

    [SerializeField]
    private float _targetVelocity = 300;

    #endregion

    #region Decralation

    private JointMotor _jointMotor;

    #endregion

    private void Reset()
    {
        _hingeJoint = this.GetComponent<HingeJoint>();
        _jointMotor = _hingeJoint.motor;

        _hingeJoint.useMotor = true;

        UpdateMotor();
    }

    private void Awake()
    {
        Reset();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateMotor();
	}


    // モーターの設定を更新する
    private void UpdateMotor()
    {
        _jointMotor.force = _motorForce;
        _jointMotor.targetVelocity = _targetVelocity;

        _hingeJoint.motor = _jointMotor;
    }
    
}
