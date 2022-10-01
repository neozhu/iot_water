using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WebApp.Models;
using WebApp.Models.JsonModel;

namespace WebApp
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      this.CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
      this.CreateMap<Meter, WaterMeter>();
      this.CreateMap<MeterData, WaterMeterRecord>();



    }
  }
  public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
  {
    public DateTime Convert(string source, DateTime destination, ResolutionContext context)
    {
      return System.Convert.ToDateTime(source);
    }
  }

}