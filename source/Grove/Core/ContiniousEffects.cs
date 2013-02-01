﻿namespace Grove.Core
{
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class ContiniousEffects : IModifiable
  {
    private readonly TrackableList<ContinuousEffect> _continiousEffects = new TrackableList<ContinuousEffect>();

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _continiousEffects.Initialize(changeTracker, hashDependancy);
    }

    public void Add(ContinuousEffect effect)
    {
      _continiousEffects.Add(effect);
    }

    public void Remove(ContinuousEffect effect)
    {
      _continiousEffects.Remove(effect);
      effect.Deactivate();
    }

    public void Activate()
    {
      foreach (var continiousEffect in _continiousEffects)
      {
        continiousEffect.Activate();
      }
    }

    public void Deactivate()
    {
      foreach (var continiousEffect in _continiousEffects)
      {
        continiousEffect.Deactivate();
      }
    }
  }
}