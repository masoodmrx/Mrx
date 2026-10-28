namespace Mrx.AspNetCore.Services
{
    [Service]
    public class SettingServiceBase<TContext>
        where TContext : class, ISettingContext
    {
        public virtual string? Collection => null;
        private readonly TContext db;
        private bool loadSettings = false;
        private List<Setting>? _settings;

        public SettingServiceBase(TContext db)
        {
            this.db = db;
        }

        public List<Setting>? Settings
        {
            get
            {
                Init();
                return _settings;
            }
            private set => _settings = value;
        }
        public void Init()
        {
            if (!loadSettings)
            {
                loadSettings = true;
                Settings = db.Setting.Where(p => p.Collection == Collection).ToList();
            }
        }
        #region Get
        public string? Get([CallerMemberName] string? key = null) => Settings?.SingleOrDefault(p => p.Key == (Collection.IsNullOrEmpty() ? "" : Collection + ".") + key)?.Value;
        public bool? GetBool([CallerMemberName] string? key = null) => Get(key)?.ToBooleanNullable();
        public byte? GetByte([CallerMemberName] string? key = null) => Get(key)?.ToByteNullable();
        public short? GetShort([CallerMemberName] string? key = null) => Get(key)?.ToInt16Nullable();
        public int? GetInt([CallerMemberName] string? key = null) => Get(key)?.ToInt32Nullable();
        public long? GetLong([CallerMemberName] string? key = null) => Get(key)?.ToInt64Nullable();
        public float? GetFloat([CallerMemberName] string? key = null) => Get(key)?.ToSingleNullable();
        public double? GetDouble([CallerMemberName] string? key = null) => Get(key)?.ToDoubleNullable();
        public decimal? GetDecimal([CallerMemberName] string? key = null) => Get(key)?.ToDecimalNullable();
        public TEnum? GetEnum<TEnum>([CallerMemberName] string? key = null) where TEnum : struct => Get(key)?.ToEnum<TEnum>();
        public TimeSpan? GetTimeSpan([CallerMemberName] string? key = null) => Get(key)?.ToTimeSpanNullable();
        public T[]? GetArray<T>([CallerMemberName] string? key = null) => Get(key)?.DeserializeJson<T[]>();
        #endregion
        #region Set
        public void Set(string? key, object? value)
        {
            key = (Collection.IsNullOrEmpty() ? "" : Collection + ".") + key;
            var val = value?.ToString();
            var setting = Settings?.FirstOrDefault(p => p.Key == key);
            if (setting == null)
            {
                setting = new Setting { Key = key, Collection = Collection };
                Settings?.Add(setting);
                db.Setting.Add(setting);
            }
            setting.Value = val;
        }
        public void Set(object? value, [CallerMemberName] string? key = null) => Set(key, value);
        public void SetArray(object[] value, [CallerMemberName] string? key = null) => Set(key, value: value.SerializeJson());
        #endregion
        public bool Exists(string? key) => Settings?.Any(p => p.Key == key) ?? false;
        public void Save()
        {
            if (db is DbContext _db) _db.SaveChanges();
        }
        public async Task SaveAsync()
        {
            if (db is DbContext _db) await _db.SaveChangesAsync();
        }
    }
}
