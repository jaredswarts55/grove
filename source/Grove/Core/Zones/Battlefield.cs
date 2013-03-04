﻿namespace Grove.Core.Zones
{
  using System.Collections.Generic;
  using System.Linq;

  public class Battlefield : UnorderedZone, IBattlefieldQuery
  {
    public Battlefield(Player owner) : base(owner) {}

    private Battlefield()
    {
      /* for state copy */
    }

    public int Score { get { return this.Sum(x => x.Score); } }

    public override Zone Zone { get { return Zone.Battlefield; } }
    public IEnumerable<Card> Attackers { get { return this.Where(card => card.IsAttacker); } }
    public IEnumerable<Card> Blockers { get { return this.Where(card => card.IsBlocker); } }
    public IEnumerable<Card> CreaturesThatCanAttack { get { return Creatures.Where(x => x.CanAttackThisTurn); } }
    public IEnumerable<Card> CreaturesThatCanBlock { get { return Creatures.Where(x => x.CanBlock()); } }
    public bool HasCreaturesThatCanAttack { get { return this.Any(card => card.CanAttackThisTurn); } }
    public IEnumerable<Card> Legends { get { return this.Where(x => x.Is().Legendary); } }

    public override void AfterAdd(Card card)
    {
      card.OnCardJoinedBattlefield();
    }

    public override void AfterRemove(Card card)
    {
      card.OnCardLeftBattlefield();
    }
  }
}