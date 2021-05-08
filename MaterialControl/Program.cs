using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MaterialControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Entity table = new Entity("Стол")
            {
                Materials = new List<MaterialController.Specification>()
                {
                    new MaterialController.Specification(
                        new Entity("Ножка деревяння")
                        {
                            Cost = 300d, MaterialGroup = new MaterialGroup() {Name="Ножки"}
                        }, 4, 0),
                    new MaterialController.Specification(
                        new Entity("Шуруп")
                        {
                            Cost = 30d, MaterialGroup = new MaterialGroup() {Name = "Шурупы"}
                        }, 20, 2),
                    new MaterialController.Specification(
                        new Entity("Спермоклей")
                        {
                            Cost = 20d, MaterialGroup = new MaterialGroup() {Name = "Клей"}
                        }, 200,.4),
                }
            };
            var pva = new Entity("ПВА")
            {
                Cost = 20d,
                MaterialGroup = new MaterialGroup() { Name = "Клей" },
            };

            Entity knife = new Entity("Нож картонный")
            {
                Materials = new List<MaterialController.Specification>()
                {
                    new MaterialController.Specification(
                        new Entity("Картон")
                        {
                            Cost = 300d, MaterialGroup = new MaterialGroup() {Name="Бумажные изделия"}
                        }, 4, 0),
                    new MaterialController.Specification(pva, 200,53),
                }
            };

            MaterialController materialController = new MaterialController();

            var materials = materialController.Check(table, 20);
            var sumCost = materials.Sum(x => x.Entity.Cost * x.Volume);
            Console.WriteLine($"Сумарные траты на 20 столов {sumCost} Р");
            Console.WriteLine("Hello World!");
        }
    }

    class MaterialController
    {
        public class Specification
        {
            public Specification(Entity entity, double volume, double percentOfLoss)
            {
                Entity = entity;
                Volume = volume;
                PercentOfLoss = percentOfLoss;
            }

            public Entity Entity { get; set; }
            public double Volume { get; set; }
            public double PercentOfLoss { get; }
        }
        public List<Specification> Check(Entity entity, double volume)
        {
            var targets = new List<Specification>();

            if (entity.Materials == null || !entity.Materials.Any()) targets.Add(new Specification(entity, volume, 0));

            if (entity.Materials != null) targets.AddRange(entity.Materials.SelectMany(x => Check(x.Entity, x.Volume * volume)));

            return targets;
        }

    }

    class Entity
    {
        public Entity(string label)
        {
            this.Label = label;
        }
        public double Cost { get; set; }
        public string Label { get; set; }
        public MaterialGroup MaterialGroup { get; set; }
        public int Id { get; set; }
        public ICollection<MaterialController.Specification> Materials { get; set; }
    }

    class MaterialGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
