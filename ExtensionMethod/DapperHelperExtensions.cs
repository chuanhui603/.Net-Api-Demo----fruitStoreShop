using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace 水水水果API.ExtensionMethod
{
    public static class DapperHelperExtensions
    {
        private static string GetColumnName(PropertyInfo property)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
            return columnAttribute != null ? columnAttribute.Name : property.Name;
        }

        private static string GetSelectColumns<T>()
        {
            var properties = typeof(T).GetProperties();
            return string.Join(",", properties.Select(p => $"{GetColumnName(p)} AS {p.Name}"));
        }

        public static IEnumerable<T> GetAll<T>(this IDbConnection dbConnection, string tableName)
        {
            var columns = GetSelectColumns<T>();
            var sql = $"SELECT {columns} FROM {tableName}";
            return dbConnection.Query<T>(sql);
        }

        public static T GetById<T>(this IDbConnection dbConnection, string tableName, int id)
        {
            var columns = GetSelectColumns<T>();
            var sql = $"SELECT {columns} FROM {tableName} WHERE {GetColumnName(typeof(T).GetProperty("Id"))} = @Id";
            return dbConnection.QuerySingleOrDefault<T>(sql, new { Id = id });
        }

        public static T GetById<T>(this IDbConnection dbConnection, string tableName, Guid id)
        {
            var columns = GetSelectColumns<T>();
            var sql = $"SELECT {columns} FROM {tableName} WHERE {GetColumnName(typeof(T).GetProperty("Id"))} = @Id";
            return dbConnection.QuerySingleOrDefault<T>(sql, new { Id = id });
        }

        public static IEnumerable<T> GetByPage<T>(this IDbConnection dbConnection, string tableName, int page, int pageSize)
        {
            var columns = GetSelectColumns<T>();
            var sql = $"SELECT {columns} FROM {tableName} ORDER BY created_at LIMIT {pageSize} OFFSET {page * pageSize}";
            return dbConnection.Query<T>(sql);
        }



        public static void Insert<T>(this IDbConnection dbConnection, string tableName, T entity)
        {
            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(",", properties.Select(p => GetColumnName(p)));
            var paramNames = string.Join(",", properties.Select(p => "@" + p.Name));
            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
            dbConnection.Execute(sql, entity);
        }

        public static void Update<T>(this IDbConnection dbConnection, string tableName, T entity)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetValue(entity) != null); 
            var setClause = string.Join(",", properties.Select(p => $"{GetColumnName(p)} = @{p.Name}"));
            var sql = $"UPDATE {tableName} SET {setClause} WHERE {GetColumnName(typeof(T).GetProperty("Id"))} = @Id";
            dbConnection.Execute(sql, entity);
        }

        public static void Delete(this IDbConnection dbConnection, string tableName, int id)
        {
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            dbConnection.Execute(sql, new { Id = id });
        }

        public static void Delete(this IDbConnection dbConnection, string tableName, Guid id)
        {
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            dbConnection.Execute(sql, new { Id = id });
        }
    }
}
