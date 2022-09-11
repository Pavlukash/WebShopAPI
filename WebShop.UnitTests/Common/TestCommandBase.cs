using System;
using WebShop.Domain.Contexts;

namespace WebShop.UnitTests.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly WebShopApiContext Context;

        public TestCommandBase()
        {
            Context = DataCreator.Create();
        }

        public void Dispose()
        {
            DataCreator.Delete(Context);
        }
    }
}