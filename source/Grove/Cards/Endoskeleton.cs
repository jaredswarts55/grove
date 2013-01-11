﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class Endoskeleton : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Endoskeleton")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "You may choose not to untap Endoskeleton during your untap step.{EOL}{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.")        
        .MayChooseNotToUntapDuringUntap()
        .Abilities(
          ActivatedAbility(
            "{2},{T}: Target creature gets +0/+3 for as long as Endoskeleton remains tapped.",
            Cost<PayMana, Tap>(cost => cost.Amount = 2.Colorless()),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Toughness = 3;
                  m.AddLifetime(Lifetime<PermanentGetsUntapedLifetime>(l => l.Permanent = m.Source));
                }))),
            Target(Validators.Card(x => x.Is().Creature), Zones.Battlefield()),
            targetingAi: TargetingAi.IncreasePowerAndToughness(0, 3, untilEot: false),
            timing: Timings.NoRestrictions()));
    }
  }
}