﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Sicken : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sicken")
        .ManaCost("{B}")
        .Type("Enchantment Aura")
        .Text(
          "{Enchant creature}{EOL}Enchanted creature gets -1/-1.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("Urza dared to attack Phyrexia. Slowly, it retaliated.")        
        .Cycling("{2}")
        .Effect<Attach>(e =>
          {
            e.ToughnessReduction = 1;
            e.Modifiers(Modifier<AddPowerAndToughness>(m =>
              {
                m.Power = -1;
                m.Toughness = -1;
              }));
          })
        .Timing(Timings.MainPhases())
        .Targets(
          TargetSelectorAi.ReduceToughness(1),
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield())
        );
    }
  }
}