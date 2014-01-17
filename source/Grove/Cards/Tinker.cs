﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Zones;

  public class Tinker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tinker")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text(
          "As an additional cost to cast Tinker, sacrifice an artifact.{EOL}Search your library for an artifact card and put that card onto the battlefield. Then shuffle your library.")
        .FlavorText("I wonder how it feels to be bored.")
        .Cast(p =>
          {
            p.Cost = new AggregateCost(
              new PayMana("{2}{U}".Parse(), ManaUsage.Spells),
              new Sacrifice());

            p.TargetSelector.AddCost(trg =>
              trg.Is.Card(c => c.Is().Artifact).On.Battlefield());

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().Artifact,
              text: "Search your library for an artifact.");

            p.TargetingRule(new EffectRankBy(c => c.Score));
          });
    }
  }
}