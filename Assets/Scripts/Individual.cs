using System.Collections.Generic;
using SubScripts.Base;
using UnityEngine;

[System.Serializable]
public class Individual{

    public List<float> GeneList = new List<float>();

    public float Evaluation;

    public SimpleAddTorque SimpleAddTorque;

    public GeneDecoderByTime GeneDecoderByTime;

    public Rigidbody SockerBallRigidBody;

    public IndividualMonobehaviour IndividualMonobehaviour;
}
