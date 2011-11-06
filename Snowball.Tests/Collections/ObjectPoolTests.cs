using System;
using NUnit.Framework;
using Snowball.Collections;

namespace Snowball.Tests.Collections
{
	[TestFixture]
	public class ObjectPoolTests
	{
		class ObjectPoolTestObject
		{
			public static int ConstructorCalled = 0;

			public ObjectPoolTestObject()
			{
				ConstructorCalled++;
			}
		}

		[SetUp]
		public void SetUp()
		{
			ObjectPoolTestObject.ConstructorCalled = 0;
		}

		[Test]
		public void ConstructorAllocatesTheNumberOfObjectSpecifiedInCapacity()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);
			Assert.AreEqual(2, ObjectPoolTestObject.ConstructorCalled);
		}

		[Test]
		public void NoNewObjectsAllocatedWhenCapacityIsNotExceeded()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);
			
			var foo = pool.Next();
			var bar = pool.Next();
			
			Assert.AreEqual(2, ObjectPoolTestObject.ConstructorCalled);
		}

		[Test]
		public void NoNewObjectsAllocatedAndNullIsReturnedByNextWhenAutoGrowIsFalse()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);
			pool.AutoGrow = false;
			
			var foo = pool.Next();
			var bar = pool.Next();
			var baz = pool.Next();
			
			Assert.AreEqual(2, ObjectPoolTestObject.ConstructorCalled);
			Assert.IsNull(baz);
		}

		[Test]
		public void AutoGrowFactorIndicatesTheNumberOfNewObjectsAllocatedByAnAutoGrow()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);
			pool.AutoGrow = true;
			pool.AutoGrowFactor = 2;

			var foo = pool.Next();
			var bar = pool.Next();
			var baz = pool.Next();

			Assert.AreEqual(4, ObjectPoolTestObject.ConstructorCalled);
			Assert.AreEqual(4, pool.Capactiy);
		}

		[Test]
		public void ActiveCountIncrementsWithEachCallToNext()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);

			var foo = pool.Next();
			Assert.AreEqual(1, pool.ActiveCount);

			var bar = pool.Next();
			Assert.AreEqual(2, pool.ActiveCount);
		}

		[Test]
		public void ReturningObjectCausesActiveCountToDecrement()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);

			var foo = pool.Next();
			pool.Return(foo);

			Assert.AreEqual(0, pool.ActiveCount);
		}

		[Test]
		public void ReturningAnObjectTwiceCausesActiveCountToOnlyDecrementOnce()
		{
			ObjectPool<ObjectPoolTestObject> pool = new ObjectPool<ObjectPoolTestObject>(() => { return new ObjectPoolTestObject(); }, 2);

			var foo = pool.Next();

			var bar = pool.Next();
			pool.Return(bar);
			pool.Return(bar);

			Assert.AreEqual(1, pool.ActiveCount);
		}
	}
}
