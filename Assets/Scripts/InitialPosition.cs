using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class InitialPosition : MonoBehaviour {

    [SerializeField]
    private Vector3 _torqueAxis;
    
    [SerializeField]
    private float _torqueMagnitude;

    // この秒数止まっていたらとまったことにする
    [SerializeField]
    private int _stopIntervalSec = 3;

    // 止まっている判定にする速度の範囲
    [SerializeField]
    private float _stopVelocityRange = 0.1f;

    private Rigidbody _rbody;

    private SimpleAddTorque _simpleAddTorque;

    public bool IsFinish;//{ get; private set; }

    // Use this for initialization
    void Start ()
    {
        _rbody = GetComponent<Rigidbody>();
        _simpleAddTorque = GetComponent<SimpleAddTorque>();

        _simpleAddTorque._torqueAxis = _torqueAxis;
        _simpleAddTorque._torqueMagnitude = _torqueMagnitude;

        Observable.Timer(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            .Where(_ => Vector3.Distance(_rbody.velocity, Vector3.zero) < _stopVelocityRange)
            .Skip(_stopIntervalSec * 10)
            .Subscribe(_ =>
            {
                IsFinish = true;
            });
	}
}
