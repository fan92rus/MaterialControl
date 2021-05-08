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
            Console.WriteLine("Hello World!");
        }
    }

    class MaterialController
    {
        public class TargetData
        {
            public TargetData(Entity entity, double volume)
            {
                Entity = entity;
                Volume = volume;
            }

            public Entity Entity { get; set; }
            public double Volume { get; set; }
        }
        public List<TargetData> Check(Entity entity, double volume)
        {
            List<TargetData> targets = new List<TargetData>();
            if (!entity.Materials.Any()) targets.Add(new TargetData(entity, volume));
            
            targets.AddRange(entity.Materials.SelectMany(x => Check(x.Entity, x.Volume)));

            return targets;
        }

    }

    class Entity
    {
        public double Cost { get; set; }
        public string Label { get; set; }
        public MaterialGroup MaterialGroup { get; set; }
        public int Id { get; set; }
        public ICollection<MaterialController.TargetData> Materials { get; set; }
    }

    class MaterialGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
