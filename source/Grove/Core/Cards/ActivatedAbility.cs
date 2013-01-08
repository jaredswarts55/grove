﻿namespace Grove.Core.Cards
{
  using System;
  using System.Linq;
  using Ai;
  using Costs;
  using Effects;
  using Infrastructure;
  using Mana;
  using Messages;
  using Targeting;
  using Zones;

  public class ActivatedAbility : Ability
  {
    public Zone ActivationZone = Zone.Battlefield;
    private Trackable<bool> _isEnabled;
    private TimingDelegate _timming = Timings.NoRestrictions();

    public ActivatedAbility()
    {
      UsesStack = true;
    }

    public bool ActivateOnlyAsSorcery { get; set; }
    protected Cost Cost { get; private set; }
    protected bool IsEnabled { get { return _isEnabled.Value; } private set { _isEnabled.Value = value; } }

    public IManaAmount ManaCost { get { return Cost.GetManaCost(); } }

    protected TurnInfo Turn { get { return Game.Turn; } }

    public void Activate(ActivationParameters activationParameters)
    {
      var effectParameters = new EffectParameters(this, EffectCategories, activationParameters);
      var effect = EffectFactory.CreateEffect(effectParameters, Game);

      Cost.Pay(activationParameters.Targets.Cost.FirstOrDefault(), activationParameters.X);

      if (UsesStack)
      {
        Stack.Push(effect);
        return;
      }

      effect.Resolve();

      Game.Publish(new PlayerHasActivatedAbility(this, activationParameters.Targets));
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(Cost),
        calc.Calculate(EffectFactory),
        ActivateOnlyAsSorcery.GetHashCode(),
        IsEnabled.GetHashCode());
    }

    public virtual SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      var canActivate = CanBeActivated(ref maxX);

      return canActivate
        ? new SpellPrerequisites
          {
            CanBeSatisfied = true,
            Description = Text,
            TargetSelector = TargetSelector,
            XCalculator = Cost.XCalculator,
            MaxX = maxX,
            Timing = _timming,
          }
        : SpellPrerequisites.CannotBeSatisfied;
    }

    public T CreateEffect<T>(ITarget target) where T : Effect
    {
      var activationParameters = new ActivationParameters();
      activationParameters.Targets.AddEffect(target);

      var effectParameters = new EffectParameters(this, EffectCategories, activationParameters);

      return EffectFactory.CreateEffect(effectParameters, Game) as T;
    }

    public void SetCost(ICostFactory costFactory)
    {
      Cost = costFactory.CreateCost(OwningCard, TargetSelector.Cost.FirstOrDefault(), Game);
    }

    public void Timing(TimingDelegate timing)
    {
      if (timing != null)
        _timming = timing;
    }

    private bool CanBeActivated(ref int? maxX)
    {
      return
        IsEnabled &&
          CanBeActivatedAtThisTime() &&
            Cost.CanPay(ref maxX);
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != ActivationZone)
        return false;

      if (ActivateOnlyAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
            Stack.IsEmpty;
      }

      return true;
    }

    protected virtual void Initialize() {}

    public void Enable()
    {
      IsEnabled = true;
    }

    public void Disable()
    {
      IsEnabled = false;
    }

    public class Factory<T> : IActivatedAbilityFactory where T : ActivatedAbility, new()
    {
      public Action<T> Init = delegate { };

      public ActivatedAbility Create(Card card, Game game)
      {
        var ability = new T();
        ability.OwningCard = card;
        ability.SourceCard = card;
        ability.Game = game;
        ability._isEnabled = new Trackable<bool>(true, game.ChangeTracker);

        Init(ability);
        ability.Initialize();

        return ability;
      }
    }
  }
}