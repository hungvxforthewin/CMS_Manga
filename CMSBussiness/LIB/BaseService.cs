using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CRMModel.Models.Data;


namespace CRMBussiness.LIB
{
    //HungVX
    public class BaseService<T, TKey> : IBaseServices<T, TKey> where T : class
    {
        public T Raw_Get(long Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Raw_Get(Id.ToString(), Columns, transaction, commandTimeout);
        }
        public virtual async Task<T> Raw_Get_Async(long Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Raw_Get_Async(Id.ToString(), Columns, transaction, commandTimeout);
        }
        public T Raw_Get(string Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.Get(cn, transaction, commandTimeout: commandTimeout, TableName, PrimaryKey, Id, Columns);
        }
        public virtual async Task<T> Raw_Get_Async(string Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return await SqlRaw<T>.Get_Async(cn, transaction, commandTimeout: commandTimeout, TableName, PrimaryKey, Id, Columns);
        }
        public T Raw_Get(int Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Raw_Get(Id.ToString(), Columns, transaction, commandTimeout);
        }
        public async Task<T> Raw_Get_Async(int Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Raw_Get_Async(Id.ToString(), Columns, transaction, commandTimeout);
        }
        public List<T> Raw_GetAll(List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.GetAll(cn, TableName, transaction, commandTimeout);
        }
        public async Task<List<T>> Raw_GetAll_Async(List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return await SqlRaw<T>.GetAll_Async(cn, TableName, transaction, commandTimeout);
        }
        public List<T> Raw_GetPage(ref Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.GetPage(cn, TableName, PrimaryKey, ref TotalRow, page, pageSize, where, transaction, commandTimeout, param, order, Columns, Join);
        }
        public async Task<List<T>> Raw_GetPage_Async(Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return await SqlRaw<T>.GetPage_Async(cn, TableName, PrimaryKey, TotalRow, page, pageSize, where, transaction, commandTimeout, param, order, Columns, Join);
        }
        public List<T> Raw_QueryStringCustom(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.QueryStringCustom(cn, TableName, query, transaction, commandTimeout, param, Columns, join);
        }
        public async Task<List<T>> Raw_QueryStringCustom_Async(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return await SqlRaw<T>.QueryStringCustom_Async(cn, TableName, query, transaction, commandTimeout, param, Columns, join);
        }
        public IEnumerable<T1> Raw_Query<T1>(string query, Dictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            param = param ?? new Dictionary<string, object>();
            var _param = new DynamicParameters();
            foreach (var e in param)
            {
                _param.Add(e.Key, e.Value);
            }
            return cn.Query<T1>(query, param: _param, transaction, commandTimeout: commandTimeout);
        }
        public async Task<IEnumerable<T1>> Raw_Query_Async<T1>(string query, Dictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            param = param ?? new Dictionary<string, object>();
            var _param = new DynamicParameters();
            foreach (var e in param)
            {
                _param.Add(e.Key, e.Value);
            }
            return await cn.QueryAsync<T1>(query, param: _param, transaction, commandTimeout: commandTimeout);
        }
        public bool Raw_TruncateTable()
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.TruncateTable(cn, TableName);
        }
        public bool Raw_DeleteStringCustom(string query, Dictionary<string, object> param, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.DeleteStringCustom(cn, TableName, query, transaction, commandTimeout, param);
        }
        public async Task<bool> Raw_DeleteStringCustom_Async(string query, Dictionary<string, object> param, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            return await SqlRaw<T>.DeleteStringCustom_Async(cn, TableName, query, transaction, commandTimeout, param);
        }
        public bool Raw_Delete(string Ids, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            return Raw_DeleteStringCustom(" where " + PrimaryKey + " in (" + Ids + ")", null);
        }
        public async Task<bool> Raw_Delete_Async(string Ids, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            return await Raw_DeleteStringCustom_Async(" where " + PrimaryKey + " in (" + Ids + ")", null);
        }
        public bool Raw_Delete(int Id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return Raw_Delete(Id.ToString());
        }
        public async Task<bool> Raw_Delete_Async(int Id, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return await Raw_Delete_Async(Id.ToString());
        }

