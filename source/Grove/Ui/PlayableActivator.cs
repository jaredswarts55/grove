﻿namespace Grove.Ui
{
  using System;
  using Core.Cards;
  using Core.Decisions.Results;

  public class PlayableActivator
  {
    public Func<ActivationParameters, Playable> GetPlayable;
    public SpellPrerequisites Prerequisites;
  }
}