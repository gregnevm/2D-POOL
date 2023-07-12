using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Ball _mainBall;
    [SerializeField] private GameObject _cueStickPivot;
    
    private static List<Ball> _balls = new();
    private static BoardState state = BoardState.Turn;

    private bool _cueRotationIsCorrect;
    private enum BoardState
    {
        Turn,
        OnMove,        
    }

    private void Update()
    {        
        switch (state)
        {
            case BoardState.OnMove:
                UnDrawCue();
                CheckBallsForMoving();
                break;
            case BoardState.Turn:
                DrawCue();
                if(!_cueRotationIsCorrect)  CorrectCueRoatation();
                ChangeCueDirection();
                break;            
        }
    }

    private void ChangeCueDirection()
    {
        DrawCue();
        
        float horizontalInput = Input.GetAxis("Horizontal");
        Quaternion rotation = _cueStickPivot.transform.rotation;

        if (horizontalInput != 0f)
        {
            rotation *= Quaternion.Euler(0f, horizontalInput, 0f);
            _cueStickPivot.transform.rotation = rotation;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Strike();
        }
    }

    private void CorrectCueRoatation()
    {
        var rotation = Quaternion.Euler(-_mainBall.transform.rotation.x, 0f, -_mainBall.transform.rotation.y);
        _cueStickPivot.transform.rotation = rotation;
        _cueRotationIsCorrect = true;
    }

    private void Strike()
    {
        Vector3 direction = _cueStickPivot.transform.forward *-1;
        UnDrawCue();
        _mainBall.AddDirectionForce(direction);
        OnMoveStarted();              
    }

    public static void OnMoveStarted()
    {        
        state = BoardState.OnMove;
    }
    public static void RemoveBall(Ball ball)
    {
        _balls.Remove(ball);
    }
    public static void AddBall(Ball ball)
    {
        _balls.Add(ball);
    }

    private void CheckBallsForMoving()
    {
        bool anyBallOnMove = false;

        foreach (var ball in _balls)
        {
            if (ball.OnMoving)
            {
                anyBallOnMove = true;
                break;
            }
        }

        state = anyBallOnMove ? BoardState.OnMove : BoardState.Turn;
    }

    private void DrawCue()
    {        
        _cueStickPivot.SetActive(state == BoardState.Turn);
    }

    private void UnDrawCue()
    {
        _cueRotationIsCorrect = false;
        _cueStickPivot.SetActive(false);
    }
}
