using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GeneDecoder : MonoBehaviour {

    public List<float> _geneList = new List<float>();

    [SerializeField]
    private CharacterJoint _characterJoint;

    [SerializeField]
    private Transform _connectedBodyTransform;

    [SerializeField]
    private float _connectedJointAngularLimitMin;

    [SerializeField]
    private float _connectedJointAngularLimitMax;

    private SimpleAddTorque _simpleAddTorque;

    private InitialPosition _initialPosition;

    // 初期角度
    private float _initialAngle;

    // 可動域角度
    private float _angleRange;

    // step角度
    private float _stepAngle;

    // 現在使用している遺伝子番号
    private int _currentGeneIdx;

    private void Awake()
    {
        _characterJoint = GetComponent<CharacterJoint>();

        _initialPosition = GetComponent<InitialPosition>();

        _simpleAddTorque = GetComponent<SimpleAddTorque>();
    }

    // Use this for initialization
    void Start ()
    { 
        _connectedBodyTransform = _characterJoint.connectedBody.transform;

        var connectedJoint = _connectedBodyTransform.GetComponent<CharacterJoint>();

        if (connectedJoint)
        {
            _connectedJointAngularLimitMax = _characterJoint.highTwistLimit.limit;
            _connectedJointAngularLimitMin = _characterJoint.lowTwistLimit.limit;

            _initialAngle = Vector3.Dot(_connectedBodyTransform.rotation.eulerAngles, connectedJoint.axis);

            _angleRange = _connectedJointAngularLimitMax - _connectedJointAngularLimitMin;

            _stepAngle = _angleRange / _geneList.Count;
        }
        
        this.UpdateAsObservable().Where(_=>_initialPosition.IsFinish).Subscribe(_ =>
        {
            Debug.Log(_connectedBodyTransform.localEulerAngles);// BAG: インスペクターの値とは360°異なる

            // 現在の分割角度よりも大きくなったら次の分割角度へ
            if( Vector3.Dot(_connectedBodyTransform.localEulerAngles, connectedJoint.axis) < -(_currentGeneIdx + 1) * _stepAngle + LowTwistAngleToLocalAngle(connectedJoint))
            {
                _currentGeneIdx++;
            }

            if (_currentGeneIdx >= _geneList.Count)
            {
                Debug.Log("END");
            }
            else
            {
                _simpleAddTorque._torqueMagnitude = _geneList[_currentGeneIdx];
                Debug.Log(_geneList[_currentGeneIdx]);
            }
        });
    }

    float LowTwistAngleToLocalAngle(CharacterJoint characterJoint)
    {
        return -_characterJoint.lowTwistLimit.limit + _initialAngle;
    }

    float HighTwistAngleToLocalAngle(CharacterJoint characterJoint)
    {
        return _characterJoint.highTwistLimit.limit - _initialAngle;
    }
}
