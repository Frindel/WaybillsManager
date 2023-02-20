using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entitys;

namespace WaybillsManager.Model
{
	internal class WaybillsStorage : IList<Waybill>, IList
	{
		private Dictionary<int, IList> _pages;
		private int _count;

		public int PageSize { get; }

		public int Count
		{
			get
			{
				return _count;
			}
			protected set
			{
				_count = value;
			}
		}

		public bool IsReadOnly => true;

		public bool IsFixedSize => false;

		public bool IsSynchronized => false;

		public object SyncRoot => this;

		object? IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

		public Waybill this[int index]
		{
			get
			{
				int localIndex = index % PageSize;
				int pageIndex = index / PageSize;

				if (!_pages.ContainsKey(pageIndex))
				{
					_pages.Clear();
					LoadPage(pageIndex);
				}

				if (localIndex > PageSize / 2 && pageIndex < Count / PageSize)
				{
					LoadPage(pageIndex + 1);
					RemovePage(pageIndex - 1);
				}
				if (localIndex < PageSize / 2 && pageIndex > 0)
				{
					LoadPage(pageIndex - 1);
					RemovePage(pageIndex + 1);
				}


				return (Waybill)_pages[pageIndex][localIndex];
			}
			set { }
		}


		public WaybillsStorage(int pageSize = 50)
		{
			PageSize = pageSize;
			GetCount();

			_pages = new Dictionary<int, IList>();
		}

		#region Waybill actions

		public async Task AddWaybill (Waybill waybill)
		{

		}

		public async Task EditWaybill(Waybill waybill)
		{

		}

		public async Task RemoveWaybill(Waybill waybill)
		{

		}

		#endregion

		#region Storage methods

		private void GetCount()
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				Count = context.Waybills.Count();
			}
		}

		private void LoadPage(int pageIndex, bool useSincContext = true)
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				_pages[pageIndex] = context.Waybills.OrderByDescending(w => w.Date.Year).Skip(pageIndex * PageSize).Take(PageSize)
					.Include(w=>w.IdentityCard)
						.ThenInclude(ic=>ic.Driver)
					.Include(w=>w.CarStateNumber)
						.ThenInclude(csn=>csn.Car)
					.Include(w=>w.Route)
						.ThenInclude(r=>r.StartPoint)
					.Include(w => w.Route)
						.ThenInclude(r => r.EndPoint)
					.ToArray();
			};
		}

		private void RemovePage(int pageIndex)
		{
			if (_pages.ContainsKey(pageIndex))
				_pages.Remove(pageIndex);
		}

		public IEnumerator<Waybill> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region IList<Waybill>

		public int IndexOf(Waybill item)
		{
			throw new System.NotImplementedException();
		}

		public void Insert(int index, Waybill item)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public void Add(Waybill item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(Waybill item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(Waybill[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public bool Remove(Waybill item)
		{
			throw new System.NotImplementedException();
		}

		public int Add(object? value)
		{
			throw new NotImplementedException();
		}

		public bool Contains(object? value)
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object? value)
		{
			return -1;
		}

		public void Insert(int index, object? value)
		{
			throw new NotImplementedException();
		}

		public void Remove(object? value)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
