﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
    
  public class Reprocess : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Reprocess")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text(
          "Sacrifice any number of artifacts, creatures, and/or lands. Draw a card for each permanent sacrificed this way.")
        .FlavorText("Everything will find its use in Phyrexia. Eventually.")
        .Cast(p =>
          {
            p.Effect = () => new DrawCardsEqualToSacrificedPermanentsCount(
              text: "Sacrifice any number of artifacts, creatures, and/or lands.",
              validator: c => c.Is().Land || c.Is().Creature || c.Is().Artifact);

            p.TimingRule(new SecondMain());
          });
    }
  }
}