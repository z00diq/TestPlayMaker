using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class FSMCreator : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _waitTime;
    [SerializeField] private string _message;
    PlayMakerFSM _playMaker;

    private void Awake()
    {
        _playMaker = _button.gameObject.AddComponent<PlayMakerFSM>();
        _playMaker.enabled = false;
        var fsm = _playMaker.Fsm;

        FsmState initState = new FsmState(fsm);
        initState.Name = "Idle";

        FsmState showMessageState = new FsmState(fsm);
        showMessageState.Name = "ShowMsg";

        FsmState waitState = new FsmState(fsm);
        waitState.Name = "Wait";

        FsmTransition fromIdleToWait = new FsmTransition();
        fromIdleToWait.ToFsmState = waitState;
        initState.Transitions = new FsmTransition[] { fromIdleToWait };

        FsmTransition fromWaitToShowMessage = new FsmTransition();
        FsmEvent fsmEvent = new FsmEvent("GoToSendMessageState");
        fromWaitToShowMessage.FsmEvent = fsmEvent;
        fromWaitToShowMessage.ToFsmState = showMessageState;
        waitState.Transitions = new FsmTransition[] { fromWaitToShowMessage };

        Wait WaitAction = new Wait();
        WaitAction.AutoName();
        WaitAction.time = _waitTime;
        WaitAction.finishEvent = fromWaitToShowMessage.FsmEvent;
        waitState.Actions = new FsmStateAction[] { WaitAction };

        ShowDebugLogAction showDebugLogAction = new ShowDebugLogAction(_message);
        showMessageState.Actions = new FsmStateAction[] { showDebugLogAction };
        FsmTransition fromMsgToIdle = new FsmTransition();
        fromMsgToIdle.FsmEvent = FsmEvent.Finished;
        fromMsgToIdle.ToFsmState = initState;
        showMessageState.Transitions = new FsmTransition[] { fromMsgToIdle };

        fsm.States = new FsmState[] { initState, waitState, showMessageState };
        fsm.StartState = initState.Name;

        fromIdleToWait.FsmEvent = FsmEvent.UiPointerClick;
        _button.onClick.AddListener(()=>_playMaker.SendEvent(FsmEvent.UiPointerClick.Name));
    }

    private void Start()
    {
        _playMaker.enabled = true;
    }

    class ShowDebugLogAction : FsmStateAction
    {
        private string _msg;

        public ShowDebugLogAction(string value)
        {
            _msg = value;
            Name = "ShowDebugLogAction";
        }

        public override void OnEnter()
        {
            Debug.Log(_msg);
            Finish();
        }
    }
}
