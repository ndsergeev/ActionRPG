using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTargetManager : MonoBehaviour
{

    public static PlayerTargetManager Singleton;

    Player player;

    public List<PlayerTarget> targets = new List<PlayerTarget>();

    [SerializeField] CinemachineTargetGroup targetGroup1;
    [SerializeField] CinemachineTargetGroup targetGroup2;

    private void Awake()
    {
        Singleton = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeTargetGroups();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeTargetGroups()
    {
        CinemachineTargetGroup.Target target1 = new CinemachineTargetGroup.Target();

        target1.target = player.Cameras.GetLookAtPlayerTarget();
        CinemachineTargetGroup.Target target2 = new CinemachineTargetGroup.Target();

        CinemachineTargetGroup.Target[] targets = { target1, target2 };

        targetGroup1.m_Targets = targets;
        targetGroup2.m_Targets = targets;
    }

    public void AddTarget(PlayerTarget target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(PlayerTarget target)
    {
        targets.Remove(target);
        //Player.Singleton.targeting.TargetHasBeenRemoved(target);
    }

    public CinemachineTargetGroup GetTargetGroup1()
    {
        return targetGroup1;
    }

    public CinemachineTargetGroup GetTargetGroup2()
    {
        return targetGroup2;
    }
}
