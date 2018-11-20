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

        var chromesomeMonobehaviorList = GetComponentsInChildren<ChromosomeMonobehaviour>().ToList();

        chromesomeMonobehaviorList.ForEach(chromesomeMonobehavior => chromesomeMonobehavior.Chromosome.ChromosomeMonobehaviour = chromesomeMonobehavior);

        Indivudual.ChromosomeList = chromesomeMonobehaviorList.Select(chromosomeMonobehaviour => chromosomeMonobehaviour.Chromosome).ToList();

        Indivudual.IndividualMonobehaviour.ScoreText.SetText(Indivudual.Evaluation.ToString());
    }
}
