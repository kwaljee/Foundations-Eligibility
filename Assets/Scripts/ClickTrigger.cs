using System;
using System.Collections.Generic;
using UnityEngine;


public class ClickTrigger : MonoBehaviour
{
	TicTacToeAI _ai;

	[SerializeField]
	private int _myCoordX = 0;
	[SerializeField]
	private int _myCoordY = 0;

	[SerializeField]
	private bool canClick;


	private int aiRandXPos;
	private int aiRandYPos;
	
	[SerializeField]
	GameObject [] _Cube;

	private void Awake()
	{
		_ai = FindObjectOfType<TicTacToeAI>();
	}

	private void Start(){
		_ai.onGameStarted.AddListener(AddReference);
		_ai.onGameStarted.AddListener(() => SetInputEndabled(true));
		_ai.onPlayerWin.AddListener((win) => SetInputEndabled(false));
	}

	private void SetInputEndabled(bool val){
		canClick = val;
		
	}

	private void AddReference()
	{
		_ai.RegisterTransform(_myCoordX, _myCoordY, this);
		canClick = true;
	}

	

	private void OnMouseDown()
	{
		if (canClick)
		{
			_ai.PlayerSelects(_myCoordX, _myCoordY);
			//Make sure no piece can be put on top of another piece on players turn
			canClick = false;
			
			SwitchTurns();
		}
	}

	private void SwitchTurns()
	{
		//aiRandXPos = UnityEngine.Random.Range(0, 3);
		//aiRandYPos = UnityEngine.Random.Range(0, 3);
		//_ai.AiSelects(aiRandXPos, aiRandYPos);
	}


}
