using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMBussiness.LIB
{
    public interface IBaseServices<T, TKey>
    {
        T Raw_Get(string Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_Get_Async(string Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        T Raw_Get(int Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_Get_Async(int Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        T Raw_Get(long Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_Get_Async(long Id, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        List<T> Raw_GetAll(List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<List<T>> Raw_GetAll_Async(List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);

        /// <summary>
        /// Get page
        /// </summary>
        /// <param name="Join">new Dictionary&lt;string, string&gt;() { { &quot;Columns&quot; , &quot;Column1, Column2..&quot; }, { &quot;Join&quot;, &quot;inner join Table on 1=1&quot;} };</param>
        /// <returns>T.</returns>
        List<T> Raw_GetPage(ref Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<List<T>> Raw_GetPage_Async(Int64 TotalRow, int page, int pageSize, string where = "", string order = "", Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> Join = null, IDbTransaction transaction = null, int? commandTimeout = null);
        List<T> Raw_QueryStringCustom(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<List<T>> Raw_QueryStringCustom_Async(string query, Dictionary<string, object> param = null, List<string> Columns = null, Dictionary<string, string> join = null, IDbTransaction transaction = null, int? commandTimeout = null);
        IEnumerable<T1> Raw_Query<T1>(string query, Dictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<IEnumerable<T1>> Raw_Query_Async<T1>(string query, Dictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_TruncateTable();
        bool Raw_DeleteStringCustom(string query, Dictionary<string, object> param = null, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_Delete(int Id, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> Raw_Delete_Async(int Id, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_Delete(string Ids, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> Raw_Delete_Async(string Id, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_SaveAll(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> Raw_SaveAll_Async(List<T> datas, List<string> Columns, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_InsertAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_InsertAllByKeys(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> Raw_InsertAll_Async(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        bool Raw_UpdateAll(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<bool> Raw_UpdateAll_Async(List<T> datas, List<string> Columns = null, bool keyIdentity = true, bool IgnoreOrSave = false, IDbTransaction transaction = null, int? commandTimeout = null);
        T Raw_SaveAuto(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_SaveAuto_Async(List<string> Columns, dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true /*1: AutoInsert/Update   |   2: Only Insert      |   3: OnlyUpdate*/, IDbTransaction transaction = null, int? commandTimeout = null);
        T Raw_Insert(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_Insert_Async(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        T Raw_Update(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
        Task<T> Raw_Update_Async(dynamic param, bool IgnoreOrSave = false, bool keyIdentity = true, List<string> Columns = null, IDbTransaction transaction = null, int? commandTimeout = null);
    }
}
