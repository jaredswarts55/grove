﻿namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;
  using Targeting;

  public class ReturnTargetCardToHand : Effect
  {
    public int Discard;

    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      Target().Card().ReturnToHand();

      if (Discard > 0)
      {
        Decisions.Enqueue<DiscardCards>(
          controller: Target().Card().Controller,
          init: p => p.Count = Discard);
      }
    }
  }
}