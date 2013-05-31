﻿namespace Grove.Gameplay.Decisions
{
  using Misc;

  public abstract class Decision<TResult> : GameObject, IDecision
  {
    private bool _hasCompleted;
    public TResult Result { get; set; }
    protected virtual bool ShouldExecuteQuery { get { return true; } }
    public Player Controller { get; private set; }

    public virtual bool HasCompleted { get { return _hasCompleted; } }
    public virtual bool WasPriorityPassed { get { return false; } }

    public virtual void Initialize(Player controller, Game game)
    {
      Controller = controller;
      Game = game;
    }

    public virtual void Execute()
    {
      if (ShouldExecuteQuery)
      {
        ExecuteQuery();
      }
      else
      {
        SetResultNoQuery();
      }

      ProcessResults();
      SaveDecisionResult(Result);
      
      _hasCompleted = true;
    }

    public abstract void ProcessResults();
    protected abstract void ExecuteQuery();
    protected virtual void SetResultNoQuery() {}
  }
}