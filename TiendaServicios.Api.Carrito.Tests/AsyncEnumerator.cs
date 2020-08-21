using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Carrito.Tests
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> enumerator;
        public T Current => enumerator.Current;

        public AsyncEnumerator(IEnumerator<T> enumerato) 
            => this.enumerator = enumerato ?? throw new NotImplementedException();


        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(enumerator.MoveNext());
        }
    }
}
