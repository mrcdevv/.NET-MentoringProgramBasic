using RecentlyUsedList.Logic;

namespace RecentlyUsedList.Tests
{
    public class Tests
    {
        [Test]
        public void RecentlyUsedList_ShouldBeEmpty_WhenInitialized()
        {
            var list = new RUL();

            var count = list.Count;
            
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void Add_ShouldAddItemToList()
        {
            var list = new RUL();

            list.Add("item1");

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list.Get(0), Is.EqualTo("item1"));
        }

        [Test]
        public void Add_ShouldMoveExistingItemToTop_WhenDuplicateAdded()
        {
            var list = new RUL();
            list.Add("item1");
            list.Add("item2");

            list.Add("item1");

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list.Get(0), Is.EqualTo("item1"));
            Assert.That(list.Get(1), Is.EqualTo("item2"));
        }

        [Test]
        public void Get_ShouldReturnItemAtIndex()
        {
            var list = new RUL();
            list.Add("item1");
            list.Add("item2");

            var item = list.Get(1);

            Assert.That(item, Is.EqualTo("item1"));
        }

        [Test]
        public void Get_ShouldThrowException_WhenIndexOutOfBounds()
        {
            var list = new RUL();

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Get(0));
        }

        [Test]
        public void Add_ShouldNotAllowNullOrEmptyStrings()
        {
            var list = new RUL();

            Assert.Throws<ArgumentException>(() => list.Add(""));
            Assert.Throws<ArgumentException>(() => list.Add(null));
        }

        [Test]
        public void List_ShouldEnforceCapacity_WhenSpecified()
        {
            var list = new RUL(3);
            list.Add("item1");
            list.Add("item2");
            list.Add("item3");

            list.Add("item4");

            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list.Get(0), Is.EqualTo("item4"));
            Assert.That(list.Get(1), Is.EqualTo("item3"));
            Assert.That(list.Get(2), Is.EqualTo("item2"));
        }

        [Test]
        public void List_ShouldHaveDefaultCapacity_WhenNotSpecified()
        {
            var list = new RUL();

            for (int i = 0; i < 10; i++)
            {
                list.Add($"item{i}");
            }

            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list.Get(0), Is.EqualTo("item9"));
        }

        [Test]
        public void Add_ShouldAllowUnboundedCapacity_WhenSetToZero()
        {
            var list = new RUL(0);

            for (int i = 0; i < 10; i++)
            {
                list.Add($"item{i}");
            }

            Assert.That(list.Count, Is.EqualTo(10));
            Assert.That(list.Get(0), Is.EqualTo("item9"));
        }

        [Test]
        public void Get_ShouldThrowException_ForNegativeIndex()
        {
            var list = new RUL();

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Get(-1));
        }
    }
}