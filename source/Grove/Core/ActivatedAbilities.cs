﻿namespace Grove.Core
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Mana;
  using Modifiers;

  [Copyable]
  public class ActivatedAbilities : IModifiable, IHashable
  {
    private readonly TrackableList<ActivatedAbility> _abilities;
    private readonly TrackableList<IManaSource> _manaSources;

    private ActivatedAbilities() {}

    public ActivatedAbilities(IEnumerable<ActivatedAbility> abilities, ChangeTracker changeTracker,
      IHashDependancy hashDependancy)
    {
      _abilities = new TrackableList<ActivatedAbility>(abilities, changeTracker, hashDependancy);

      var manaSources = abilities
        .Where(x => x is IManaSource)
        .Cast<IManaSource>();

      _manaSources = new TrackableList<IManaSource>(manaSources, changeTracker);
    }

    public IList<IManaSource> ManaSources { get { return _manaSources; } }
    public IEnumerable<ManaAbility> ManaAbilities { get { return _abilities.Where(x => x is ManaAbility).Cast<ManaAbility>(); } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Activate(int abilityIndex, ActivationParameters activationParameters)
    {
      _abilities[abilityIndex].Activate(activationParameters);
    }

    public List<ActivationPrerequisites> CanActivate(bool ignoreManaAbilities)
    {
      var result = new List<ActivationPrerequisites>();

      for (int index = 0; index < _abilities.Count; index++)
      {
        var ability = _abilities[index];
        
        if (ignoreManaAbilities && ability is ManaAbility)
        {
          continue;
        }

        ActivationPrerequisites prerequisites;
        if (ability.CanActivate(out prerequisites))
        {
          prerequisites.Index = index;          
          result.Add(prerequisites);
        }
      }

      return result;
    }

    public IManaAmount GetManaCost(int index)
    {
      return _abilities[index].GetManaCost();
    }

    public void EnableAll()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.IsEnabled = true;
      }
    }

    public void DisableAll()
    {
      foreach (var activatedAbility in _abilities)
      {
        activatedAbility.IsEnabled = false;
      }
    }

    public void Add(ActivatedAbility ability)
    {
      _abilities.Add(ability);
      AddManaSource(ability);
    }

    private void AddManaSource(ActivatedAbility ability)
    {
      var manaSource = ability as IManaSource;
      if (manaSource == null)
        return;

      _manaSources.Add(manaSource);
      ability.OwningCard.Controller.AddManaSource(manaSource);
    }

    public void Remove(ActivatedAbility ability)
    {
      _abilities.Remove(ability);

      RemoveManaSource(ability);
    }

    private void RemoveManaSource(ActivatedAbility ability)
    {
      var manaSource = ability as IManaSource;
      if (manaSource == null)
        return;

      _manaSources.Remove(manaSource);
      ability.OwningCard.Controller.RemoveManaSource(manaSource);
    }

    public ActivatedAbility RemoveFirst()
    {
      var ability = _abilities.First();
      _abilities.Remove(ability);
      RemoveManaSource(ability);

      return ability;
    }
  }
}