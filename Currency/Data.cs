using AuxLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Currency
{
    public class Data
    {
        #region Singleton

        private static volatile Data _instance;
        private static readonly object _lock = new object();
        public static Data Instance
        {
            get
            {
                if (_instance == null)
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new Data();
                    }
                return _instance;
            }
        }

        #endregion Singleton

        public async Task<IEnumerable<CurrencyParams>> GetData<T>(T path)
        {
            try
            {
                var data = new DataStrategy<T>(path);
                return await data.GetData<BankJSON, CurrencyParams>();
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message, "ArgumentException", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message, "InvalidOperationException", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (AggregateException e)
            {
                MessageBox.Show(e.InnerException.Message, "AggregateException", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            
        }

    }
}
