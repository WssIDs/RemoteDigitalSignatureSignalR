using Microsoft.Extensions.DependencyInjection;

namespace Common.Mvvm.DI
{
    public static class SimpleIoC
    {
        private static IServiceCollection _services;

        public static IServiceCollection Services
        {
            get
            {
                if (_services != null) return _services;
                _services = new ServiceCollection();
                return _services;
            }
            set => _services = value;
        }

        private static IServiceProvider _current;

        public static IServiceProvider Current
        {
            get
            {
                if (_current != null) return _current;
                _current = _services.BuildServiceProvider();
                return _current;
            }
            set => _current = value;
        }
    }
}
