using JavaSerializer.Content.Object;
using JavaSerializer.Content.Object.Inteface;

namespace JavaSerializerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Simple array with [ 2001 ] :: int[]
        private readonly byte[] Sample1 = Convert.FromBase64String("rO0ABXVyAAJbSU26YCZ26rKlAgAAeHAAAAABAAAH0Q==");
        // Simple array with [ 2001, 2002 ] :: int[]
        private readonly byte[] Sample2 = Convert.FromBase64String("rO0ABXVyAAJbSU26YCZ26rKlAgAAeHAAAAACAAAH0QAAB9I=");
        // String array with [ "This", "Is", "A Test" ] :: string[]
        private readonly byte[] Sample3 = Convert.FromBase64String("rO0ABXVyABNbTGphdmEubGFuZy5TdHJpbmc7rdJW5+kde0cCAAB4cAAAAAN0AARUaGlzdAACSXN0AAZBIFRlc3Q=");
        // Integer array with [ 1, 2, 3 ] :: Integer[]
        private readonly byte[] Sample4 = Convert.FromBase64String("rO0ABXVyABRbTGphdmEubGFuZy5JbnRlZ2VyO/6XraABg+IbAgAAeHAAAAADc3IAEWphdmEubGFuZy5JbnRlZ2VyEuKgpPeBhzgCAAFJAAV2YWx1ZXhyABBqYXZhLmxhbmcuTnVtYmVyhqyVHQuU4IsCAAB4cAAAAAFzcQB+AAIAAAACc3EAfgACAAAAAw==");
        // Simple class with [{ author: "Test_Auth1", subect: "Test_Sub1", yearwritten: 2001 }] :: ReadingMaterial[]
        private readonly byte[] Sample5 = Convert.FromBase64String("rO0ABXVyABJbTFJlYWRpbmdNYXRlcmlhbDtMeJlmUd2DtQIAAHhwAAAAAXNyAA9SZWFkaW5nTWF0ZXJpYWxUHlhqoGAGAwIAA0kAC3llYXJ3cml0dGVuTAAGYXV0aG9ydAASTGphdmEvbGFuZy9TdHJpbmc7TAAHc3ViamVjdHEAfgADeHAAAAfRdAAKVGVzdF9BdXRoMXQACVRlc3RfU3ViMQ==");

        [Test]
        public void TestThatArraysWithSingleIntElementAreParsed()
        {
            var memoryStream = new MemoryStream(Sample1);
            var reader = new JavaSerializer.SerializedStreamReader(memoryStream);

            reader.Read();

            Assert.That(reader.Content, Is.Not.Null);
            Assert.That(reader.Content, Has.Count.EqualTo(1));

            var content = reader.Content[0];

            Assert.That(content, Is.InstanceOf<ArrayContent>());

            var typedContent = (ArrayContent)content;
            Assert.That(typedContent.Data, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(typedContent.ClassDescriptor, Is.Not.Null);
                Assert.That(typedContent.Data, Has.Length.EqualTo(1));
                Assert.That(typedContent.Data[0], Is.InstanceOf<int>());
                Assert.That(typedContent.Data[0], Is.EqualTo(2001));
            });
        }

        [Test]
        public void TestThatArraysWithMultipleIntElementsAreParsed()
        {
            var memoryStream = new MemoryStream(Sample2);
            var reader = new JavaSerializer.SerializedStreamReader(memoryStream);

            reader.Read();

            Assert.That(reader.Content, Is.Not.Null);
            Assert.That(reader.Content, Has.Count.EqualTo(1));

            var content = reader.Content[0];

            Assert.That(content, Is.InstanceOf<ArrayContent>());

            var typedContent = (ArrayContent)content;
            Assert.That(typedContent.Data, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(typedContent.ClassDescriptor, Is.Not.Null);
                Assert.That(typedContent.Data, Has.Length.EqualTo(2));
                Assert.That(typedContent.Data[0], Is.InstanceOf<int>());
                Assert.That(typedContent.Data[1], Is.InstanceOf<int>());
                Assert.That(typedContent.Data[0], Is.EqualTo(2001));
                Assert.That(typedContent.Data[1], Is.EqualTo(2002));
            });
        }

        [Test]
        public void TestThatArraysWithMultipleStringElementsAreParsed()
        {
            var memoryStream = new MemoryStream(Sample3);
            var reader = new JavaSerializer.SerializedStreamReader(memoryStream);

            reader.Read();

            Assert.That(reader.Content, Is.Not.Null);
            Assert.That(reader.Content, Has.Count.EqualTo(1));

            var content = reader.Content[0];

            Assert.That(content, Is.InstanceOf<ArrayContent>());

            var typedContent = (ArrayContent)content;
            Assert.That(typedContent.Data, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(typedContent.ClassDescriptor, Is.Not.Null);
                Assert.That(typedContent.Data, Has.Length.EqualTo(3));
                Assert.That(typedContent.Data[0], Is.InstanceOf<IString>());
                Assert.That(typedContent.Data[1], Is.InstanceOf<IString>());
                Assert.That(typedContent.Data[2], Is.InstanceOf<IString>());
            });

            var typedEntries = typedContent.Data.OfType<IString>().ToImmutableArray();

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].FinalString, Is.EqualTo("This"));
                Assert.That(typedEntries[1].FinalString, Is.EqualTo("Is"));
                Assert.That(typedEntries[2].FinalString, Is.EqualTo("A Test"));
                Assert.That(typedEntries, Has.Length.EqualTo(3));
            });
        }

        [Test]
        public void TestThatArraysWithMultipleIntegerElementsAreParsed()
        {
            var memoryStream = new MemoryStream(Sample4);
            var reader = new JavaSerializer.SerializedStreamReader(memoryStream);

            reader.Read();

            Assert.That(reader.Content, Is.Not.Null);
            Assert.That(reader.Content, Has.Count.EqualTo(1));

            var content = reader.Content[0];

            Assert.That(content, Is.InstanceOf<ArrayContent>());

            var typedContent = (ArrayContent)content;
            Assert.That(typedContent.Data, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(typedContent.ClassDescriptor, Is.Not.Null);
                Assert.That(typedContent.Data, Has.Length.EqualTo(3));
                Assert.That(typedContent.Data[0], Is.InstanceOf<ObjectContent>());
                Assert.That(typedContent.Data[1], Is.InstanceOf<ObjectContent>());
                Assert.That(typedContent.Data[2], Is.InstanceOf<ObjectContent>());
            });

            var typedEntries = typedContent.Data.OfType<ObjectContent>().ToImmutableArray();

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values, Is.Not.Null);
                Assert.That(typedEntries[1].Values, Is.Not.Null);
                Assert.That(typedEntries[2].Values, Is.Not.Null);
                Assert.That(typedEntries, Has.Length.EqualTo(3));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values?.Keys, Has.Count.EqualTo(1));
                Assert.That(typedEntries[1].Values?.Keys, Has.Count.EqualTo(1));
                Assert.That(typedEntries[2].Values?.Keys, Has.Count.EqualTo(1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values?.Keys.Single().Name, Is.EqualTo("value"));
                Assert.That(typedEntries[1].Values?.Keys.Single().Name, Is.EqualTo("value"));
                Assert.That(typedEntries[2].Values?.Keys.Single().Name, Is.EqualTo("value"));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values?.Values.Single(), Is.EqualTo(1));
                Assert.That(typedEntries[1].Values?.Values.Single(), Is.EqualTo(2));
                Assert.That(typedEntries[2].Values?.Values.Single(), Is.EqualTo(3));
            });

            Assert.That(typedEntries[0].Fields, Contains.Key("value"));

            var valueField = typedEntries[0].Fields["value"];

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values, Contains.Key(valueField));
                Assert.That(typedEntries[1].Values, Contains.Key(valueField));
                Assert.That(typedEntries[2].Values, Contains.Key(valueField));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].Values[valueField], Is.EqualTo(1));
                Assert.That(typedEntries[1].Values[valueField], Is.EqualTo(2));
                Assert.That(typedEntries[2].Values[valueField], Is.EqualTo(3));
            });
        }

        [Test]
        public void TestThatArraysWithSingleObjectAreParsed()
        {
            var memoryStream = new MemoryStream(Sample5);
            var reader = new JavaSerializer.SerializedStreamReader(memoryStream);

            reader.Read();

            Assert.That(reader.Content, Is.Not.Null);
            Assert.That(reader.Content, Has.Count.EqualTo(1));

            var content = reader.Content[0];

            Assert.That(content, Is.InstanceOf<ArrayContent>());

            var typedContent = (ArrayContent)content;
            Assert.That(typedContent.Data, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(typedContent.ClassDescriptor, Is.Not.Null);
                Assert.That(typedContent.Data, Has.Length.EqualTo(1));
                Assert.That(typedContent.Data[0], Is.InstanceOf<ObjectContent>());
            });

            var typedEntry = typedContent.Data.OfType<ObjectContent>().Single();

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Values, Is.Not.Null);
                Assert.That(typedEntry.Values, Has.Count.EqualTo(3));
            });

            Assert.That(typedEntry.Values?.Keys, Has.Count.EqualTo(3));

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Values.Keys.ElementAt(0).Name, Is.EqualTo("yearwritten"));
                Assert.That(typedEntry.Values.Keys.ElementAt(1).Name, Is.EqualTo("author"));
                Assert.That(typedEntry.Values.Keys.ElementAt(2).Name, Is.EqualTo("subject"));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Values.Values.ElementAt(0), Is.EqualTo(2001));
                Assert.That(typedEntry.Values.Values.ElementAt(1), Is.InstanceOf<IString>());
                Assert.That(typedEntry.Values.Values.ElementAt(2), Is.InstanceOf<IString>());
            });

            var strings = typedEntry.Values.Values.OfType<IString>().ToImmutableArray();

            Assert.Multiple(() =>
            {
                Assert.That(strings[0].FinalString, Is.EqualTo("Test_Auth1"));
                Assert.That(strings[1].FinalString, Is.EqualTo("Test_Sub1"));
            });

            Assert.That(typedEntry.Fields, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Fields, Contains.Key("yearwritten"));
                Assert.That(typedEntry.Fields, Contains.Key("author"));
                Assert.That(typedEntry.Fields, Contains.Key("subject"));
            });

            var yearField = typedEntry.Fields["yearwritten"];
            var authorField = typedEntry.Fields["author"];
            var subjectField = typedEntry.Fields["subject"];

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Values, Contains.Key(yearField));
                Assert.That(typedEntry.Values, Contains.Key(authorField));
                Assert.That(typedEntry.Values, Contains.Key(subjectField));
            });

            Assert.Multiple(() =>
            {
                Assert.That(typedEntry.Values[yearField], Is.EqualTo(2001));
                Assert.That(typedEntry.Values[authorField], Is.InstanceOf<IString>());
                Assert.That(typedEntry.Values[subjectField], Is.InstanceOf<IString>());
            });

            Assert.Multiple(() =>
            {
                Assert.That(((IString)typedEntry.Values[authorField]).FinalString, Is.EqualTo("Test_Auth1"));
                Assert.That(((IString)typedEntry.Values[subjectField]).FinalString, Is.EqualTo("Test_Sub1"));
            });
        }
    }
}