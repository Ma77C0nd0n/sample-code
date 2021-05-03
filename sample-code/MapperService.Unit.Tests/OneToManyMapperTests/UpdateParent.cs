using NUnit.Framework;
using System;
using System.Linq;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class UpdateParent
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
            Assert.Throws<ArgumentException>(() => _sut.UpdateParent(1, 947483648));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMinRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateParent(0, 1));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenParentValuesAreEqual()
        {
            Assert.Throws<ArgumentException>(() => _sut.UpdateParent(1, 1));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapIsNotPopulated()
        {
            Assert.Throws<ApplicationException>(() => _sut.UpdateParent(1, 2));
        }

        [Test]
        public void ShouldThrowApplicationException_WhenMapDoesNotContainOldParent()
        {
            _sut.Add(1, 2);
            Assert.Throws<ApplicationException>(() => _sut.UpdateParent(20, 3));
        }

        [Test]
        public void ShouldNotThrow_WhenRemovedSuccessfully()
        {
            _sut.Add(1, 2);
            Assert.DoesNotThrow(() => _sut.UpdateParent(1, 3));
            var res = _sut.GetParent(2);
            Assert.AreEqual(3, res);
        }
        
        [Test]
        public void ShouldNotThrow_AndShouldUpdateParent_WhenMultipleUpdatedSuccessfully()
        {
            _sut.Add(1, 2);
            _sut.Add(1, 3);
            Assert.DoesNotThrow(() => _sut.UpdateParent(1, 10));
            var res = _sut.GetChildren(10).ToList();
            Assert.AreEqual(2, res.Count());
            Assert.IsTrue(res.Contains(2));
            Assert.IsTrue(res.Contains(3));
        }
    }
}
