using UnityEngine;
/*
    第1足の動きを決めるクラス
  */

[RequireComponent(typeof(CharacterJoint))]
[RequireComponent(typeof(SimpleAddTorque))]
public class ThighController : MonoBehaviour
{
    private CharacterJoint _characterJoint;

    private SimpleAddTorque _simpleAddTorque;

    private void Reset()
    {
        _characterJoint = GetComponent<CharacterJoint>();

        _simpleAddTorque = GetComponent<SimpleAddTorque>();
    }

    private void Awake()
    {
        Reset();
    }

    private void Update()
    {
        Kick();
    }

    // ボールをける動作
    // ループさせることもできる
    void Kick()
    {
        var angle = transform.localRotation.eulerAngles.x;

        var currentAngle = angle < 180 ? angle : 360 - angle;

        // 最大まで後ろに足を上げたら逆回転
        if (Mathf.Abs(currentAngle - _characterJoint.highTwistLimit.limit) < 0.1f)
        {
            _simpleAddTorque._torqueAxis *= -1;
        }

        // 最大まで前に足を上げたら逆回転（ループチェック）
        if (Mathf.Abs(currentAngle - -_characterJoint.lowTwistLimit.limit) < 0.1f)
        {
            _simpleAddTorque._torqueAxis *= -1;
        }

        Debug.Log(currentAngle + "/" + _characterJoint.highTwistLimit.limit);

    }
}
