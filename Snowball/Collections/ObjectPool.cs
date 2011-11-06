using System;
using System.Collections;
using System.Collections.Generic;

namespace Snowball.Collections
{
	/// <summary>
	/// Manages a collection of preallocated objects to avoid creating garbage.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObjectPool<T> : IObjectPool<T>, IEnumerable<T> where T : class
	{
		T[] objects;
		bool[] isActive;
		Queue<int> inactive;

		/// <summary>
		/// Gets the total number of objects in the pool, both active and inactive.
		/// </summary>
		public int Capactiy
		{
			get { return this.objects.Length; }
		}

		/// <summary>
		/// Gets the total number of in use objects in the pool.
		/// </summary>
		public int ActiveCount
		{
			get { return this.objects.Length - this.inactive.Count; }
		}

		/// <summary>
		/// Gets the total number of objects which are not in use in the pool.
		/// </summary>
		public int InactiveCount
		{
			get { return this.inactive.Count; }
		}

		/// <summary>
		/// The callback method used to create an object.
		/// </summary>
		public Func<T> CreateObject
		{
			get;
			private set;
		}

		/// <summary>
		/// The callback method used to reset an object.
		/// </summary>
		public Action<T> ResetObject
		{
			get;
			set;
		}

		/// <summary>
		/// Whether or not the pool should grow as needed.
		/// </summary>
		public bool AutoGrow
		{
			get;
			set;
		}

		/// <summary>
		/// The number of objects which will be added to the pool when an AutoGrow occurs.
		/// </summary>
		public int AutoGrowFactor
		{
			get;
			set;
		}

		/// <summary>
		/// All objects in the pool.
		/// </summary>
		public IEnumerable<T> AllObjects
		{
			get
			{
				for(int i = 0; i < this.objects.Length; i++)
					yield return this.objects[i];
			}
		}

		/// <summary>
		/// All active objects in the pool.
		/// </summary>
		public IEnumerable<T> ActiveObjects
		{
			get
			{
				for(int i = 0; i < this.objects.Length; i++)
				{
					if(this.isActive[i])
						yield return this.objects[i];
				}
			}
		}

		/// <summary>
		/// All inactive objects in the pool.
		/// </summary>
		public IEnumerable<T> InactiveObjects
		{
			get
			{
				for(int i = 0; i < this.objects.Length; i++)
				{
					if(!this.isActive[i])
						yield return this.objects[i];
				}
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="createObject">The callback method used to create an object.</param>
		/// <param name="capacity">The number of objects to allocate into the pool.</param>
		public ObjectPool(Func<T> createObject, int capacity)
		{
			if(createObject == null)
				throw new ArgumentNullException("create");

			if(capacity <= 0)
				throw new ArgumentOutOfRangeException("capacity", "Pool capacity must be > 0.");

			this.CreateObject = createObject;

			this.objects = new T[capacity];
			this.isActive = new bool[capacity];
			this.inactive = new Queue<int>(capacity);

			for(int i = 0; i < capacity; i++)
			{
				T obj = this.CreateObject();

				if(obj == null)
					throw new InvalidOperationException("CreateObject returned null.");

				this.objects[i] = obj;
				this.isActive[i] = false;
				this.inactive.Enqueue(i);
			}

			this.AutoGrow = true;
			this.AutoGrowFactor = capacity / 10;
		}

		/// <summary>
		/// Adds more objects into the pool.
		/// </summary>
		/// <param name="addedCapacity"></param>
		public void Grow(int addedCapacity)
		{
			int capactiy = this.objects.Length;

			Array.Resize<T>(ref this.objects, capactiy + addedCapacity);
			Array.Resize<bool>(ref this.isActive, capactiy + addedCapacity);

			for(int i = capactiy; i < capactiy + addedCapacity; i++)
			{
				var obj = this.CreateObject();

				if(obj == null)
					throw new InvalidOperationException("CreateObject returned null.");

				this.objects[i] = obj;
				this.isActive[i] = false;
				this.inactive.Enqueue(i);
			}
		}

		/// <summary>
		/// Returns the next inactive object from the pool.
		/// </summary>
		public T Next()
		{
			if(this.inactive.Count == 0)
			{
				if(!this.AutoGrow)
					return null;

				this.Grow(this.AutoGrowFactor);
			}

			int index = this.inactive.Dequeue();
			this.isActive[index] = true;

			if(this.ResetObject != null)
				this.ResetObject(this.objects[index]);

			return this.objects[index];
		}
				
		/// <summary>
		/// Returns the index of the object in the pool.
		/// </summary>
		/// <param name="obj"></param>
		public int IndexOf(T obj)
		{
			for(int i = 0; i < this.objects.Length; i++)
			{
				if(this.objects[i] == obj)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Marks an object from the pool as inactive.
		/// </summary>
		/// <param name="obj"></param>
		public void Return(T obj)
		{
			for(int i = 0; i < this.objects.Length; i++)
			{
				if(this.objects[i] == obj)
				{
					if(this.isActive[i])
					{
						this.isActive[i] = false;
						this.inactive.Enqueue(i);
					}

					return;
				}
			}
		}

		/// <summary>
		/// Marks the object at the given index as inactive.
		/// </summary>
		/// <param name="index"></param>
		public void ReturnAt(int index)
		{
			if(this.isActive[index])
			{
				this.isActive[index] = false;
				this.inactive.Enqueue(index);
			}
		}

		/// <summary>
		/// Checks all active objects in the pool against the predicate. If the predicate returns false, the object will be marked inactive.
		/// </summary>
		/// <param name="isValid">Predicate used to determine if an object is still active.</param>
		public void Validate(Predicate<T> isValid)
		{
			for(int i = 0; i < this.objects.Length; i++)
			{
				if(this.isActive[i] && !isValid(this.objects[i]))
				{
					this.isActive[i] = false;
					this.inactive.Enqueue(i);
				}
			}
		}

		/// <summary>
		/// Returns an enumerator for all active objects.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			for(int i = 0; i < this.objects.Length; i++)
			{
				if(this.isActive[i])
					yield return this.objects[i];
			}
		}

		/// <summary>
		/// Returns an enumerator for all active objects.
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
