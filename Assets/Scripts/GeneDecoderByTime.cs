using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(SimpleAddTorque))]
public class GeneDecoderByTime : MonoBehaviour{

    // トルク変更間隔
    private readonly float _intervalSec = 0.1f;

    private readonly Queue<float> _geneQueue = new Queue<float>();

    [SerializeField] [NotNull] private IndividualMonobehaviour _individualMonobehaviour;
    
    private void Start (){

        // キューにデータを突っ込む
        _individualMonobehaviour.Indivudual.GeneList.ForEach(gene=>_geneQueue.Enqueue(gene));

        // 一定間隔ごとにトルクを変更する
	    Observable
	        .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(_intervalSec))
	        .Where(_ => _geneQueue.Count > 0)
	        .Subscribe(_ => {

	            _individualMonobehaviour.Indivudual.SimpleAddTorque._torqueMagnitude = _geneQueue.Dequeue();

	        }).AddTo(gameObject);
	}
}
