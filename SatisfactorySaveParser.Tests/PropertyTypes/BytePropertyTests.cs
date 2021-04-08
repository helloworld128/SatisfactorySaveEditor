﻿using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class BytePropertyTests
    {
        private const int BuildVersion = 139586;

        private const string ByteEnumName = "mGamePhase";
        private const string ByteEnumType = "EGamePhase";
        private const string ByteEnumValue = "EGP_EndGame";
        private static readonly byte[] ByteEnumBytes = new byte[] { 0x0B, 0x00, 0x00, 0x00, 0x6D, 0x47, 0x61, 0x6D, 0x65, 0x50, 0x68, 0x61, 0x73, 0x65, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x79, 0x74, 0x65, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00, 0x00, 0x00, 0x45, 0x47, 0x61, 0x6D, 0x65, 0x50, 0x68, 0x61, 0x73, 0x65, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x45, 0x47, 0x50, 0x5F, 0x45, 0x6E, 0x64, 0x47, 0x61, 0x6D, 0x65, 0x00 };

        private const string ByteNumberName = "mLastAutosaveId";
        private const byte ByteNumberValue = 0;
        private static readonly byte[] ByteNumberBytes = new byte[] { 0x10, 0x00, 0x00, 0x00, 0x6D, 0x4C, 0x61, 0x73, 0x74, 0x41, 0x75, 0x74, 0x6F, 0x73, 0x61, 0x76, 0x65, 0x49, 0x64, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x79, 0x74, 0x65, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x4E, 0x6F, 0x6E, 0x65, 0x00, 0x00, 0x00 };

        [TestMethod]
        public void BytePropertyEnumRead()
        {
            using var stream = new MemoryStream(ByteEnumBytes);
            using var reader = new BinaryReader(stream);

            var prop = SatisfactorySaveSerializer.DeserializeProperty(reader, BuildVersion) as ByteProperty;

            Assert.AreNotEqual(null, prop);

            Assert.AreEqual(ByteEnumName, prop.PropertyName);
            Assert.AreEqual(ByteProperty.TypeName, prop.PropertyType);

            Assert.AreEqual(0, prop.Index);

            Assert.AreEqual(true, prop.IsEnum);
            Assert.AreEqual(ByteEnumType, prop.EnumType);
            Assert.AreEqual(ByteEnumValue, prop.EnumValue);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void BytePropertyEnumWrite()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var prop = new ByteProperty(ByteEnumName)
            {
                EnumType = ByteEnumType,
                EnumValue = ByteEnumValue
            };

            SatisfactorySaveSerializer.SerializeProperty(prop, writer);

            Assert.AreEqual(16, prop.SerializedLength);
            CollectionAssert.AreEqual(ByteEnumBytes, stream.ToArray());
        }

        [TestMethod]
        public void BytePropertyNumberRead()
        {
            using var stream = new MemoryStream(ByteNumberBytes);
            using var reader = new BinaryReader(stream);

            var prop = SatisfactorySaveSerializer.DeserializeProperty(reader, BuildVersion) as ByteProperty;

            Assert.AreNotEqual(null, prop);

            Assert.AreEqual(ByteNumberName, prop.PropertyName);
            Assert.AreEqual(ByteProperty.TypeName, prop.PropertyType);

            Assert.AreEqual(0, prop.Index);

            Assert.AreEqual(false, prop.IsEnum);
            Assert.AreEqual(ByteNumberValue, prop.ByteValue);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void BytePropertyNumberWrite()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var prop = new ByteProperty(ByteNumberName)
            {
                EnumType = "None",
                ByteValue = 0
            };

            SatisfactorySaveSerializer.SerializeProperty(prop, writer);

            Assert.AreEqual(1, prop.SerializedLength);
            CollectionAssert.AreEqual(ByteNumberBytes, stream.ToArray());
        }
    }
}
