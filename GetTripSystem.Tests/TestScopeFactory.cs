using GetTripSystem.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using static GetTripSystem.DAL;

namespace GetTripSystem.Tests
{
    internal class TestScopeFactory : IServiceScopeFactory
    {
        private readonly Context _context;

        public TestScopeFactory(Context context)
        {
            _context = context;
        }

        public IServiceScope CreateScope()
        {
            return new TestScope(_context);
        }
    }

    internal class TestScope : IServiceScope
    {
        private readonly Context _context;

        public TestScope(Context context)
        {
            _context = context;
        }

        public IServiceProvider ServiceProvider => new TestServiceProvider(_context);

        public void Dispose() { }
    }

    internal class TestServiceProvider : IServiceProvider
    {
        private readonly Context _context;

        public TestServiceProvider(Context context)
        {
            _context = context;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(TripRepository))
                return new TripRepository(_context);
            if (serviceType == typeof(PictureRepository))
                return new PictureRepository(_context);
            if (serviceType == typeof(RegistrationRepository))
                return new RegistrationRepository(_context);
            if (serviceType == typeof(UserRepository))
                return new UserRepository(_context);

            return null;
        }
    }
}

