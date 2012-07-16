﻿namespace Grove.Core.Details.Cards
{
  using Effects;
  using Mana;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private IManaAmount _manaAmount;

    public ManaAbility()
    {
      UsesStack = false;
    }

    public int Priority { get; set; }
    object IManaSource.Resource { get { return OwningCard; } }

    public void Consume(IManaAmount amount)
    {
      Cost.Pay(target: null, x: null);

      if (amount.Converted == _manaAmount.Converted)
        return;

      // if effect produces more mana than needed 
      // add overflow mana to manapool.          
      var manaBag = new ManaBag(amount);
      manaBag.Consume(amount);
      Controller.AddManaToManaPool(manaBag.Amount);
    }

    public IManaAmount GetAvailableMana()
    {
      var prerequisites = CanActivate();
      return prerequisites.CanBeSatisfied ? _manaAmount : ManaAmount.Zero;
    }

    public override SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      if (IsEnabled && Cost.CanPay(ref maxX))
      {
        return new SpellPrerequisites
          {
            CanBeSatisfied = true,
            Description = Text,
            Timming = delegate { return true; },
            IsManaSource = true
          };
      }

      return new SpellPrerequisites {CanBeSatisfied = false};
    }

    public void SetManaAmount(IManaAmount manaAmount)
    {
      Effect(new Effect.Factory<AddManaToPool>
        {
          Game = Game,
          Init = (effect, _) => effect.Mana = manaAmount
        });

      _manaAmount = manaAmount;
    }
  }
}