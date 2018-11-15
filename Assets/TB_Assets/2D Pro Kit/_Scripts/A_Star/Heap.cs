using System.Collections;
using UnityEngine;
using System;

namespace TrollBridge{

	/// <summary>
	/// Heap script that works with our A Star Pathfinding
	/// </summary>
	public class Heap<T> where T : IHeapItem<T> {

		T[] items;
		int currentItemCount;

		/// <summary>
		/// Initializes a new instance of the Heap class.
		/// </summary>
		public Heap(int maxHeapSize) {
			items = new T[maxHeapSize];
		}

		/// <summary>
		/// Add the specified item.
		/// </summary>
		public void Add(T item) {
			item.HeapIndex = currentItemCount;
			items [currentItemCount] = item;
			SortUp (item);
			currentItemCount++;
		}

		/// <summary>
		/// Removes the first from our heap.
		/// </summary>
		public T RemoveFirst() {
			T firstItem = items [0];
			currentItemCount--;
			items [0] = items [currentItemCount];
			items [0].HeapIndex = 0;
			SortDown (items[0]);
			return firstItem;
		}

		/// <summary>
		/// Updates the item.
		/// </summary>
		public void UpdateItem(T item) {
			SortUp (item);
		}

		/// <summary>
		/// Gets the countItemCount.
		/// </summary>
		public int Count {
			get {
				return currentItemCount;
			}
		}

		/// <summary>
		/// Does this contain the specified item?
		/// </summary>
		public bool Contains(T item) {
			return Equals (items [item.HeapIndex], item);
		}

		/// <summary>
		/// Sorts down.
		/// </summary>
		private void SortDown(T item) {
			// WHILE always true.
			while(true){
				// Get our left and right child indexes
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				// IF our childIndexLeft is less than our currentItemCount.
				if (childIndexLeft < currentItemCount) {
					// Set our swapIndex to our childIndexLeft.
					swapIndex = childIndexLeft;
					// IF our childIndexRight is less than our currentItemCount.
					if (childIndexRight < currentItemCount) {
						// IF the items array where the childIndexLeft index is before the childIndexRight.
						if (items [childIndexLeft].CompareTo (items [childIndexRight]) < 0) {
							// Set our swapIndex to our childIndexRight.
							swapIndex = childIndexRight;
						}
					}
					// IF our item comes before items [swapIndex] in the array.
					if (item.CompareTo (items [swapIndex]) < 0) {
						// Swap.
						Swap (item, items [swapIndex]);
					} else {
						// Leave.
						return;
					}
				} else {
					// Leave.
					return;
				}
			}
		}

		/// <summary>
		/// Sorts up.
		/// </summary>
		private void SortUp(T item) {
			// Get the parent heap index of our item.
			int parentIndex = (item.HeapIndex - 1) / 2;
			// WHILE always true.
			while (true) {
				// Get our parentItem that from the items array with parentIndex being the indexer.
				T parentItem = items [parentIndex];
				// IF item is after parentItem in the list.
				if (item.CompareTo (parentItem) > 0) {
					// Swap.
					Swap (item, parentItem);
				} else {
					// Break out of the loop.
					break;
				}
				// Set our parentIndex based on our item.
				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		/// <summary>
		/// Swap the specified itemA and itemB.
		/// </summary>
		private void Swap(T itemA, T itemB) {
			items [itemA.HeapIndex] = itemB;
			items [itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}

	}


	public interface IHeapItem<T> : IComparable<T> {
		int HeapIndex {
			get;
			set;
		}
	}
}
