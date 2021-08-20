using NLog;
using SatisfactorySaveParser;
using FactoryBuilder.Parts;
using SatisfactorySaveParser.Structures;

using System;
using System.Collections.Generic;
using System.IO;

namespace FactoryBuilder
{
    class FactoryBuilder
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var save = new SatisfactorySave("C:/Users/Li/AppData/Local/FactoryGame/Saved/SaveGames/76561198315996096/proto.sav");
            //ExtractPrototype(save);
            var mach0 = new Constructor();
            mach0.Transform = new Vector3(0, 0, 0);

        }

        static void ExtractPrototype(SatisfactorySave save)
        {
            string[] names = { "Constructor", "Assembler", "Smelter", "Foundry", "Manufacturer" };
            SaveEntity[] entities = new SaveEntity[names.Length];
            List<SaveComponent>[] components = new List<SaveComponent>[names.Length];
            for (int i = 0; i < names.Length; ++i)
            {
                components[i] = new List<SaveComponent>();
            }
            foreach (var obj in save.Entries)
            {
                for (int i = 0; i < names.Length; ++i)
                {
                    if (obj.InstanceName.Contains(names[i]))
                    {
                        if (obj is SaveEntity)
                        {
                            entities[i] = (SaveEntity)obj;
                            logger.Info($"Extracted {names[i]} prototype");
                        } 
                        else {
                            components[i].Add((SaveComponent)obj);
                        }
                        break;
                    }
                }
            }
            for (int i = 0; i < names.Length; ++i)
            {
                using var stream = new FileStream($"./proto/{names[i]}.bin", FileMode.OpenOrCreate, FileAccess.Write);
                using var writer = new BinaryWriter(stream);
                entities[i].SerializeHeader(writer);
                using (var ms = new MemoryStream())
                using (var dataWriter = new BinaryWriter(ms))
                {
                    entities[i].SerializeData(dataWriter, 0);
                    var bytes = ms.ToArray();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                }
                foreach (var component in components[i])
                {
                    component.SerializeHeader(writer);
                    using var ms = new MemoryStream();
                    using var dataWriter = new BinaryWriter(ms);
                    component.SerializeData(dataWriter, 0);
                    var bytes = ms.ToArray();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                }
            }
        }
    }
}
