using Microsoft.EntityFrameworkCore;
using Moonlay.Core.Models;
using Moonlay.MCServiceWebApi.Db;
using System;

namespace Moonlay.MCServiceWebApi.UnitTests
{
    class SignInServiceMock : ISignInService
    {
        public string CurrentUser => "demo";

        public bool Demo => true;
    }

    internal class DbTestConnection : IDisposable
    {
        public DbTestConnection()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "moonlaydev")
                .Options;

            var optionsTrail = new DbContextOptionsBuilder<MyDbTrailContext>()
                .UseInMemoryDatabase(databaseName: "moonlaydev_trail")
                .Options;

            DbTrail = new MyDbTrailContext(optionsTrail);
            Db = new MyDbContext(options, DbTrail, new SignInServiceMock());
        }

        public MyDbContext Db { get; }
        public MyDbTrailContext DbTrail { get; }

        public void Dispose()
        {
            Db.Dispose();
            DbTrail.Dispose();
        }
    }
}