        public bool Raw_SaveAll(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            if (!keyIdentity)
            {
                Columns.Add(PrimaryKey);
            }
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: Columns, IgnoreOrSave: IgnoreOrSave, Action: 1, keyIdentity: keyIdentity).Count > 0;
        }
        public async Task<bool> Raw_SaveAll_Async(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            if (!keyIdentity)
            {
                Columns.Add(PrimaryKey);
            }
            return (await SqlRaw<T>.SaveAll_Async(cn, TableName, PrimaryKey, datas, Columns: Columns, IgnoreOrSave: IgnoreOrSave, Action: 1, keyIdentity: keyIdentity)).Count > 0;
        }
        public bool Raw_InsertAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], CrudFieldType.Create);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 2, keyIdentity: keyIdentity).Count > 0;
        }
        public async Task<bool> Raw_InsertAll_Async(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], CrudFieldType.Create);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return (await SqlRaw<T>.SaveAll_Async(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 2, keyIdentity: keyIdentity)).Count > 0;
        }
        public bool Raw_UpdateAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.SaveAll(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 3, keyIdentity: keyIdentity).Count > 0;
        }
        public async Task<bool> Raw_UpdateAll_Async(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            datas = datas ?? new List<T>();
            if (datas.Count <= 0)
            {
                return false;
            }
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(datas[0], CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return (await SqlRaw<T>.SaveAll_Async(cn, TableName, PrimaryKey, datas, Columns: allColumns, IgnoreOrSave: false, Action: 3, keyIdentity: keyIdentity)).Count > 0;
        }

        public T Raw_SaveAuto(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, Columns, IgnoreOrSave, 1);
        }
        public async Task<T> Raw_SaveAuto_Async(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var cn = new SqlConnection(OpenDapper.connectionStr);
            var PrimaryKey = getPrimaryKeyColumn();
            return await SqlRaw<T>.Save_Async(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, Columns, IgnoreOrSave, 1);
        }
        public T Raw_Insert(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, CrudFieldType.Update);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, allColumns, IgnoreOrSave: false, Action: 2);
        }
        public async Task<T> Raw_Insert_Async(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, CrudFieldType.Update);
            if (!keyIdentity)
            {
                allColumns.Add(PrimaryKey);
            }
            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return await SqlRaw<T>.Save_Async(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, allColumns, IgnoreOrSave: false, Action: 2);
        }
        public T Raw_Update(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, allColumns, IgnoreOrSave: false, Action: 3);
        }
        public async Task<T> Raw_Update_Async(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var PrimaryKey = getPrimaryKeyColumn();
            Columns = Columns ?? new List<string>();

            List<string> allColumns = DataHelper.GetPropertyOrColumnsAccess(param, CrudFieldType.Update);

            allColumns = Raw_GetColumns(allColumns, Columns, IgnoreOrSave).ToList();

            var cn = new SqlConnection(OpenDapper.connectionStr);
            return await SqlRaw<T>.Save(cn, TableName, PrimaryKey, param, transaction, commandTimeout, keyIdentity, allColumns, IgnoreOrSave: false, Action: 3);
        }
        #region stored procedure
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="procedure"></param>
        /// <param name="oParams"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public IEnumerable<K> Procedure<K>(string procedure, object oParams = null, bool useCache = false)
        {
            try
            {
                var cn = new SqlConnection(OpenDapper.connectionStr);
                using (cn)
                {
                    var listParams = new DynamicParameters();
                    IEnumerable<K> items = null;
                    items = cn.Query<K>(procedure, oParams, commandType: CommandType.StoredProcedure);
                    return items;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlMapper.GridReader ProcedureQueryMulti(string procedure, object oParams = null, bool useCache = true)
        {
            try
            {
                var cn = new SqlConnection(OpenDapper.connectionStr);
                //if (oParams == null) oParams = new { schema = _schema };          
                using (cn)
                {
                    //SqlMapper.GridReader items = Cache.Get<SqlMapper.GridReader>(key);
                    var items = cn.QueryMultiple(procedure, oParams, commandType: CommandType.StoredProcedure);
                    //Cache.Set(key, items);
                    return items;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ProcedureExecute(string procedure, object oParams = null)
        {
            try
            {
                var cn = new SqlConnection(OpenDapper.connectionStr);
                using (cn)
                {
                    cn.Execute(procedure, oParams, commandType: CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        public bool ProcedureExecute(string procedure, int commandTimeout, object oParams = null)
        {
            try
            {
                var cn = new SqlConnection(OpenDapper.connectionStr);
                using (cn)
                {
                    cn.Execute(procedure, oParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        #endregion
        IEnumerable<string> Raw_GetColumns(List<string> all, List<string> column, bool ignore = false)
        {
            var get_ = all.Where(e =>
            {
                return column.Count == 0 || (ignore && !column.Contains(e)) || (!ignore && column.Contains(e));
            });
            return get_;
        }
        #region sql command
        /// <summary>
        /// The _table name
        /// </summary>
        private readonly string _tableName;
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService{T, TKey}" /> class.
        /// </summary>
        public BaseService()
        {
            _tableName = GetTableName();
        }
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">TableAttribute not found</exception>
        private string GetTableName()
        {
            //var attr = typeof(T).CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == "TableAttribute");
            //if (attr == null)
            //    throw new ArgumentNullException("TableAttribute not found");
            ////TODO: Get TableAttribute name
            //return (string)attr.ConstructorArguments[0].Value;
            var temp = typeof(T).CustomAttributes.Where(c => c.AttributeType.Name == "TableAttribute").FirstOrDefault();
            if (temp != null)
            {
                return temp.ConstructorArguments[0].Value.ToString();
            }
            return "";
        }
        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetInsertCommand()
        {
            return $"sp_{TableName}_Insert";
        }
        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetUpdateCommand()
        {
            return $"sp_{TableName}_Update";
        }
        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetDeleteCommand()
        {
            return $"sp_{TableName}_Delete";
        }
        /// <summary>
        /// Gets all command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetAllCommand()
        {
            return $"sp_{TableName}_GetAll";
        }
        /// <summary>
        /// Gets the by paging command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetByPagingCommand()
        {
            return $"sp_{TableName}_GetPaging";
        }
        /// <summary>
        /// Gets the by identifier command.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetByIdCommand()
        {
            return $"sp_{TableName}_GetById";
        }
        #endregion

        #region Parameters command
        /// <summary>
        /// Inserts the command.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DynamicParameters.</returns>
        private DynamicParameters InsertCommand(T entity)
        {
            var allParams = DataHelper.GetSQLParametersFromPublicProperties(entity, CrudFieldType.Create);
            allParams.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);
            return allParams;
        }
        /// <summary>
        /// Updates the parameters.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DynamicParameters.</returns>
        private DynamicParameters UpdateParameters(T entity)
        {
            var allParams = DataHelper.GetSQLParametersFromPublicProperties(entity, CrudFieldType.Update);
            return allParams;
        }

        private string getPrimaryKeyColumn()
        {
            return getPrimaryKeyColumns().FirstOrDefault();
        }
        private IEnumerable<string> getPrimaryKeyColumns()
        {
            return typeof(T).GetProperties().Where(e =>
            {
                return Attribute.IsDefined(e, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
            }).Select(e => e.Name);
        }



        #endregion
    }
}
