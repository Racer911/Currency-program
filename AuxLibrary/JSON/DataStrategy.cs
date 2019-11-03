using AuxLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    //public interface IDataStrategy
    //{
    //    Task<IEnumerable<TResult>> GetData<TSource, TResult>() where TSource : class
    //                                                           where TResult : class, new();
    //}
    public class DataStrategy<T> /*: IDataStrategy*/
    {
        readonly IJsonGetter<T> _getter;

        public string Json { get; private set; }

        public DataStrategy(T path)
        {
            _getter = CreateGetter(path);
        }

        private IJsonGetter<T> CreateGetter(T path)
        {
            var factory = new JsonGetterFactory();
            return factory.Getter(path);
        }

        public async Task<IEnumerable<TResult>> GetData<TSource, TResult>() where TSource : BankJSON
                                                                where TResult : CurrencyParams, new()
        {
            Json = await GetJsonAsync();

            if (string.IsNullOrEmpty(Json))
            {
                throw new InvalidOperationException("Json is empty");
            }

            // ToDo провести бы валидацию json перед сериализацией
            var bank = Deserializator.Deserialize<TSource>(Json);

            var result = bank?.Valute.Values
                .Select(cur => new TResult
                {
                    NumCode = cur.NumCode,
                    Name = cur.Name,
                    CharCode = cur.CharCode,
                    Nominal = cur.Nominal,
                    Value = cur.Value
                });
            return result;
        }

        private async Task<string> GetJsonAsync()
        {
            return await Task.Factory.StartNew(() => _getter?.GetJson()).ConfigureAwait(false);
        }

    }
}
