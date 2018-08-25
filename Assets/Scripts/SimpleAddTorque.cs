using UnityEngine;

/*
    トルクを加えるだけのコントローラークラス
  */

[RequireComponent(typeof(Rigidbody))]
public class SimpleAddTorque : MonoBehaviour {

    private Rigidbody _rbody;

    // トルクの向き
    public Vector3 _torqueAxis;

    // トルクの大きさ
    public float _torqueMagnitude;

    // トルクの種類
    [SerializeField]
    private ForceMode _forceMode = ForceMode.Force;

	// Use this for initialization
	void Start () {
        _rbody = GetComponent<Rigidbody>();

        _rbody.maxAngularVelocity = Mathf.Infinity;
	}
	
	// Update is called once per frame
	void Update () {
        // 単位ベクトル化
        _torqueAxis = Vector3.Normalize(_torqueAxis);

        // トルクを加える
        _rbody.AddTorque(_torqueAxis * _torqueMagnitude, _forceMode);
	}
}
