using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState{none, cross, circle}

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{

	int _aiLevel;
	

	TicTacToeState[,] boardState;

	[SerializeField]
	private bool _isPlayerTurn;

	[SerializeField]
	private int _gridSize = 3;
	
	[SerializeField]
	private TicTacToeState playerState = TicTacToeState.circle;
	TicTacToeState aiState = TicTacToeState.cross;

	[SerializeField]
	private GameObject _xPrefab;

	[SerializeField]
	private GameObject _oPrefab;

	public UnityEvent onGameStarted;

	//Call This event with the player number to denote the winner
	public WinnerEvent onPlayerWin;

	ClickTrigger[,] _triggers;
	


	private int aiRandXPos; //assign random x pos for ai turn
	private int aiRandYPos; //assign random y pos for ai turn



	private void Awake()
	{
		if(onPlayerWin == null){
			onPlayerWin = new WinnerEvent();
		}
	}

	public void StartAI(int AILevel){
		_aiLevel = AILevel;
		StartGame();
	}

	public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
	{
		_triggers[myCoordX, myCoordY] = clickTrigger;
	}

	private void StartGame()
	{
		_triggers = new ClickTrigger[3,3];
		boardState = new TicTacToeState[_gridSize, _gridSize];
		onGameStarted.Invoke();
	}

	public void PlayerSelects(int coordX, int coordY){
		
		//Turn player selection to false
		_triggers[coordX, coordY].canClick = false;	
		
		//Place Circle in spot selected by player
		SetVisual(coordX, coordY, playerState);

		
		//Track which positions have already been selected by the player
		TrackSelection(coordX, coordY, playerState);

		//Switch turns for AI to move
		AiTurn(coordX, coordY, aiState);
	}

	private void TrackSelection(int coordX, int coordY, TicTacToeState spotTaken)
	{
		SetVisual(coordX, coordY, spotTaken);
		boardState[coordX, coordY] = spotTaken;

		int winner = CheckWinState(boardState, spotTaken);

		if (winner == -1)
		{
			return;
		}
		else
		{
			onPlayerWin.Invoke(winner);
		}

	}

	
    public void AiTurn(int coordX, int coordY, TicTacToeState aiState)
	{
		aiRandXPos = UnityEngine.Random.Range(0, 3);
		aiRandYPos = UnityEngine.Random.Range(0, 3);

		//Player cannot play on own piece or piece placed by AI, however, AI can place on itself?
		if (_triggers[coordX, coordY].canClick == false && _triggers[aiRandXPos, aiRandYPos].canClick == false)
		{	
			//recursive call to keep randomizing options
			AiTurn(coordX, coordY, aiState);
		}
		else
		{
			//If conditions are true, allow AI to select board
			AiSelects(aiRandXPos, aiRandYPos);
		}

	}

	public void AiSelects(int coordX, int coordY)
	{
		_triggers[coordX, coordY].canClick = false;
		_triggers[aiRandXPos, aiRandYPos].canClick = false;
		TrackSelection(coordX, coordY, aiState);
		SetVisual(coordX, coordY, aiState);
	}


	private int CheckWinState(TicTacToeState[,] board, TicTacToeState currentPlayerState)
	{
		//Check for a winning state on the board either horizontally, vertically, or diagonally
		if ((board[0, 0] == currentPlayerState && board[0, 1] == currentPlayerState && board[0, 2] == currentPlayerState) ||
			(board[1, 0] == currentPlayerState && board[1, 1] == currentPlayerState && board[1, 2] == currentPlayerState) ||
			(board[2, 0] == currentPlayerState && board[2, 1] == currentPlayerState && board[2, 2] == currentPlayerState) ||
			(board[0, 0] == currentPlayerState && board[1, 0] == currentPlayerState && board[2, 0] == currentPlayerState) ||
			(board[0, 1] == currentPlayerState && board[1, 1] == currentPlayerState && board[2, 1] == currentPlayerState) ||
			(board[0, 2] == currentPlayerState && board[1, 2] == currentPlayerState && board[2, 2] == currentPlayerState) ||
			(board[0, 0] == currentPlayerState && board[1, 1] == currentPlayerState && board[2, 2] == currentPlayerState) ||
			(board[2, 0] == currentPlayerState && board[1, 1] == currentPlayerState && board[0, 2] == currentPlayerState))
		{
			return currentPlayerState.GetHashCode();
		}

		//Check if there are any moves left on the board
		foreach (var boardItem in board)
		{
			if (boardItem == TicTacToeState.none) return -1;
		}

		//Check to see if there is a tie
		return 0;
	}


	private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
	{
		Instantiate(
			targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
			_triggers[coordX, coordY].transform.position,
			Quaternion.identity
        );
	}

	



}
