using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;

using System.Collections.Generic;
using System.IO;

namespace FactoryBuilder.Parts
{
    public class Entity
    {
        protected SaveEntity entity;
        protected List<SaveComponent> components;

        protected Entity(string protoName)
        {
            using var stream = new FileStream($"./proto/{protoName}.bin", FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(stream);
            entity = new SaveEntity(reader);
            entity.ParseData(reader.ReadInt32(), reader, 0);
            components = new List<SaveComponent>();
            for (int i = 0; i < entity.Components.Count; ++i)
            {
                components.Add(new SaveComponent(reader));
                components[i].ParseData(reader.ReadInt32(), reader, 0);
            }
        }

        public Vector3 Transform { get => entity.Position; set => entity.Position = value; }

    }
}
