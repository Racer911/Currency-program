using AuxLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Currency
{
    public class MainWindowViewModel : DependencyObject, INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChanged([CallerMemberName]string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        readonly string _path = "https://www.cbr-xml-daily.ru/daily_json.js";
        IEnumerable<CurrencyParams> _data;
        const string _notSelected = "-не выбрано-";

        #region Props

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyOfPropertyChanged();
            }
        }

        string _curFrom;
        public string CurFrom
        {
            get => _curFrom;
            set
            {
                if (!value.IsDecimal())
                    return;

                _curFrom = value.Replace(" ", "").AddRemoveFirstZero();
                NotifyOfPropertyChanged();
                CurTo = string.IsNullOrEmpty(_curFrom) || SelectedItemConvertTo.ValueToRUB == 0 ? string.Empty 
                    : Convertation.ConvertCurrency(SelectedItemConvertFrom.ValueToRUB, SelectedItemConvertTo.ValueToRUB, decimal.Parse(_curFrom)).ToString();
            }
        }

        string _curTo;
        public string CurTo
        {
            get => _curTo;
            set
            {
                _curTo = value;
                NotifyOfPropertyChanged();
            }
        }

        //public ObservableCollection<ConvertionToRubAndUsd> ToRubAndUsd { get; private set; }

        //CurrencyNameCode _selectedCurrency;
        //public CurrencyNameCode SelectedCurrency
        //{
        //    get => _selectedCurrency;
        //    set
        //    {
        //        _selectedCurrency = value;
        //        NotifyOfPropertyChanged();
        //        ToRubAndUsd = new ObservableCollection<ConvertionToRubAndUsd>
        //        {
        //            new ConvertionToRubAndUsd
        //            {
        //                Currency = value.CharCode,
        //                ToRub = value.ValueToRUB,
        //                ToUsd = Convertation.ConvertCurrency(value.ValueToRUB, CurrencyCodes.SourceCollection.Cast<CurrencyNameCode>().Single(x => x.CharCode.Equals("USD")).ValueToRUB)
        //            }
        //        };
        //        NotifyOfPropertyChanged(nameof(ToRubAndUsd));
        //    }
        //}

        #endregion

        #region DP


        public CurrencyNameCodeValue SelectedCurrency
        {
            get { return (CurrencyNameCodeValue)GetValue(SelectedCurrencyProperty); }
            set { SetValue(SelectedCurrencyProperty, value); }
        }
        public static readonly DependencyProperty SelectedCurrencyProperty =
            DependencyProperty.Register(nameof(SelectedCurrency), typeof(CurrencyNameCodeValue), typeof(MainWindowViewModel), new PropertyMetadata(null, new PropertyChangedCallback(SelectedCurrencyChanged)));

        private static void SelectedCurrencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cl = d as MainWindowViewModel;

            if (cl == null || cl.SelectedCurrency == null)
                return;

            if (cl.CurrencyCodes.SourceCollection.Cast<CurrencyNameCodeValue>().FirstOrDefault(x => x.CharCode.Equals("USD")) == null)
            {
                MessageBox.Show("Источник не содержит данных о USD", "Котировки", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            cl.ToRubAndUsd = new ObservableCollection<ConvertionToRubAndUsd>
            {
                new ConvertionToRubAndUsd
                {
                    Currency = cl.SelectedCurrency.CharCode,
                    ToRub = cl.SelectedCurrency.ValueToRUB,
                    ToUsd = Convertation.ConvertCurrency(cl.SelectedCurrency.ValueToRUB, cl.CurrencyCodes.SourceCollection.Cast<CurrencyNameCodeValue>().Single(x => x.CharCode.Equals("USD")).ValueToRUB)
                }
            };
        }

        public ObservableCollection<ConvertionToRubAndUsd> ToRubAndUsd
        {
            get { return (ObservableCollection<ConvertionToRubAndUsd>)GetValue(ToRubAndUsdProperty); }
            set { SetValue(ToRubAndUsdProperty, value); }
        }
        public static readonly DependencyProperty ToRubAndUsdProperty =
            DependencyProperty.Register(nameof(ToRubAndUsd), typeof(ObservableCollection<ConvertionToRubAndUsd>), typeof(MainWindowViewModel), new PropertyMetadata(null));

        public ICollectionView CurrencyCodes
        {
            get { return (ICollectionView)GetValue(CurrencyCodesProperty); }
            set { SetValue(CurrencyCodesProperty, value); }
        }
        public static readonly DependencyProperty CurrencyCodesProperty =
            DependencyProperty.Register(nameof(CurrencyCodes), typeof(ICollectionView), typeof(MainWindowViewModel), new PropertyMetadata(null));

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(string), typeof(MainWindowViewModel), new PropertyMetadata(null, new PropertyChangedCallback(FilterChanged)));

        private static void FilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cl = d as MainWindowViewModel;
            if (cl != null && cl.CurrencyCodes != null)
            {
                cl.CurrencyCodes.Filter = null;
                cl.CurrencyCodes.Filter = cl.ApplyFilter;
            }
        }

        public ObservableCollection<CurrencyNameCodeValue> ConvertFrom
        {
            get { return (ObservableCollection<CurrencyNameCodeValue>)GetValue(ConvertFromProperty); }
            set { SetValue(ConvertFromProperty, value); }
        }
        public static readonly DependencyProperty ConvertFromProperty =
            DependencyProperty.Register(nameof(ConvertFrom), typeof(ObservableCollection<CurrencyNameCodeValue>), typeof(MainWindowViewModel), new PropertyMetadata(null));

        public ObservableCollection<CurrencyNameCodeValue> ConvertTo
        {
            get { return (ObservableCollection<CurrencyNameCodeValue>)GetValue(ConvertToProperty); }
            set { SetValue(ConvertToProperty, value); }
        }
        public static readonly DependencyProperty ConvertToProperty =
            DependencyProperty.Register(nameof(ConvertTo), typeof(ObservableCollection<CurrencyNameCodeValue>), typeof(MainWindowViewModel), new PropertyMetadata(null));

        public CurrencyNameCodeValue SelectedItemConvertFrom
        {
            get { return (CurrencyNameCodeValue)GetValue(SelectedItemConvertFromProperty); }
            set { SetValue(SelectedItemConvertFromProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemConvertFromProperty =
            DependencyProperty.Register(nameof(SelectedItemConvertFrom), typeof(CurrencyNameCodeValue), typeof(MainWindowViewModel), new PropertyMetadata(null, new PropertyChangedCallback(SelectedItemConvertFromChanged)));

        private static void SelectedItemConvertFromChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindowViewModel cl)
            {
                cl.CurTo = string.IsNullOrEmpty(cl.CurFrom) || cl.SelectedItemConvertTo.ValueToRUB == 0 ? string.Empty : Convertation.ConvertCurrency(cl.SelectedItemConvertFrom.ValueToRUB, cl.SelectedItemConvertTo.ValueToRUB, decimal.Parse(cl.CurFrom)).ToString();
            }
        }

        public CurrencyNameCodeValue SelectedItemConvertTo
        {
            get { return (CurrencyNameCodeValue)GetValue(SelectedItemConvertToProperty); }
            set { SetValue(SelectedItemConvertToProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemConvertToProperty =
            DependencyProperty.Register(nameof(SelectedItemConvertTo), typeof(CurrencyNameCodeValue), typeof(MainWindowViewModel), new PropertyMetadata(null, new PropertyChangedCallback(SelectedItemConvertToChanged)));

        private static void SelectedItemConvertToChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MainWindowViewModel cl)
            {
                cl.CurTo = string.IsNullOrEmpty(cl.CurFrom) || cl.SelectedItemConvertTo.ValueToRUB == 0 ? string.Empty : Convertation.ConvertCurrency(cl.SelectedItemConvertFrom.ValueToRUB, cl.SelectedItemConvertTo.ValueToRUB, decimal.Parse(cl.CurFrom)).ToString();
            }
        }


        #endregion

        #region Commands

        ICommand _updateCom;
        public ICommand UpdateCom => _updateCom ?? (_updateCom = new DelegateCommand(Update));

        ICommand _loadCom;
        public ICommand LoadCom => _loadCom ?? (_loadCom = new DelegateCommand(() => 
        {
            LoadData();
            Update();
        }));

        ICommand _swapCom;
        public ICommand SwapCom => _swapCom ?? (_swapCom = new DelegateCommand(() => 
        {
            var temp = SelectedItemConvertFrom;
            SelectedItemConvertFrom = SelectedItemConvertTo;
            SelectedItemConvertTo = temp;
        }));

        ICommand _disposeCom;
        public ICommand DisposeCom => _disposeCom ?? (_disposeCom = new DelegateCommand(Dispose));

        #endregion

        public MainWindowViewModel()
        {
            LoadData();
            InitComboBoxes(new ObservableCollection<CurrencyNameCodeValue>());
        }

        private async void LoadData()
        {
            IsBusy = true;
            _data = await Data.Instance.GetData(_path).ConfigureAwait(false);
            IsBusy = false;
        }

        private void Update()
        {
            var temp = _data?.OrderBy(x => x.CharCode).Select(x => new CurrencyNameCodeValue { Name = x.Name, CharCode = x.CharCode, ValueToRUB = x.Value / x.Nominal, Nominal = x.Nominal });

            InitComboBoxes(temp.ToObservableCollection());
            CurrencyCodes = null;
            CurrencyCodes = CollectionViewSource.GetDefaultView(temp);
            if(CurrencyCodes != null)
                CurrencyCodes.Filter = ApplyFilter;
            ToRubAndUsd = null;
        }

        private void InitComboBoxes(ObservableCollection<CurrencyNameCodeValue> list)
        {
            list.Insert(0, new CurrencyNameCodeValue { CharCode = _notSelected, Name = _notSelected, ValueToRUB = 0 });
            ConvertFrom = ConvertTo = list;
            SelectedItemConvertFrom = new CurrencyNameCodeValue { CharCode = _notSelected, Name = _notSelected, ValueToRUB = 0 };
            SelectedItemConvertTo = new CurrencyNameCodeValue { CharCode = _notSelected, Name = _notSelected, ValueToRUB = 0 };
        }

        private bool ApplyFilter(object obj)
        {
            bool res = true;

            var cur = obj as CurrencyNameCodeValue;

            if (!string.IsNullOrWhiteSpace(Filter) && cur != null && !cur.Name.ContainsIgnoreCase(Filter) && !cur.CharCode.ContainsIgnoreCase(Filter))
                res = false;

            return res;
        }

        public void Dispose()
        {
            if (CurrencyCodes != null)
            {
                CurrencyCodes.Filter = null;
                CurrencyCodes = null;
            }
        }
    }
}
