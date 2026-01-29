using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;
using 水水水果API.Models.ConfigurationModel;
using 水水水果API.Services;

namespace WaterFruitApi.Tests
{
    public class RedisServiceTests
    {
        [Fact]
        public async Task AuthToken_Lifecycle_Works()
        {
            var service = CreateService(out var db, out var store, out _);

            await service.StoreAuthTokenAsync("user1", "token123", TimeSpan.FromMinutes(1));
            Assert.True(await service.ValidateAuthTokenAsync("user1", "token123"));

            await service.InvalidateAuthTokenAsync("user1");
            Assert.False(await service.ValidateAuthTokenAsync("user1", "token123"));
            Assert.False(store.ContainsKey("auth:user1"));
        }

        [Fact]
        public async Task Session_Lifecycle_Works()
        {
            var service = CreateService(out var db, out var store, out _);

            await service.StoreUserSessionAsync("sid", "data", TimeSpan.FromMinutes(5));
            Assert.Equal("data", await service.GetUserSessionAsync("sid"));
            await service.RemoveUserSessionAsync("sid");
            Assert.False(store.ContainsKey("session:sid"));
        }

        [Fact]
        public async Task Cache_ReturnsCachedValue_WhenPresent()
        {
            var service = CreateService(out var db, out var store, out _);
            var cached = new Sample { Value = "cached" };
            store["cache:key"] = JsonConvert.SerializeObject(cached);

            var result = await service.GetOrCreateCacheAsync("key", () => Task.FromResult<Sample>(null), TimeSpan.FromMinutes(1));

            Assert.Equal("cached", result.Value);
        }

        [Fact]
        public async Task Cache_BuildsAndStores_WhenMissing()
        {
            var service = CreateService(out var db, out var store, out _);
            var result = await service.GetOrCreateCacheAsync("newkey", () => Task.FromResult(new Sample { Value = "new" }), TimeSpan.FromMinutes(1));

            Assert.Equal("new", result.Value);
            Assert.True(store.ContainsKey("cache:newkey"));
        }

        [Fact]
        public async Task KeyExists_ChecksDatabase()
        {
            var service = CreateService(out var db, out var store, out _);
            store["cache:exists"] = "val";

            Assert.True(await service.KeyExistsAsync("cache:exists"));
            Assert.False(await service.KeyExistsAsync("cache:missing"));
        }

        [Fact]
        public async Task Lock_AcquireAndRelease()
        {
            var service = CreateService(out var db, out var store, out _);

            Assert.True(await service.AcquireLockAsync("lock1", TimeSpan.FromSeconds(1)));
            Assert.False(await service.AcquireLockAsync("lock1", TimeSpan.FromSeconds(1)));
            await service.ReleaseLockAsync("lock1");
            Assert.False(store.ContainsKey("lock:lock1"));
        }

        [Fact]
        public void LogoutList_AddAndRemove()
        {
            var service = CreateService(out var db, out _, out var lists);

            service.AddUserToLogoutList("user@test.com");
            Assert.True(service.IsUserLoggedOut("user@test.com"));

            service.RemoveUserFromLogoutList("user@test.com");
            Assert.False(service.IsUserLoggedOut("user@test.com"));
        }

        private static RedisService CreateService(out Mock<IDatabase> db, out Dictionary<string, RedisValue> store, out Dictionary<string, List<RedisValue>> lists)
        {
            store = new Dictionary<string, RedisValue>();
            lists = new Dictionary<string, List<RedisValue>>();
            db = CreateDatabaseMock(store, lists);

            var mux = new Mock<IConnectionMultiplexer>();
            mux.Setup(m => m.GetDatabase(-1, null)).Returns(db.Object);
            mux.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(db.Object);

            var service = new RedisService(
                new Mock<ILogger<RedisService>>().Object,
                Options.Create(new RedisSettingModel { LogoutDefault = "logout:list", ConnectionString = "" }),
                mux.Object);

            return service;
        }

        private static Mock<IDatabase> CreateDatabaseMock(Dictionary<string, RedisValue> store, Dictionary<string, List<RedisValue>> lists)
        {
            var db = new Mock<IDatabase>();
            var dbAsync = db.As<IDatabaseAsync>();

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.Always))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.NotExists))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.Always))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.NotExists))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            db.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), When.NotExists, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, TimeSpan? expiry, bool keepTtl, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (store.ContainsKey(k))
                    {
                        return Task.FromResult(false);
                    }
                    store[k] = value;
                    return Task.FromResult(true);
                });

            dbAsync.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    return Task.FromResult(store.ContainsKey(k) ? store[k] : RedisValue.Null);
                });

            db.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    return Task.FromResult(store.ContainsKey(k) ? store[k] : RedisValue.Null);
                });

            dbAsync.Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    var removed = store.Remove(k) || lists.Remove(k);
                    return Task.FromResult(removed);
                });

            db.Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    var removed = store.Remove(k) || lists.Remove(k);
                    return Task.FromResult(removed);
                });

            dbAsync.Setup(d => d.KeyExistsAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    return Task.FromResult(store.ContainsKey(k));
                });

            db.Setup(d => d.KeyExistsAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .Returns((RedisKey key, CommandFlags flags) =>
                {
                    var k = (string)key;
                    return Task.FromResult(store.ContainsKey(k));
                });

            db.Setup(d => d.ListRange(It.IsAny<RedisKey>(), 0, -1, CommandFlags.None))
                .Returns((RedisKey key, long start, long stop, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (!lists.TryGetValue(k, out var list))
                    {
                        return Array.Empty<RedisValue>();
                    }
                    return list.ToArray();
                });

            db.Setup(d => d.ListRemove(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), 0, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, long count, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (!lists.TryGetValue(k, out var list))
                    {
                        return 0;
                    }
                    var removed = list.RemoveAll(v => v == value);
                    return removed;
                });

            db.Setup(d => d.ListLeftPush(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), When.Always, CommandFlags.None))
                .Returns((RedisKey key, RedisValue value, When when, CommandFlags flags) =>
                {
                    var k = (string)key;
                    if (!lists.TryGetValue(k, out var list))
                    {
                        list = new List<RedisValue>();
                        lists[k] = list;
                    }
                    list.Insert(0, value);
                    return list.Count;
                });

            return db;
        }

        private class Sample
        {
            public string Value { get; set; }
        }
    }
}
