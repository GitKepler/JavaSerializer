using JavaSerializer.Content.Object;
using JavaSerializer.Content.Object.String;

namespace JavaSerializerTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        // Simple array with { 2001 } :: int[]
        private readonly byte[] Sample1 = Convert.FromBase64String("rO0ABXVyAAJbSU26YCZ26rKlAgAAeHAAAAABAAAH0Q==");
        // Simple array with { 2001, 2002 } :: int[]
        private readonly byte[] Sample2 = Convert.FromBase64String("rO0ABXVyAAJbSU26YCZ26rKlAgAAeHAAAAACAAAH0QAAB9I=");
        // String array with { "This", "Is", "A Test" } :: string[]
        private readonly byte[] Sample3 = Convert.FromBase64String("rO0ABXVyABNbTGphdmEubGFuZy5TdHJpbmc7rdJW5+kde0cCAAB4cAAAAAN0AARUaGlzdAACSXN0AAZBIFRlc3Q=");
        // Integer array with { 1, 2, 3 } :: Integer[]
        private readonly byte[] Sample4 = Convert.FromBase64String("rO0ABXVyABRbTGphdmEubGFuZy5JbnRlZ2VyO/6XraABg+IbAgAAeHAAAAADc3IAEWphdmEubGFuZy5JbnRlZ2VyEuKgpPeBhzgCAAFJAAV2YWx1ZXhyABBqYXZhLmxhbmcuTnVtYmVyhqyVHQuU4IsCAAB4cAAAAAFzcQB+AAIAAAACc3EAfgACAAAAAw==");

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
                Assert.That(typedContent.Data[0], Is.InstanceOf<UtfStringContent>());
                Assert.That(typedContent.Data[1], Is.InstanceOf<UtfStringContent>());
                Assert.That(typedContent.Data[2], Is.InstanceOf<UtfStringContent>());
            });

            var typedEntries = typedContent.Data.OfType<UtfStringContent>().ToImmutableArray();

            Assert.Multiple(() =>
            {
                Assert.That(typedEntries[0].String, Is.EqualTo("This"));
                Assert.That(typedEntries[1].String, Is.EqualTo("Is"));
                Assert.That(typedEntries[2].String, Is.EqualTo("A Test"));
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
        }
    }
}