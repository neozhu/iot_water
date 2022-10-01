




using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using WebApp.Models;
using System.Data.Entity.SqlServer;

namespace WebApp.Repositories
{
  public class DataTableImportMappingQuery : QueryObject<DataTableImportMapping>
  {
    public DataTableImportMappingQuery Withfilter(IEnumerable<filterRule> filters)
    {

      if (filters != null)
      {
        foreach (var rule in filters)
        {


          if (rule.field == "Id")
          {
            int val = Convert.ToInt32(rule.value);
            And(x => x.Id == val);
          }


          if (rule.field == "EntitySetName")
          {
            if (rule.op == "equal")
            {
              And(x => x.EntitySetName == rule.value);
            }
            else
            {
              And(x => x.EntitySetName.Contains(rule.value));
            }
            
          }



          if (rule.field == "FieldName")
          {
            And(x => x.FieldName.Contains(rule.value));
          }



          if (rule.field == "TypeName")
          {
            And(x => x.TypeName.Contains(rule.value));
          }



          if (rule.field == "SourceFieldName")
          {
            And(x => x.SourceFieldName.Contains(rule.value));
          }






          if (rule.field == "RegularExpression")
          {
            And(x => x.RegularExpression.Contains(rule.value));
          }

          if (rule.field == "IsEnabled")
          {

            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsEnabled== boolval);
          }
          if (rule.field == "IsRequired")
          {

            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IsRequired == boolval);
          }
          if (rule.field == "IgnoredColumn")
          {

            var boolval = Convert.ToBoolean(rule.value);
            And(x => x.IgnoredColumn == boolval);
          }

        }
      }
      //And(x => x.IsEnabled == true);
      return this;
    }
  }
}



