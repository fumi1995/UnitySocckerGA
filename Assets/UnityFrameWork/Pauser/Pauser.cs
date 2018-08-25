using System.Collections.Generic;
using System.Linq;
using SubScripts.Base;
using UnityEngine;
using UnityEngine.AI;

/* Pauser
    指定したオブジェクト以外にポーズ処理をします
*/

// ReSharper disable once CheckNamespace
public class Pauser : SingletonMonobehaviour<Pauser>
{
    private readonly List<RigidBodyVelocity> _stoppingRigidBodyVelocityList = new List<RigidBodyVelocity>();

    public List<GameObject> IgnoreGameObjects = new List<GameObject>();

    public bool IsPauseing { get; private set; }

    public string Key { get; private set; }

    public void Pause(string key)
    {
        if (IsPauseing)return;

        Key = key;

        IsPauseing = true;

        _stoppingRigidBodyVelocityList.Clear();

        foreach (var stopRigidbody in FindObjectsOfType<Rigidbody>()
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<Rigidbody>())
                .Where(obj => obj != null)))
        {
            _stoppingRigidBodyVelocityList.Add(new RigidBodyVelocity(stopRigidbody));

            stopRigidbody.Sleep();
        }

        foreach (var navMeshAgent in FindObjectsOfType<NavMeshAgent>()
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<NavMeshAgent>())
                .Where(obj => obj != null)))
            navMeshAgent.enabled = false;

        foreach (var stopAnimator in FindObjectsOfType<Animator>()
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<Animator>())
                .Where(obj => obj != null)))
            stopAnimator.enabled = false;
        
    }

    public void Resume(string key)
    {
        if(!IsPauseing)return;

        if(Key!=key)return;

        IsPauseing = false;

        foreach (var stopRigidbody in FindObjectsOfType<Rigidbody>()
            .Where(rbody => rbody.IsSleeping())
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<Rigidbody>())
                .Where(rbody => rbody != null)))
        {
            stopRigidbody.WakeUp();

            var rigidBodyVelocity = _stoppingRigidBodyVelocityList
                .SingleOrDefault(rbodyVelocity => rbodyVelocity.StoppingRigidbody == stopRigidbody);

            if (rigidBodyVelocity == null) continue;

            stopRigidbody.velocity = rigidBodyVelocity.Velocity;

            stopRigidbody.angularVelocity = rigidBodyVelocity.AngularVelocity;
        }
        
        foreach (var navMeshAgent in FindObjectsOfType<NavMeshAgent>()
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<NavMeshAgent>())
                .Where(obj => obj != null)))
            navMeshAgent.enabled = true;

        foreach (var stopAnimator in FindObjectsOfType<Animator>()
            .Except(IgnoreGameObjects
                .Select(obj => obj.GetComponent<Animator>())
                .Where(obj => obj != null)))
            stopAnimator.enabled = true;
        
    }

    // RigidBodyの速度データ格納用クラス
    internal class RigidBodyVelocity
    {
        public Vector3 AngularVelocity;
        public Rigidbody StoppingRigidbody;

        public Vector3 Velocity;

        public RigidBodyVelocity(Rigidbody rbody)
        {
            StoppingRigidbody = rbody;
            Velocity = rbody.velocity;
            AngularVelocity = rbody.angularVelocity;
        }
    }
}