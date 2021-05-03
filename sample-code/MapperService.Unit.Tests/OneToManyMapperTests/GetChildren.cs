using NUnit.Framework;
using System;
using System.Linq;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class GetChildren
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
            Assert.Throws<ArgumentException>(() => _sut.GetChildren(0));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetChildren(947483648));
        }

        [Test]
        public void ShouldReturnEmpty_WhenMapIsNotPopulated()
        {
            var res = _sut.GetChildren(1).ToList();
            Assert.AreEqual(0, res.Count());
        }

        [Test]
        public void ShouldReturnEmpty_WhenMapDoesNotContainParent()
        {
            _sut.Add(1, 2);
            var res = _sut.GetChildren(20).ToList();
            Assert.AreEqual(0, res.Count());
        }

        [Test]
        public void ShouldReturnChildren_WhenMapIsPopulated()
        {
            _sut.Add(1, 2);
            _sut.Add(1, 3);
            var res = _sut.GetChildren(1).ToList();
            Assert.AreEqual(2, res.Count());
            Assert.IsTrue(res.Contains(2));
            Assert.IsTrue(res.Contains(3));
        }
    }
}
