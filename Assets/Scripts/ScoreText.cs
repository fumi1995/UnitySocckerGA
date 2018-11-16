using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMesh))]
public class ScoreText : MonoBehaviour {

    [SerializeField]
    private TextMesh _textMesh;

    public void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    public void SetText(string text)
    {
        _textMesh.text = text;
    }
}
