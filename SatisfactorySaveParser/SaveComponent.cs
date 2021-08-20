using NLog;
using System.IO;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     Engine class: FObjectSaveHeader
    /// </summary>
    public class SaveComponent : SaveObject
    {
        public const int TypeID = 0;

        /// <summary>
        ///     Instance name of the parent entity object
        /// </summary>
        public string ParentEntityName { get; set; }

        public SaveComponent(string typePath, string rootObject, string instanceName) : base(typePath, rootObject, instanceName)
        {
        }

        public SaveComponent(BinaryReader reader) : base(reader)
        {
            ParentEntityName = reader.ReadLengthPrefixedString();
        }

        public override SaveObject Clone()
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new BinaryWriter(buffer, System.Text.Encoding.UTF8, true))
                {
                    SerializeHeader(writer);
                    SerializeData(writer, 0); // buildVersion is useless
                }
                buffer.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(buffer))
                {
                    var clone = new SaveComponent(reader);
                    clone.ParseData((int)buffer.Length, reader, 0);
                    return clone;
                }
            }
        }

        public override void SerializeHeader(BinaryWriter writer)
        {
            base.SerializeHeader(writer);

            writer.WriteLengthPrefixedString(ParentEntityName);
        }
    }
}
