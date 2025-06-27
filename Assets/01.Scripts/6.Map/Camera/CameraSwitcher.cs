using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startCam;
    [SerializeField] private CinemachineVirtualCamera battleCam;
    [SerializeField] private CinemachineVirtualCamera shopCam;
    [SerializeField] private CinemachineVirtualCamera rewardCam;
    [SerializeField] private CinemachineVirtualCamera eventCam;
    [SerializeField] private CinemachineVirtualCamera bossCam;
    
    private const int PLAY_PRIORITY = 10;
    private const int NON_PlAY_PRIORITY = 0;

    public void SwitchTo(NodeType type)
    {
        startCam.Priority = NON_PlAY_PRIORITY;
        battleCam.Priority = NON_PlAY_PRIORITY;
        shopCam.Priority = NON_PlAY_PRIORITY;
        rewardCam.Priority = NON_PlAY_PRIORITY;
        eventCam.Priority = NON_PlAY_PRIORITY;
        bossCam.Priority = NON_PlAY_PRIORITY;

        switch (type)
        {
            case NodeType.Start:
                startCam.Priority = PLAY_PRIORITY;
                break;
            case NodeType.Battle:
                battleCam.Priority = PLAY_PRIORITY;
                break;
            case NodeType.Shop:
                shopCam.Priority = PLAY_PRIORITY;
                break;
            case NodeType.Reward:
                rewardCam.Priority = PLAY_PRIORITY;
                break;
            case NodeType.Event:
                eventCam.Priority = PLAY_PRIORITY;
                break;
            case NodeType.Boss:
                bossCam.Priority = PLAY_PRIORITY;
                break;
        }
        
    }
}
