using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WaybillsManager.Model
{

	internal class HintsStorage
	{
		private static HintsStorage _hintsStorage;

		public Dictionary<Type, ObservableCollection<string>> Hints { get; }

		public Dictionary<Type, Func<IEnumerable<string>>> _getHintsFuncs;

		public void SetGetHintsFunc(Type typeForHints, Func<IEnumerable<string>> getHintsFunc)
		{
			// получение подсказок
			IEnumerable<string> hints = getHintsFunc.Invoke();

			// проверка существования ранее подсказок
			if (Hints.ContainsKey(typeForHints))
			{
				// сохранение новых подсказок в UI потоке
				Application.Current.Dispatcher.Invoke(()=>
				{
					_getHintsFuncs.Remove(typeForHints);

					Hints[typeForHints].AddRange(hints);
				});
			}
			else
			{
				// сохранение подсказок
				Hints.Add(typeForHints, new ObservableCollection<string>(hints));
			}

			// добавление функции получения подсказок
			_getHintsFuncs.Add(typeForHints, getHintsFunc);
		}

		public static HintsStorage Get()
		{
			if (_hintsStorage == null)
				_hintsStorage = new HintsStorage();
			return _hintsStorage;
		}

		private HintsStorage()
		{
			Hints = new Dictionary<Type, ObservableCollection<string>>();
			_getHintsFuncs = new Dictionary<Type, Func<IEnumerable<string>>>();

			//подпись на событие изменения путевок
			WaybillsStorage.Get().CreateNewElement += HintsUpdate;
		}

		private void HintsUpdate(object obj, NewElementArgs args)
		{
			Type typeForHints = args.ElementType;

			if (!Hints.ContainsKey(typeForHints))
				return;

			// получение новых подсказок
			IEnumerable<string> hints = _getHintsFuncs[typeForHints].Invoke();

			// сохранение новых подсказок в UI потоке
			Application.Current.Dispatcher.Invoke(() =>
			{
				Hints[typeForHints].Clear();

				Hints[typeForHints].AddRange(hints);
			});
		}
	}
}
