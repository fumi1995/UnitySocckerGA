using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Individual{

    public List<Chromosome> ChromosomeList = new List<Chromosome>();

    public float Evaluation;

    public Rigidbody SockerBallRigidBody;

    public IndividualMonobehaviour IndividualMonobehaviour;
}
