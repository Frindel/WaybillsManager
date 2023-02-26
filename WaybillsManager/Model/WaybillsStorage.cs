using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WaybillsManager.Model.Data;
using WaybillsManager.Model.Data.Entitys;

namespace WaybillsManager.Model
{
	public class NewElementArgs : EventArgs
	{
		public Type HintsType { get; }
	}

	public delegate void NewElementDelegate(object obj, NewElementArgs args);

	public class WaybillsStorage : IList<Waybill>, IList, INotifyPropertyChanged, INotifyCollectionChanged
	{
		private static WaybillsStorage _storage;

		private int _count = -1;

		private Dictionary<int, IList> _pages;

		private readonly Waybill _emptyWaybill;

		// контекст, используемый при выполнении операций добавления/редактирования
		private ApplicationContext _context;

		// задачи преждевременной загрузки страцниц
		private List<int> _pageLoaded;

		private Dictionary<int, DateTime> _pageTouchTimes;
		#region Events

		public event PropertyChangedEventHandler? PropertyChanged;

		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		public event NewElementDelegate? NewWaybillElement;

		#endregion

		#region Indexer

		public Waybill this[int index]
		{
			get
			{
				int localIndex = index % PageSize;
				int pageIndex = index / PageSize;

				CleanUpPages();

				if (!_pages.ContainsKey(pageIndex))
				{
					LoadPage(pageIndex);
				}

				if (localIndex > PageSize / 2 && pageIndex < Count / PageSize)
				{
					LoadPageAsync(pageIndex + 1);
				}

				if (localIndex < PageSize / 2 && pageIndex > 0)
				{
					LoadPageAsync(pageIndex - 1);
				}

				return (Waybill)_pages[pageIndex][localIndex];
			}
		}


		object? IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

		Waybill IList<Waybill>.this[int index] { get => this[index]; set => throw new NotImplementedException(); }
		#endregion

		#region Propertys

		public int Count
		{
			get
			{
				if (_count == -1)
				{
					_count = GetCount();
				}
				return _count;
			}
			private set
			{
				_count = value;
				OnPropertyChanged(nameof(Count));
			}
		}

		public int PageSize { get; }

		public bool IsReadOnly { get; } = true;

		public bool IsSynchronized { get; } = false;

		public object SyncRoot { get; } = _storage;

		public bool IsFixedSize => false;

		#endregion

		#region Singleton

		private WaybillsStorage(int pageSize)
		{
			PageSize = pageSize;

			_pages = new Dictionary<int, IList>();

			_pageLoaded = new List<int>();

			_pageTouchTimes = new Dictionary<int, DateTime>();
		}

		public static WaybillsStorage Get(int pageSize = 200)
		{
			if (_storage == null)
				_storage = new WaybillsStorage(pageSize);
			return _storage;
		}

		#endregion

		#region Waybills methods

