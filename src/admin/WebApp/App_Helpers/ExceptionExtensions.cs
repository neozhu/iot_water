using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp
{
  public static class ExceptionExtensions
  {
    public static string GetMessage(this Exception exception) {
      var message = "";
      if (exception is System.Data.Entity.Validation.DbEntityValidationException)
      {
        var e = exception as System.Data.Entity.Validation.DbEntityValidationException;
        message = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
      }
      else if (exception is System.Data.Entity.Infrastructure.DbUpdateException)
      {
        var e = exception as System.Data.Entity.Infrastructure.DbUpdateException;
        if (e.InnerException != null
            && e.InnerException.InnerException != null)
        {
          if (e.InnerException.InnerException is System.Data.SqlClient.SqlException)
          {
            var sqlexception = e.InnerException.InnerException as System.Data.SqlClient.SqlException;
            switch (sqlexception.Number)
            {
              case 2627:  // Unique constraint error
                message = sqlexception.Message;
                break;
              case 547:   // Constraint check violation
                var str = sqlexception.Message.Replace("\"", "'");
                var reg = new Regex(@"The DELETE statement conflicted with the SAME TABLE REFERENCE constraint '(?<index>[\s\S]*?)'. The conflict occurred in database '(?<db>[\s\S]*?)', table '(?<table>[\s\S]*?)', column '(?<column>[\s\S]*?)'.
The statement has been terminated.");
                var mc = reg.Match(str);
                if (mc.Success)
                {
                  var table = mc.Groups["table"].Value;
                  var index = mc.Groups["index"].Value;
                  message = $"有关联的明细数据存在,不允许删除该记录.约束:{index}";
                }
                else
                {
                  message = $"有关联的明细数据存在,不允许删除该记录";
                }
                break;
              case 2601:  // Duplicated key row error
                var regex = @"\ACannot insert duplicate key row in object \'(?<tableName>.+?)\' with unique index \'(?<indexName>.+?)\'\. The duplicate key value is \((?<keyValues>.+?)\)";
                var match = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Compiled).Match(sqlexception.Message);
                if (match.Success)
                {
                  var tablename = match?.Groups["tableName"].Value;
                  var indexname = match?.Groups["indexName"].Value;
                  var keyvalue = match?.Groups["keyValues"].Value;
                  message = $"值:{keyvalue} 已经存在,不允许重复.约束:{indexname}";
                }
                else
                {
                  message = $"不允许重复插入相同的数据";
                }
                break;
              default:
                message = e.InnerException.InnerException.Message;
                break;
            }
          }
          else
          {
            message = e.GetBaseException().Message;
          }
        }
        else
        {
          message = e.GetBaseException().Message;
        }
      }
      else
      {
        message = exception.GetBaseException().Message;
      }
      return message;

      }

  }
}