using NUnit.Framework;
using System;
using System.Linq;

namespace MapperService.Unit.Tests.OneToManyMapperTests
{
    [TestFixture]
    public class GetParent
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
            Assert.Throws<ArgumentException>(() => _sut.GetParent(0));
        }

        [Test]
        public void ShouldThrowArgumentException_WhenInputIsOutOfMaxRange()
        {
            Assert.Throws<ArgumentException>(() => _sut.GetParent(947483648));
        }

        [Test]
        public void ShouldReturnZero_WhenMapIsNotPopulated()
        {
            var res = _sut.GetParent(20);
            Assert.AreEqual(0, res);
        }

        [Test]
        public void ShouldReturnZero_WhenMapDoesNotContainChild()
        {
            _sut.Add(1, 2);
            var res = _sut.GetParent(20);
            Assert.AreEqual(0, res);
        }

        [Test]
        public void ShouldReturnParent_WhenMapIsPopulated()
        {
            _sut.Add(1, 2);
            var res = _sut.GetParent(2);
            Assert.AreEqual(1, res);
        }
    }
}
