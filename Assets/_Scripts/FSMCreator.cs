using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;


public class FSMCreator : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _waitTime;

    private void Start()
    {
        PlayMakerFSM playMaker = _button.gameObject.AddComponent<PlayMakerFSM>();
        playMaker.enabled = false;
        var fsm = playMaker.Fsm;
        FsmState initState = new FsmState(fsm);
        initState.Name = "idle";

        FsmState showMessageState = new FsmState(fsm);
        showMessageState.Name = "ShowMsg";

        FsmState waitState = new FsmState(fsm);
        waitState.Name = "Wait";

        FsmTransition fromIdleToWait = new FsmTransition();
        FsmEvent uiEvent = new FsmEvent(FsmEvent.GetFsmEvent(FsmEvent.UiClick));
        fromIdleToWait.FsmEvent = uiEvent;
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

        ShowDebugLogAction showDebugLogAction = new ShowDebugLogAction("hello world");
        showMessageState.Actions = new FsmStateAction[] { showDebugLogAction };
        FsmTransition fromMsgToIdle = new FsmTransition();
        fromMsgToIdle.FsmEvent = new FsmEvent(FsmEvent.GetFsmEvent(FsmEvent.Finished));
        fromMsgToIdle.ToFsmState = initState;
        showMessageState.Transitions = new FsmTransition[] { fromMsgToIdle };


        fsm.States = new FsmState[] { initState, waitState, showMessageState };
        fsm.StartState = initState.Name;
        playMaker.enabled = true;
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
