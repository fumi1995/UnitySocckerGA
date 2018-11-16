using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(ChromosomeMonobehaviour))]
[RequireComponent(typeof(SimpleAddTorque))]
public class GeneDecoderByTime : MonoBehaviour{

    // トルク変更間隔
    private readonly float _intervalSec = 0.1f;

    private readonly Queue<float> _geneQueue = new Queue<float>();

    [SerializeField] [NotNull] private ChromosomeMonobehaviour _chromosomeMonobehaviour;

    [SerializeField] [NotNull] private SimpleAddTorque _simpleAddTorque;

    private void Reset()
    {
        _chromosomeMonobehaviour = GetComponent<ChromosomeMonobehaviour>();
        _simpleAddTorque = GetComponent<SimpleAddTorque>();
    }

    private void Awake()
    {
        _chromosomeMonobehaviour = GetComponent<ChromosomeMonobehaviour>();
        _simpleAddTorque = GetComponent<SimpleAddTorque>();
    }

    private void Start (){

        // キューにデータを突っ込む
        _chromosomeMonobehaviour.Chromosome.GeneList.ForEach(gene => _geneQueue.Enqueue(gene));

        // 一定間隔ごとにトルクを変更する
	    Observable
	        .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_intervalSec))
	        .Where(_ => _geneQueue.Count > 0)
	        .Subscribe(_ => {

	            _simpleAddTorque._torqueMagnitude = _geneQueue.Dequeue();

	        }).AddTo(gameObject);
	}
}
