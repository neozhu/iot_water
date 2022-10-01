using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using SqlSugar;

namespace WebApp
{
  public static class SqlSugarFactory
  {
    public static ISqlSugarClient CreateSqlSugarClient()
    {
      var db = new SqlSugarClient(new ConnectionConfig()
      {
        ConnectionString =ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, //必填（连接字符串）
        DbType = DbType.SqlServer, //必填（那个数据库）
        IsAutoCloseConnection = false, //默认false（是否自动关闭连接）
        InitKeyType = InitKeyType.Attribute
      }); //默认SystemTable
      return db;
    }
  }
}