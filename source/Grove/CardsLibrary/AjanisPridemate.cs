﻿namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    class AjanisPridemate : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Ajani's Pridemate")
              .ManaCost("{1}{W}")
              .Type("Creature - Cat Soldier")
              .Text("Whenever you gain life, you may put a +1/+1 counter on Ajani's Pridemate.")
              .FlavorText("\"When one of us prospers, the pride prospers.\"{EOL}—Jazal Goldmane")
              .Power(2)
              .Toughness(2)
              .TriggeredAbility(p =>
              {
                  p.Text = "Whenever you gain life, you may put a +1/+1 counter on Ajani's Pridemate.";
                  
                  p.Trigger(new OnLifeChanged((ability, player) => ability.OwningCard.Controller == player));

                  p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1));

                  p.TriggerOnlyIfOwningCardIsInPlay = true;
              });
        }
    }
}
