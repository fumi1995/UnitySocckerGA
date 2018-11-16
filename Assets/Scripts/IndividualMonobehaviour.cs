using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubScripts.Base;
using System.Linq;

public class IndividualMonobehaviour : InitializationMonobehaviour
{
    [SerializeField]
    public Individual Indivudual;

    [SerializeField]
    public ScoreText ScoreText;

    public override void Initialization()
    {
        base.Initialization();

        Indivudual.ChromosomeList = GetComponentsInChildren<ChromosomeMonobehaviour>().Select(chromosomeMonobehaviour => chromosomeMonobehaviour.Chromosome).ToList();

        Indivudual.IndividualMonobehaviour.ScoreText.SetText(Indivudual.Evaluation.ToString());
    }
}
