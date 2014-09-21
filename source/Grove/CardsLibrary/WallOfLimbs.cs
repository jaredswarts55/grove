﻿namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Costs;
    using Effects;
    using Modifiers;
    using Triggers;

    public class WallOfLimbs : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Wall of Limbs")
                .ManaCost("{2}{B}")
                .Type("Creature - Zombie Wall")
                .Text("{Defender}{I}(This creature can't attack.){/I}{EOL}Whenever you gain life, put a +1/+1 counter on Wall of Limbs.{EOL}{5}{B}{B},Sacrifice Wall of Limbs: Target player loses X life, where X is Wall of Limbs's power.")
                .FlavorText("\"If you cannot turn your enemy's strength to weakness, then make that strength your own.\"{EOL}—Gresha, warrior sage")
                .Power(0)
                .Toughness(3)
                .SimpleAbilities(Static.Defender)
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever you gain life, put a +1/+1 counter on Wall of Limbs.";

                    p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1));

                    p.Trigger(new OnLifeChanged((ability, player) => ability.OwningCard.Controller == player));                    

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{5}{B}{B},Sacrifice Wall of Limbs: Target player loses X life, where X is Wall of Limbs's power.";
                    
                    p.Cost = new AggregateCost(
                      new PayMana("{5}{B}{B}".Parse(), ManaUsage.Abilities),
                      new Sacrifice());

                    p.Effect = () => new TargetPlayerLoosesLife(P(e => e.Source.OwningCard.Power.GetValueOrDefault()));

                    p.TargetSelector.AddEffect(trg => trg.Is.Player());

                    p.TargetingRule(new EffectDealDamage(tp => tp.Card.Power.GetValueOrDefault()));                    
                    p.TimingRule(new OnSecondMain());
                });
        }
    }
}
