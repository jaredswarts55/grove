﻿namespace Grove.Gameplay.Decisions.Results
{
  using System;
  using System.Runtime.Serialization;
  using Infrastructure;
  using Persistance;

  [Copyable, Serializable]
  public class ChosenPlayer : ISerializable
  {
    private ChosenPlayer() {}

    public ChosenPlayer(Player player)
    {
      Player = player;
    }

    protected ChosenPlayer(SerializationInfo info, StreamingContext context)
    {
      var ctx = (SerializationContext) context.Context;
      Player = (Player) ctx.Game.GetObject(info.GetInt32("player"));
    }

    public Player Player { get; private set; }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue("player", Player.Id);
    }
  }
}