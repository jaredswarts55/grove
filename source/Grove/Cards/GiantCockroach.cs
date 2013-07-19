﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Misc;

  public class GiantCockroach : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Giant Cockroach")
        .ManaCost("{3}{B}")
        .Type("Creature Insect")
        .FlavorText("If the sun ever hit the swamp, where would these scurry to?")
        .Power(4)
        .Toughness(2);
    }
  }
}