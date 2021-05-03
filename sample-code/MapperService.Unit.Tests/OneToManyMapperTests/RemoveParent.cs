using NUnit.Framework;
using System;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class RemoveParent
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
            Assert.Throws<ArgumentException>(() => _sut.RemoveParent(0));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.RemoveParent(947483648));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapIsNotPopulated()
        {
            Assert.Throws<ApplicationException>(() => _sut.RemoveParent(1));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapDoesNotContainChild()
        {
            _sut.Add(1, 2);
            Assert.Throws<ApplicationException>(() => _sut.RemoveParent(20));
        }

        [Test]
        public void ShouldNotThrow_WhenRemovedSuccessfully()
        {
            _sut.Add(1, 2);
            Assert.DoesNotThrow(() => _sut.RemoveParent(1));
        }
        
        [Test]
        public void ShouldNotThrow_WhenMultipleChildrenRemovedSuccessfully()
        {
            _sut.Add(1, 2);
            _sut.Add(1, 3);
            Assert.DoesNotThrow(() => _sut.RemoveParent(1));
        }
    }
}
