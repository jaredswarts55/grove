﻿namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;

  public class SwitchController : Effect
  {
    protected override void ResolveEffect()
    {
      var opponent = Game.Players.GetOpponent(Controller);

      var modifier = Builder
        .Modifier<ChangeController>(m => m.NewController = opponent)
        .CreateModifier(Source.OwningCard, Source.OwningCard, X);

      Source.OwningCard.AddModifier(modifier);
    }
  }
}