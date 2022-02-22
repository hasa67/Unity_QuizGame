using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunnyIdle : StateMachineBehaviour {
	private BunnyController bc;
	private List<string> actionsList;
	private string nextAction;
	private float idleTime;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		bc = animator.GetComponent<BunnyController> ();
		actionsList = bc.GetActionsList ();
		idleTime = Random.Range (3f, 5f);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		idleTime -= Time.deltaTime;

		nextAction = bc.GetNextAction ();
		if (nextAction != "") {
			bc.PerformAction (nextAction);
//			Debug.Log (nextAction);
			bc.SetNextAction ("");
		} else if (idleTime <= 0) {
			nextAction = actionsList [Random.Range (0, actionsList.Count)];
			bc.PerformAction (nextAction);
//			Debug.Log (nextAction);
			idleTime = 10f;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}
}