		public async Task AddWaybillAsync(Waybill waybill)
		{
			await Task.Run(() =>
			{

				lock (_pages)
				{

					using (_context = new ApplicationContext())
					{
						try
						{
							_context.Database.BeginTransaction();

							waybill.Car = GetOrCreateCar(waybill.Car);

							waybill.CarStateNumber = GetOrCreateCarStateNumber(waybill.CarStateNumber);

							waybill.IdentityCard = GetOrCreateIdntityCard(waybill.IdentityCard);

							waybill.Route = GetOrCreateRoute(waybill.Route);

							_context.Waybills.Add(waybill);

							// добавление записи
							_context.SaveChanges();

							_context.Database.CommitTransaction();
						}
						catch (Exception e)
						{
							_context.Database.RollbackTransaction();
							MessageBox.Show("При сохранении путевки возникла ошибка.");
						}
					}

				}
			});

			// уведомление представления об изменениях
			int localIndex = Count % PageSize;
			int pageIndex = Count / PageSize;

			LoadPage(pageIndex);

			_pages[pageIndex].RemoveAt(localIndex);
			_pages[pageIndex].Add(waybill);

			Count++;
			OnPropertyChanged("Item[]");

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, waybill, Count - 1));
		}

		public async Task RemoveWaybillAsync(Waybill waybill)
		{
			int index = IndexOf(waybill);

			await Task.Run(() =>
			{
				using (ApplicationContext context = new ApplicationContext())
				{
					context.Waybills.Remove(waybill);
					context.SaveChanges();
				}
			});

			LoadPage(index / PageSize);

			Count--;
			OnPropertyChanged("Item[]");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, waybill, index));
		}
		#endregion

		#region IList methods

		public int Add(object? value)
		{
			Add((Waybill)value);

			return Count;
		}

		public void Add(Waybill item)
		{
			int index = Count;

			this.Insert(index, item);

		}

		public void Clear()
		{
			_pages.Clear();

			//todo: уведомление об изменении
		}

		public bool Contains(object? value)
		{
			return Contains((Waybill)value);
		}

		// выполняет проверку существования путевки с данным номером в данном периоде
		public bool Contains(Waybill item)
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				return context.Waybills
					.Where(w => w.Number == item.Number && w.Date.Year == item.Date.Year)
					.FirstOrDefault() != null;
			}
		}

		public void CopyTo(Array array, int index)
		{
			CopyTo(array, index);
		}

		public void CopyTo(Waybill[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<Waybill> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		public void Remove(object? value)
		{
			Remove((Waybill)value);
		}

		public bool Remove(Waybill item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public int IndexOf(object? value)
		{
			return IndexOf((Waybill)value);
		}

		public int IndexOf(Waybill item)
		{
			// todo: реализовать получение индекса элемента

			using (ApplicationContext context = new ApplicationContext())
			{
				return context.Waybills
					.OrderBy(w => w.Date)
					.ThenBy(w => w.Number)
					.AsEnumerable()
					.Select((w, i) => new { Waybill = w, Index = i })
					.Where(w => w.Waybill.Id == item.Id).Select(w => w.Index).SingleOrDefault();
			}
		}

		public void Insert(int index, object? value)
		{
			Insert(index, (Waybill)value);
		}

		public void Insert(int index, Waybill item)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Protected methods

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			CollectionChanged?.Invoke(this, args);
		}

		private int GetCount()
		{
			using (ApplicationContext context = new ApplicationContext())
			{
				return context.Waybills.Count();
			}
		}

		private async Task LoadPageAsync(int pageIndex)
		{
			if (_pages.ContainsKey(pageIndex) || _pageLoaded.Contains(pageIndex))
				return;

			_pageLoaded.Add(pageIndex);

			IList waybills = await Task.Run(() => GetPage(pageIndex));

			_pages[pageIndex] = waybills;

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

			_pageLoaded.Remove(pageIndex);
		}

		private void LoadPage(int pageIndex)
		{
			_pages[pageIndex] = GetPage(pageIndex);
		}

		private IList GetPage(int pageIndex)
		{
			_pageTouchTimes[pageIndex] = DateTime.Now;

			using (ApplicationContext context = new ApplicationContext())
			{

				return context.Waybills.OrderBy(w => w.Date).ThenBy(w => w.Number).Skip(pageIndex * PageSize).Take(PageSize)
					.Include(w => w.IdentityCard)
						.ThenInclude(ic => ic.Driver)
					.Include(w => w.CarStateNumber)
					.Include(w => w.Car)
					.Include(w => w.Route)
						.ThenInclude(r => r.StartPoint)
					.Include(w => w.Route)
						.ThenInclude(r => r.EndPoint)
					.ToList();
			};
		}

		private void CleanUpPages()
		{
			foreach (int pageNumber in _pageTouchTimes.Keys)
			{
				if ((DateTime.Now - _pageTouchTimes[pageNumber]).TotalSeconds > 5)
				{
					_pages.Remove(pageNumber);
					_pageTouchTimes.Remove(pageNumber);
				}
			}
		}

		// создает или возвращает из БД машину с переданными характеристиками 
		private Car GetOrCreateCar(Car car)
		{
			Car? returnedСar = _context.Cars.Where(c => c.Name == car.Name).FirstOrDefault();

			if (returnedСar == null)
				returnedСar = new Car() { Name = car.Name };

			// todo: вызов события добавления подсказки

			return returnedСar;
		}

		// создает или возвращает из БД гос. номер машины с переданными характеристиками 
		private CarStateNumber GetOrCreateCarStateNumber(CarStateNumber carStateNumber)
		{
			var a = _context.CarStateNumbers.ToList();

			var b = _context.CarStateNumbers.Where(csn => csn.Number == carStateNumber.Number).ToList();

			var c = a[1].Number.CompareTo(carStateNumber.Number);

			CarStateNumber? returnedCarStateNumber = _context.CarStateNumbers
				.Where(csn => csn.Number.ToLower().CompareTo(carStateNumber.Number) == 0).FirstOrDefault();

			if (returnedCarStateNumber == null)
				returnedCarStateNumber = new CarStateNumber() { Number = carStateNumber.Number };

			return returnedCarStateNumber;
		}

		// создает или возвращает из БД гос. удостоверение и информацию о водителе с переданными характеристиками 
		private IdentityCard GetOrCreateIdntityCard(IdentityCard identityCard)
		{
			IdentityCard? returnedIdentityCard = _context.IdentityCards
				.Include(ic => ic.Driver)
				.Where(ic => ic.Number == identityCard.Number)
				.FirstOrDefault();

			if (returnedIdentityCard == null)
			{
				returnedIdentityCard = new IdentityCard() { Number = identityCard.Number };

				Driver? returnedDriver = _context.Drivers
					.Where(d => d.Name == identityCard.Driver.Name)
					.FirstOrDefault();

				if (returnedDriver == null)
				{
					returnedDriver = new Driver() { Name = identityCard.Driver.Name };
				}

				returnedIdentityCard.Driver = returnedDriver;
			}

			return returnedIdentityCard;
		}

		// создает или возвращает из БД маршрут с переданными характеристиками 
		private Route GetOrCreateRoute(Route route)
		{
			Route? returnedRoute = _context.Routes
				.Include(r => r.StartPoint)
				.Include(r => r.EndPoint)
				.Where(r => r.StartPoint.Name == route.StartPoint.Name &&
					route.EndPoint != null && r.EndPoint.Name == route.EndPoint.Name)
				.FirstOrDefault();

			if (returnedRoute == null)
			{
				returnedRoute = new Route();

				RoutePoint startPoint = _context.RoutePoints.Where(rp => rp.Name == route.StartPoint.Name).FirstOrDefault();

				if (startPoint == null)
				{
					startPoint = new RoutePoint() { Name = route.StartPoint.Name };
				}

				returnedRoute.StartPoint = startPoint;

				if (route.EndPoint == null || route.EndPoint.Name == string.Empty)
				{
					returnedRoute.EndPoint = null;
					return returnedRoute;
				}

				RoutePoint endPoint = _context.RoutePoints.Where(rp => rp.Name == route.EndPoint.Name)
					.FirstOrDefault();

				if (endPoint == null)
				{
					endPoint = new RoutePoint() { Name = route.EndPoint.Name };
				}

				returnedRoute.EndPoint = endPoint;
			}

			return returnedRoute;
		}
		#endregion
	}
}
