using NUnit.Framework;
using System;
using System.Linq;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class UpdateChild
    {
        private OneToManyMapper _sut;

        [SetUp]
        public void TestSetup()
        {
            _sut = new OneToManyMapper();
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateChild(1, 947483648));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMinRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateChild(0, 1));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParentValuesAreEqual()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateChild(1, 1));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapIsNotPopulated()
        {
            Assert.Throws<ApplicationException>(() => _sut.UpdateChild(1, 2));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapDoesNotContainOldParent()
        {
            _sut.Add(1, 2);
            Assert.Throws<ApplicationException>(() => _sut.UpdateChild(20, 3));
        }

        [Test]
        public void ShouldNotThrow_WhenRemovedSuccessfully()
        {
            _sut.Add(1, 2);
            Assert.DoesNotThrow(() => _sut.UpdateChild(2, 3));
            var res = _sut.GetParent(3);
            Assert.AreEqual(1, res);
        }
    }
}
