using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SubScripts.Base;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneticAlgorithm : SingletonMonobehaviour<GeneticAlgorithm> {

    // 個体集団
    [NotNull] public List<Individual> IndividualList = new List<Individual>();

    // 個体数
    public readonly int IndividualNum = 100;

    // 遺伝子長
    public readonly int GeneLength = 100;

    // 拡張率（交叉）
    public float ExpasionRate;

    // 1世代に対するエリート率
    public readonly float EliteRate = 0.2f;

    // 1世代に対する子孫率(エリート率の2倍)
    public float ProgenyRate { get { return EliteRate*2; } }

    // 1世代に対する現状維持個体率
    public float OldGenerationRate { get { return Mathf.Max(1 - EliteRate - ProgenyRate); } }

    // 1回のシミュレーションがいったん終了したか
    public bool IsFinishSimulation;

    // 1回のシミュレーション時間
    public float SimulationTimeSec = 10f;

    // 最大トルク
    public float MaxTorque = 100;
    
    // 最小トルク
    public float MinTorque = -60;

    // 世代
    private int _generationNum = 1;

    // 生成する個体
    [SerializeField] [NotNull] private GameObject _individualPrefab;

    public override void Initialization()
    {
        base.Initialization();

        ExpasionRate = Mathf.Sqrt(GeneLength + 2);
        Debug.Log(_generationNum + " 世代目");
        GeneratePopulation();

        Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(SimulationTimeSec))
            .Delay(TimeSpan.FromSeconds(SimulationTimeSec))// 最初のみこの時間だけ遅らせる
            .Subscribe(_ =>
            {

                // 評価
                Evaluate();

                // 終了条件

                // 選択
                var eliteIndividualList = Select();

                // 交叉
                var progenyIndividualList = CrossOver();

                // 生成
                IndividualList = GenerateNextGeneration(IndividualList, eliteIndividualList, progenyIndividualList);

                _generationNum++;
                Debug.Log(_generationNum + "世代目");
            }).AddTo(gameObject);
    }

    // 集団の生成
    private void GeneratePopulation()
    {
        DestroyPopulation();

        for (var i = 0; i < IndividualNum; i++)
        {
            var o = Instantiate(_individualPrefab, new Vector3(i, 0, 0), Quaternion.identity);

            var individual = o.GetComponent<IndividualMonobehaviour>().Indivudual;

            individual.GeneList.Clear();
            
            // ランダムな遺伝子生成
            for (var j = 0; j < GeneLength; j++)
            {
                individual.GeneList.Add(Random.Range(MinTorque, MaxTorque));
            }

            IndividualList.Add(individual);
        }
    }

    // 集団の消去
    private void DestroyPopulation()
    {
        IndividualList.ForEach(i => Destroy(i.IndividualMonobehaviour.gameObject));
        IndividualList.Clear();
    }


    // 評価
    private void Evaluate()
    {
        // IndividualList.ForEach(individual => individual.Evaluation = individual.SockerBallRigidBody.velocity.magnitude);
        IndividualList.ForEach(individual => individual.Evaluation = individual.SockerBallRigidBody.transform.position.z);

        Debug.Log("Top Record:" + IndividualList.OrderBy(individual=> -individual.Evaluation).First().Evaluation + "[m]");
    }

    // ソート
    private List<Individual> SortByEvaluation()
    {
        return IndividualList.OrderBy(individual => -individual.Evaluation).ToList();
    }

    // 選択
    private List<Individual> Select()
    {
        var eliteIndividualList = new List<Individual>();

        var sortedIndividualList = SortByEvaluation();

        for (var i = 0; i < EliteRate * IndividualNum; i++)
        {
            eliteIndividualList.Add(sortedIndividualList[i]);
        }

        return eliteIndividualList;
    }

    // 交叉
    private List<Individual> CrossOver()
    {
        var progenyIndividualList = new List<Individual>();

        for (var i = 0; i < IndividualNum; i++)
        {
            progenyIndividualList.Add(new Individual());

            for (var j = 0; j < GeneLength; j++)
            {
                progenyIndividualList[i].GeneList.Add(0);
            }
        }


        for (var i = 0; i < IndividualList.Count; i++)
        {
            var firstGeneList = IndividualList.LoopElementAt(i).GeneList;
            var secoundGeneList = IndividualList.LoopElementAt(i+1).GeneList;
            var thirdGeneList = IndividualList.LoopElementAt(i+2).GeneList;

            var resultFirstGeneList = progenyIndividualList.LoopElementAt(i).GeneList;
            var resultSecoundGeneList = progenyIndividualList.LoopElementAt(i + 1).GeneList;
            var resultThirdGeneList = progenyIndividualList.LoopElementAt(i + 2).GeneList;

            var gradientPointList = new List<float>();
            
            for (var j = 0; j < GeneLength; j++)
            {
                gradientPointList.Add((firstGeneList[j] + secoundGeneList[j] + thirdGeneList[j]) / 3);
            }
            
            var firstExpansionPointList = new List<float>();
            var secoundExpansionPointList = new List<float>();
            var thirdExpansionPointList = new List<float>();

            for (var j = 0; j < GeneLength; j++)
            {
                firstExpansionPointList.Add(gradientPointList[j] + ExpasionRate * (firstGeneList[j] - gradientPointList[j]));
                secoundExpansionPointList.Add(gradientPointList[j] + ExpasionRate * (secoundGeneList[j] - gradientPointList[j]));
                thirdExpansionPointList.Add(gradientPointList[j] + ExpasionRate * (thirdGeneList[j] - gradientPointList[j]));
            }

            for (var j = 0; j < GeneLength; j++)
            {
                var r1 = Random.Range(0, 1);
                var r2 = Random.Range(0, 1) ^ (1 / 2);

                resultFirstGeneList[j] = firstExpansionPointList[j];
                resultSecoundGeneList[j] = r1 * (firstExpansionPointList[j] - secoundExpansionPointList[j]) + secoundExpansionPointList[j];
                resultThirdGeneList[j] = r2 * (secoundExpansionPointList[j] - thirdExpansionPointList[j] + r1 * (firstExpansionPointList[j] - secoundExpansionPointList[j]) + thirdExpansionPointList[j]);
            }
        }

        // Clamp
        foreach (var individual in progenyIndividualList)
        {
            for (var index = 0; index < individual.GeneList.Count; index++)
            {
                individual.GeneList[index] = Mathf.Clamp(individual.GeneList[index], MinTorque, MaxTorque);
            }
        }

        return progenyIndividualList;
    }

    // 次世代の生成
    private List<Individual> GenerateNextGeneration(List<Individual> currentGenerationIndividualList, List<Individual> eliteIndividualList, List<Individual> progenyIndividualList)
    {
        var nextGenerationIndividualList = new List<Individual>();

        // エリート遺伝子の受け継ぎ
        foreach (var individual in eliteIndividualList)
        {
            nextGenerationIndividualList.Add(Instantiate(_individualPrefab, new Vector3(nextGenerationIndividualList.Count(), 0, 0), Quaternion.identity).GetComponent<IndividualMonobehaviour>().Indivudual);

            nextGenerationIndividualList.Last().GeneList = individual.GeneList;

            nextGenerationIndividualList.Last().Evaluation = individual.Evaluation;

            nextGenerationIndividualList.Last().IndividualMonobehaviour.gameObject.name = "Elite";
        }

        // 子孫遺伝子の受け継ぎ
        foreach (var individual in progenyIndividualList)
        {
            nextGenerationIndividualList.Add(Instantiate(_individualPrefab, new Vector3(nextGenerationIndividualList.Count(), 0, 0), Quaternion.identity).GetComponent<IndividualMonobehaviour>().Indivudual);

            nextGenerationIndividualList.Last().GeneList = individual.GeneList;

            nextGenerationIndividualList.Last().Evaluation = individual.Evaluation;

            nextGenerationIndividualList.Last().IndividualMonobehaviour.gameObject.name = "Progeny";
        }

        // 現行遺伝子の受け継ぎ
        foreach (var individual in currentGenerationIndividualList.Take((int)(IndividualNum * OldGenerationRate)))
        {
            nextGenerationIndividualList.Add(Instantiate(_individualPrefab, new Vector3(nextGenerationIndividualList.Count(), 0, 0), Quaternion.identity).GetComponent<IndividualMonobehaviour>().Indivudual);

            nextGenerationIndividualList.Last().GeneList = individual.GeneList;

            nextGenerationIndividualList.Last().Evaluation = individual.Evaluation;

            nextGenerationIndividualList.Last().IndividualMonobehaviour.gameObject.name = "OldGeneration";
        }

        IndividualList.ForEach(individual=>Destroy(individual.IndividualMonobehaviour.gameObject));

        IndividualList.Clear();

        return nextGenerationIndividualList;
    }
}
