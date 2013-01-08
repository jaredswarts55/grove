﻿namespace Grove.Core.Ai
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using Cards;
  using Decisions.Results;

  public class PlayableGenerator : IEnumerable<Playable>
  {
    private readonly Game _game;
    private readonly Player _player;

    public PlayableGenerator(Player player, Game game)
    {
      _player = player;
      _game = game;
    }

    public IEnumerator<Playable> GetEnumerator()
    {
      return GetPlayables().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    private IEnumerable<Playable> GetPlayableAbilities()
    {
      var zones = _player.Battlefield.Concat(_player.Hand);

      foreach (var card in zones)
      {
        if (!card.IsVisible)
          continue;

        var abilitiesPrerequisites = card.CanActivateAbilities(ignoreManaAbilities: true);

        for (var abilityIndex = 0; abilityIndex < abilitiesPrerequisites.Count; abilityIndex++)
        {
          var prerequisites = abilitiesPrerequisites[abilityIndex];

          if (!prerequisites.CanBeSatisfied)
            continue;

          foreach (var playable in GeneratePlayables(
            card, prerequisites, p => new Decisions.Results.Ability(card, p, abilityIndex)))
          {
            yield return playable;
          }
        }
      }
    }

    private IEnumerable<Playable> GetPlayableSpells()
    {
      foreach (var card in _player.Hand)
      {
        if (!card.IsVisible)
          continue;

        var spellsPrerequisites = card.CanCast();

        for (var spellIndex = 0; spellIndex < spellsPrerequisites.Count; spellIndex++)
        {
          var prerequisites = spellsPrerequisites[spellIndex];

          if (!prerequisites.CanBeSatisfied)
            continue;

          foreach (var playable in GeneratePlayables(card, prerequisites, p => new Spell(card, p, spellIndex)))
          {
            yield return playable;
          }
        }
      }
    }

    private IEnumerable<Playable> GeneratePlayables(Card card, SpellPrerequisites prerequisites,
      Func<ActivationParameters, Playable> factory)
    {
      var activationGenerator = new ActivationGenerator(card, prerequisites, _game);

      var count = 0;
      foreach (var activationParameters in activationGenerator)
      {
        var timingParameters = new TimingParameters(
          _game,
          card,
          activationParameters);

        if (prerequisites.Timing(timingParameters))
        {
          yield return factory(activationParameters);
          count++;
        }

        if (count >= _game.Search.TargetCountLimit)
        {
          break;
        }
      }
    }


    private IEnumerable<Playable> GetPlayables()
    {
      foreach (var playable in GetPlayableSpells())
      {
        yield return playable;
      }

      foreach (var playable in GetPlayableAbilities())
      {
        yield return playable;
      }
    }
  }
}