using SubScripts.Base;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    public AudioSource Audio;

    protected override void Awake()
    {
        base.Awake();

        Initialization();
    }

    public override void Initialization()
    {
        base.Initialization();

        Audio = GetComponent<AudioSource>();
    }
}
