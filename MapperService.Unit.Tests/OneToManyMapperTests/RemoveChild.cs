using NUnit.Framework;
using System;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class RemoveChild
    {
        private OneToManyMapper _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new OneToManyMapper();
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMinRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.RemoveChild(0));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.RemoveChild(947483648));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapIsNotPopulated()
        {
            Assert.Throws<ApplicationException>(() => _sut.RemoveChild(1));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapDoesNotContainChild()
        {
            _sut.Add(1, 2);
            Assert.Throws<ApplicationException>(() => _sut.RemoveChild(20));
        }

        [Test]
        public void ShouldNotThrow_WhenRemovedSuccessfully()
        {
            _sut.Add(1, 2);
            Assert.DoesNotThrow(() => _sut.RemoveChild(2));
        }
    }
}
