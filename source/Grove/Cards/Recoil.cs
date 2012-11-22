﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class Recoil : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Recoil")
        .ManaCost("{1}{U}{B}")
        .Type("Instant")
        .Text("Return target permanent to its owner's hand. Then that player discards a card.")
        .FlavorText("Anything sent into a plagued world is bound to come back infected.")
        .Timing(Timings.InstantRemovalTarget())
        .Category(EffectCategories.Bounce)
        .Effect<ReturnToHand>(e =>
          {
            e.Discard = 1;
            e.ReturnTarget = true;
          })
        .Targets(
          TargetSelectorAi.Bounce(), 
          TargetValidator(TargetIs.Card(), ZoneIs.Battlefield()));
    }
  }
}