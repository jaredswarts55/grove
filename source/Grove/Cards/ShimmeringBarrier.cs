﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class ShimmeringBarrier : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Shimmering Barrier")
        .ManaCost("{1}{W}")
        .Type("Creature Wall")
        .Text("{Defender}, {First strike}{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(1)
        .Toughness(3)
        .Cycling("{2}")
        .SimpleAbilities(Static.Defender, Static.FirstStrike);
    }
  }
}