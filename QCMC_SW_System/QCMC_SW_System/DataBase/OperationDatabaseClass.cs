using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace QCMC_SW_System.DataBase
{
    class OperationDatabaseClass
    {
        #region 连接数据库的接口类变量(Interface class variables connected to the database)

        #region Sqlserver接口类(Interface class of Sqlserver)

        public SqlConnection eSqlConnection = new SqlConnection("Server=.;user=sa;pwd=sa;database=QCMC_PU4_SW;Integrated Security=True");

        public SqlCommand eSqlCommand = new SqlCommand();

        #endregion

        public string eSqlString = null;

        public string InsertString = null;

        public string UpdateString = null;

        public string ProjectString = null;

        public string WhereString = null;

        public DataSet eDataSet = new DataSet();

        public DataView eDataView = new DataView();

        #endregion

        /// <summary>
        /// 返回操作信息字符串，TableName是表名，TableIDValue是用于判断存入ID是否存在的条件，多记录插入没值为""，InsertValue是插入字符串
        /// Return the operation information string, TableName is the table name, TableIDValue is the condition for judging whether 
        ///     the stored ID exists, no value is inserted for multi-record insertion, InsertValue is the insertion string
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="TableIDValue"></param>
        /// <param name="InsertValue"></param>
        /// <returns></returns>
        public string Insert(string TableName, string TableIDValue, string InsertValue)
        {
            string ResultString = null;
            eSqlConnection.Open();
            try
            {
                eSqlCommand.Connection = eSqlConnection;
                if (TableIDValue != "")
                {
                    eSqlCommand.CommandText = "select * from " + TableName + " where " + TableIDValue;
                    if (eSqlCommand.ExecuteScalar() == null)
                    {
                        eSqlCommand.CommandText = "insert into " + TableName + " select " + InsertValue;
                        eSqlCommand.ExecuteNonQuery();
                        ResultString = "新信息添加成功！";
                    }
                    else
                    {
                        ResultString = "ID信息已经存在！";
                    }
                }//单个插入记录
                else
                {
                    eSqlCommand.CommandText = "insert into " + TableName + " select " + InsertValue;
                    eSqlCommand.ExecuteNonQuery();
                    ResultString = "新信息添加成功！";
                }
            }
            catch (Exception ex)
            {
                ResultString = ex.Message.ToString() + "     数据添加操作失败!";
            }
            eSqlConnection.Close();
            return ResultString;
        }

        /// <summary>
        /// 返回更新信息,tablename表名,UpdateString更新语句，WhereString条件字符串,SingleUpdate表示是否为单次更新
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="tableID"></param>
        /// <param name="ptableID"></param>
        /// <param name="sqlstring"></param>
        public string Update(string TableName, string WhereString, string UpdateString, bool SingleUpdate)
        {
            string ResultString = null;
            eSqlConnection.Open();
            try
            {
                eSqlCommand.Connection = eSqlConnection;
                if (WhereString != "")
                {
                    eSqlCommand.CommandText = "update " + TableName + " set " + UpdateString + " where " + WhereString;
                }//单次更新
                else
                {
                    eSqlCommand.CommandText = "update " + TableName + " set " + UpdateString;
                }//批量更新
                eSqlCommand.ExecuteNonQuery();
                if (SingleUpdate)
                {
                    ResultString = "修改操作成功！";
                }
            }
            catch (Exception ex)
            {
                ResultString = ex.Message.ToString() + WhereString + "     数据库更新操作失败!";
            }
            eSqlConnection.Close();
            return ResultString;
        }

        /// <summary>
        /// 返回dataview,tablename表名,ProjectionString为投影字符串，WhereString为条件字符串，若为""表示查询全部
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="sqlstring"></param>
        /// <returns></returns>
        public DataView Query(string TableName, string ProjectionString, string WhereString)
        {
            eSqlConnection.Open();
            if (WhereString == "")
            {
                eSqlString = "select " + ProjectionString + " from " + TableName;
            }
            else
            {
                eSqlString = "select " + ProjectionString + " from " + TableName + " where " + WhereString;
            }
            SqlDataAdapter eSqlDataAdapter = new SqlDataAdapter(eSqlString, eSqlConnection);
            eDataSet.Tables.Clear();
            eDataSet.Clear();
            eSqlDataAdapter.Fill(eDataSet, TableName);
            eSqlConnection.Close();
            eDataSet.Tables[TableName].DefaultView.AllowNew = false;
            return eDataSet.Tables[TableName].DefaultView;
        }

        /// <summary>
        /// 返回删除信息字符串,TableName为删除表,DeleteString为删除条件字符串
        /// </summary>
        public string Delete(string TableName, string DeleteString)
        {
            string ResultString = null;
            eSqlConnection.Open();
            try
            {
                eSqlCommand.Connection = eSqlConnection;
                eSqlCommand.CommandText = "delete  from " + TableName + " where " + DeleteString;
                eSqlCommand.ExecuteNonQuery();
                ResultString = "删除操作成功！";
            }
            catch (Exception ex)
            {

                ResultString = ex.Message.ToString() + "     数据库删除操作失败!";
            }
            eSqlConnection.Close();
            return ResultString;
        }
    }
}
