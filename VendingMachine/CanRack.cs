using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Diagnostics;

namespace VendingMachine
{
    public class CanRack
    {
        private int maxInventory;
        private List<Can> cans = new List<Can>();

        public CanRack()
        {
            this.Cans = new List<Can>();
        }
        public CanRack(int inventory)
        {
            this.maxInventory = inventory;
            this.Cans = new List<Can>();
        }

        public List<Can> Cans
        {
            get { return cans; }
            set { cans = value; }
        }

        public int MaxInventory
        {
            get { return maxInventory; }
            set { maxInventory = value; }
        }

        public void AddACanOf(Flavor FlavorOfCanToBeAdded)
        {
            if (!IsFull(FlavorOfCanToBeAdded))
            {
                this.cans.Add(new Can(FlavorOfCanToBeAdded));
            }
        }
       

        public void RemoveACanOf(Flavor FlavorOfCanToBeRemoved)
        {
            if (!IsEmpty(FlavorOfCanToBeRemoved))
            {
                this.cans.Remove(this.cans.FirstOrDefault(x => x.Flavor == FlavorOfCanToBeRemoved));
            }
        }

        public void FillTheCanRack()
        {
            foreach (Flavor f in Enum.GetValues(typeof(Flavor)))
            {                
                while (!IsFull(f))
                {
                    this.cans.Add(new Can(f));
                }
            }
        }

        public void EmptyCanRackOf(Flavor flavor)
        {
            this.Cans.RemoveAll(x => x.Flavor == flavor);
        }

        public bool IsFull(Flavor flavor)
        {
            return this.cans.Where(x=>x.Flavor==flavor).Count() >= this.maxInventory;
        }

        public bool IsEmpty(Flavor flavor)
        {
            return this.cans.Where(x => x.Flavor == flavor).Count() == 0;
        }

        public List<Content> Contents()
        { 
            var content =  this.cans.GroupBy(x => x.Flavor)
                .Select(g => new Content { Flavor = g.Key, Amount = g.Count() }).ToList();

            foreach (Flavor f  in Enum.GetValues(typeof(Flavor)))
            {
                if (content.FirstOrDefault(x=>x.Flavor==f) is null)
                {
                    content.Add(new Content()
                    {
                        Flavor = f,
                        Amount = 0
                    });
                }
            }
            return content;
        }
        public Content Contents(Flavor flavor)
        {
            return new Content()
            {
                Amount = cans.Count(x => x.Flavor == flavor),
                Flavor = flavor
            };
        }

        public void DebugWriteCanRackContents()
        {
            foreach (Content v in this.Contents())
            {
                var suf = v.Amount > 1 ? "s" : string.Empty;
                Debug.WriteLine($"{v.Amount} can{suf} of {v.Flavor} soda in the inventory");
            }
        }
    }
}
