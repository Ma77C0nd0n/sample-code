using NUnit.Framework;
using System;

namespace MapperService.UnitTests.OneToManyMapperTests
{
    [TestFixture]
    public class Add
    {
        private OneToManyMapper _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new OneToManyMapper();
        }

        [Test]
        public void ShouldCreateNewMapping_WhenNoMappingsExistAndInputIsValid()
        {
            Assert.DoesNotThrow(() => _sut.Add(1,2));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.Add(1, 947483648));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMinRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.Add(0, 1));
        }
        
        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.Add(0, 1));
        }
    }
}