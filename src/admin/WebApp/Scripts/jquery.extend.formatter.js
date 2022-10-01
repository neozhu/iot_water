//-------账号类型---------//
var accounttypefiltersource = [{ value: '', text: 'All'}];
var accounttypedatasource = [];
accounttypefiltersource.push({ value: '0',text:'公司'  });
accounttypedatasource.push({ value: '0',text:'公司'  });
accounttypefiltersource.push({ value: '1',text:'供应商'  });
accounttypedatasource.push({ value: '1',text:'供应商'  });
accounttypefiltersource.push({ value: '2',text:'客户'  });
accounttypedatasource.push({ value: '2',text:'客户'  });
accounttypefiltersource.push({ value: '3',text:'外协单位'  });
accounttypedatasource.push({ value: '3',text:'外协单位'  });
//for datagrid AccountType field  formatter
function accounttypeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = accounttypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = accounttypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   AccountType  field filter 
$.extend($.fn.datagrid.defaults.filters, {
accounttypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: accounttypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   AccountType   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
accounttypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: accounttypedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------告警等级---------//
var alarmlevelfiltersource = [{ value: '', text: 'All'}];
var alarmleveldatasource = [];
alarmlevelfiltersource.push({ value: '一级',text:'一级'  });
alarmleveldatasource.push({ value: '一级',text:'一级'  });
alarmlevelfiltersource.push({ value: '三级',text:'三级'  });
alarmleveldatasource.push({ value: '三级',text:'三级'  });
alarmlevelfiltersource.push({ value: '二级',text:'二级'  });
alarmleveldatasource.push({ value: '二级',text:'二级'  });
//for datagrid alarmlevel field  formatter
function alarmlevelformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = alarmleveldatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = alarmleveldatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   alarmlevel  field filter 
$.extend($.fn.datagrid.defaults.filters, {
alarmlevelfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: alarmlevelfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   alarmlevel   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
alarmleveleditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: alarmleveldatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------告警状态---------//
var alarmstatusfiltersource = [{ value: '', text: 'All'}];
var alarmstatusdatasource = [];
alarmstatusfiltersource.push({ value: '已处理',text:'已处理'  });
alarmstatusdatasource.push({ value: '已处理',text:'已处理'  });
alarmstatusfiltersource.push({ value: '待处理',text:'待处理'  });
alarmstatusdatasource.push({ value: '待处理',text:'待处理'  });
//for datagrid alarmstatus field  formatter
function alarmstatusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = alarmstatusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = alarmstatusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   alarmstatus  field filter 
$.extend($.fn.datagrid.defaults.filters, {
alarmstatusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: alarmstatusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   alarmstatus   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
alarmstatuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: alarmstatusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------告警类型---------//
var alarmtypefiltersource = [{ value: '', text: 'All'}];
var alarmtypedatasource = [];
alarmtypefiltersource.push({ value: '宕机',text:'宕机'  });
alarmtypedatasource.push({ value: '宕机',text:'宕机'  });
alarmtypefiltersource.push({ value: '故障',text:'故障'  });
alarmtypedatasource.push({ value: '故障',text:'故障'  });
alarmtypefiltersource.push({ value: '阈值',text:'阈值'  });
alarmtypedatasource.push({ value: '阈值',text:'阈值'  });
alarmtypefiltersource.push({ value: '预测',text:'预测'  });
alarmtypedatasource.push({ value: '预测',text:'预测'  });
//for datagrid alarmtype field  formatter
function alarmtypeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = alarmtypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = alarmtypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   alarmtype  field filter 
$.extend($.fn.datagrid.defaults.filters, {
alarmtypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: alarmtypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   alarmtype   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
alarmtypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: alarmtypedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------台账状态---------//
var billstatusfiltersource = [{ value: '', text: 'All'}];
var billstatusdatasource = [];
billstatusfiltersource.push({ value: '已确认',text:'已确认'  });
billstatusdatasource.push({ value: '已确认',text:'已确认'  });
billstatusfiltersource.push({ value: '待确认',text:'待确认'  });
billstatusdatasource.push({ value: '待确认',text:'待确认'  });
//for datagrid billstatus field  formatter
function billstatusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = billstatusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = billstatusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   billstatus  field filter 
$.extend($.fn.datagrid.defaults.filters, {
billstatusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: billstatusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   billstatus   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
billstatuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: billstatusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------账单状态---------//
var bstatusfiltersource = [{ value: '', text: 'All'}];
var bstatusdatasource = [];
bstatusfiltersource.push({ value: '已开票',text:'已开票'  });
bstatusdatasource.push({ value: '已开票',text:'已开票'  });
bstatusfiltersource.push({ value: '已收款',text:'已收款'  });
bstatusdatasource.push({ value: '已收款',text:'已收款'  });
bstatusfiltersource.push({ value: '已确认',text:'已确认'  });
bstatusdatasource.push({ value: '已确认',text:'已确认'  });
bstatusfiltersource.push({ value: '待确认',text:'待确认'  });
bstatusdatasource.push({ value: '待确认',text:'待确认'  });
//for datagrid bstatus field  formatter
function bstatusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = bstatusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = bstatusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   bstatus  field filter 
$.extend($.fn.datagrid.defaults.filters, {
bstatusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: bstatusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   bstatus   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
bstatuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: bstatusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------客户类型---------//
var categoryfiltersource = [{ value: '', text: 'All'}];
var categorydatasource = [];
categoryfiltersource.push({ value: '建筑单位客户',text:'建筑单位客户'  });
categorydatasource.push({ value: '建筑单位客户',text:'建筑单位客户'  });
categoryfiltersource.push({ value: '收费客户',text:'收费客户'  });
categorydatasource.push({ value: '收费客户',text:'收费客户'  });
categoryfiltersource.push({ value: '部门客户',text:'部门客户'  });
categorydatasource.push({ value: '部门客户',text:'部门客户'  });
//for datagrid category field  formatter
function categoryformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = categorydatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = categorydatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   category  field filter 
$.extend($.fn.datagrid.defaults.filters, {
categoryfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: categoryfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   category   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
categoryeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: categorydatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------单位级别---------//
var customerlevelfiltersource = [{ value: '', text: 'All'}];
var customerleveldatasource = [];
customerlevelfiltersource.push({ value: '一级单位',text:'一级单位'  });
customerleveldatasource.push({ value: '一级单位',text:'一级单位'  });
customerlevelfiltersource.push({ value: '二级单位',text:'二级单位'  });
customerleveldatasource.push({ value: '二级单位',text:'二级单位'  });
//for datagrid customerlevel field  formatter
function customerlevelformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = customerleveldatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = customerleveldatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   customerlevel  field filter 
$.extend($.fn.datagrid.defaults.filters, {
customerlevelfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: customerlevelfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   customerlevel   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
customerleveleditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: customerleveldatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------单位业态---------//
var customertypefiltersource = [{ value: '', text: 'All'}];
var customertypedatasource = [];
customertypefiltersource.push({ value: '内供',text:'内供'  });
customertypedatasource.push({ value: '内供',text:'内供'  });
customertypefiltersource.push({ value: '外供',text:'外供'  });
customertypedatasource.push({ value: '外供',text:'外供'  });
customertypefiltersource.push({ value: '核算',text:'核算'  });
customertypedatasource.push({ value: '核算',text:'核算'  });
customertypefiltersource.push({ value: '自用',text:'自用'  });
customertypedatasource.push({ value: '自用',text:'自用'  });
//for datagrid customertype field  formatter
function customertypeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = customertypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = customertypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   customertype  field filter 
$.extend($.fn.datagrid.defaults.filters, {
customertypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: customertypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   customertype   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
customertypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: customertypedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------水表方向---------//
var directfiltersource = [{ value: '', text: 'All'}];
var directdatasource = [];
directfiltersource.push({ value: '1',text:'进口'  });
directdatasource.push({ value: '1',text:'进口'  });
directfiltersource.push({ value: '-1',text:'出口'  });
directdatasource.push({ value: '-1',text:'出口'  });
//for datagrid direct field  formatter
function directformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = directdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = directdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   direct  field filter 
$.extend($.fn.datagrid.defaults.filters, {
directfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: directfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   direct   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
directeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: directdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------账单生成截至日---------//
var enddayfiltersource = [{ value: '', text: 'All'}];
var enddaydatasource = [];
enddayfiltersource.push({ value: '10',text:'10号'  });
enddaydatasource.push({ value: '10',text:'10号'  });
//for datagrid endday field  formatter
function enddayformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = enddaydatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = enddaydatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   endday  field filter 
$.extend($.fn.datagrid.defaults.filters, {
enddayfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: enddayfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   endday   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
enddayeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: enddaydatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------文件类型---------//
var filetypefiltersource = [{ value: '', text: 'All'}];
var filetypedatasource = [];
filetypefiltersource.push({ value: '0',text:'txt'  });
filetypedatasource.push({ value: '0',text:'txt'  });
filetypefiltersource.push({ value: '1',text:'xls'  });
filetypedatasource.push({ value: '1',text:'xls'  });
filetypefiltersource.push({ value: '10',text:'docx'  });
filetypedatasource.push({ value: '10',text:'docx'  });
filetypefiltersource.push({ value: '11',text:'py'  });
filetypedatasource.push({ value: '11',text:'py'  });
filetypefiltersource.push({ value: '12',text:'c'  });
filetypedatasource.push({ value: '12',text:'c'  });
filetypefiltersource.push({ value: '13',text:'java'  });
filetypedatasource.push({ value: '13',text:'java'  });
filetypefiltersource.push({ value: '2',text:'pdf'  });
filetypedatasource.push({ value: '2',text:'pdf'  });
filetypefiltersource.push({ value: '3',text:'xlsx'  });
filetypedatasource.push({ value: '3',text:'xlsx'  });
filetypefiltersource.push({ value: '4',text:'json'  });
filetypedatasource.push({ value: '4',text:'json'  });
filetypefiltersource.push({ value: '5',text:'js'  });
filetypedatasource.push({ value: '5',text:'js'  });
filetypefiltersource.push({ value: '6',text:'html'  });
filetypedatasource.push({ value: '6',text:'html'  });
filetypefiltersource.push({ value: '7',text:'xml'  });
filetypedatasource.push({ value: '7',text:'xml'  });
filetypefiltersource.push({ value: '8',text:'cs'  });
filetypedatasource.push({ value: '8',text:'cs'  });
filetypefiltersource.push({ value: '9',text:'doc'  });
filetypedatasource.push({ value: '9',text:'doc'  });
//for datagrid FileType field  formatter
function filetypeformatter(value, row, index) { 
     let multiple = true; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = filetypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = filetypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   FileType  field filter 
$.extend($.fn.datagrid.defaults.filters, {
filetypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: filetypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   FileType   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
filetypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: filetypedatasource,
         multiple: true,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------禁用标志---------//
var isdisabledfiltersource = [{ value: '', text: 'All'}];
var isdisableddatasource = [];
isdisabledfiltersource.push({ value: '0',text:'未禁用'  });
isdisableddatasource.push({ value: '0',text:'未禁用'  });
isdisabledfiltersource.push({ value: '1',text:'已禁用'  });
isdisableddatasource.push({ value: '1',text:'已禁用'  });
//for datagrid IsDisabled field  formatter
function isdisabledformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = isdisableddatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = isdisableddatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   IsDisabled  field filter 
$.extend($.fn.datagrid.defaults.filters, {
isdisabledfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: isdisabledfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   IsDisabled   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
isdisablededitor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: isdisableddatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------启用标志---------//
var isenabledfiltersource = [{ value: '', text: 'All'}];
var isenableddatasource = [];
isenabledfiltersource.push({ value: '0',text:'未启用'  });
isenableddatasource.push({ value: '0',text:'未启用'  });
isenabledfiltersource.push({ value: '1',text:'已启用'  });
isenableddatasource.push({ value: '1',text:'已启用'  });
//for datagrid IsEnabled field  formatter
function isenabledformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = isenableddatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = isenableddatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   IsEnabled  field filter 
$.extend($.fn.datagrid.defaults.filters, {
isenabledfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: isenabledfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   IsEnabled   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
isenablededitor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: isenableddatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------已读标志---------//
var isnewfiltersource = [{ value: '', text: 'All'}];
var isnewdatasource = [];
isnewfiltersource.push({ value: '0',text:'未读'  });
isnewdatasource.push({ value: '0',text:'未读'  });
isnewfiltersource.push({ value: '1',text:'已读'  });
isnewdatasource.push({ value: '1',text:'已读'  });
//for datagrid IsNew field  formatter
function isnewformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = isnewdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = isnewdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   IsNew  field filter 
$.extend($.fn.datagrid.defaults.filters, {
isnewfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: isnewfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   IsNew   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
isneweditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: isnewdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------通知标志---------//
var isnoticefiltersource = [{ value: '', text: 'All'}];
var isnoticedatasource = [];
isnoticefiltersource.push({ value: '0',text:'未发'  });
isnoticedatasource.push({ value: '0',text:'未发'  });
isnoticefiltersource.push({ value: '1',text:'已发'  });
isnoticedatasource.push({ value: '1',text:'已发'  });
//for datagrid IsNotice field  formatter
function isnoticeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = isnoticedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = isnoticedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   IsNotice  field filter 
$.extend($.fn.datagrid.defaults.filters, {
isnoticefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: isnoticefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   IsNotice   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
isnoticeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: isnoticedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------NLogLevel---------//
var levelfiltersource = [{ value: '', text: 'All'}];
var leveldatasource = [];
levelfiltersource.push({ value: 'Debug',text:'Debug'  });
leveldatasource.push({ value: 'Debug',text:'Debug'  });
levelfiltersource.push({ value: 'Error',text:'Error'  });
leveldatasource.push({ value: 'Error',text:'Error'  });
levelfiltersource.push({ value: 'Fatal',text:'Fatal'  });
leveldatasource.push({ value: 'Fatal',text:'Fatal'  });
levelfiltersource.push({ value: 'Info',text:'Info'  });
leveldatasource.push({ value: 'Info',text:'Info'  });
levelfiltersource.push({ value: 'Trace',text:'Trace'  });
leveldatasource.push({ value: 'Trace',text:'Trace'  });
levelfiltersource.push({ value: 'Warn',text:'Warn'  });
leveldatasource.push({ value: 'Warn',text:'Warn'  });
//for datagrid Level field  formatter
function levelformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = leveldatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = leveldatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   Level  field filter 
$.extend($.fn.datagrid.defaults.filters, {
levelfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: levelfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   Level   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
leveleditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: leveldatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------日志分组---------//
var messagegroupfiltersource = [{ value: '', text: 'All'}];
var messagegroupdatasource = [];
messagegroupfiltersource.push({ value: '0',text:'系统操作'  });
messagegroupdatasource.push({ value: '0',text:'系统操作'  });
messagegroupfiltersource.push({ value: '1',text:'业务操作'  });
messagegroupdatasource.push({ value: '1',text:'业务操作'  });
messagegroupfiltersource.push({ value: '2',text:'接口操作'  });
messagegroupdatasource.push({ value: '2',text:'接口操作'  });
//for datagrid MessageGroup field  formatter
function messagegroupformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = messagegroupdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = messagegroupdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   MessageGroup  field filter 
$.extend($.fn.datagrid.defaults.filters, {
messagegroupfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: messagegroupfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   MessageGroup   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
messagegroupeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: messagegroupdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------日志类型---------//
var messagetypefiltersource = [{ value: '', text: 'All'}];
var messagetypedatasource = [];
messagetypefiltersource.push({ value: '0',text:'Information'  });
messagetypedatasource.push({ value: '0',text:'Information'  });
messagetypefiltersource.push({ value: '1',text:'Error'  });
messagetypedatasource.push({ value: '1',text:'Error'  });
messagetypefiltersource.push({ value: '2',text:'Alert'  });
messagetypedatasource.push({ value: '2',text:'Alert'  });
//for datagrid MessageType field  formatter
function messagetypeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = messagetypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = messagetypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   MessageType  field filter 
$.extend($.fn.datagrid.defaults.filters, {
messagetypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: messagetypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   MessageType   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
messagetypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: messagetypedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------水表状态---------//
var meterstatusfiltersource = [{ value: '', text: 'All'}];
var meterstatusdatasource = [];
meterstatusfiltersource.push({ value: '使用中',text:'使用中'  });
meterstatusdatasource.push({ value: '使用中',text:'使用中'  });
meterstatusfiltersource.push({ value: '停用',text:'停用'  });
meterstatusdatasource.push({ value: '停用',text:'停用'  });
meterstatusfiltersource.push({ value: '周转',text:'周转'  });
meterstatusdatasource.push({ value: '周转',text:'周转'  });
meterstatusfiltersource.push({ value: '在库',text:'在库'  });
meterstatusdatasource.push({ value: '在库',text:'在库'  });
meterstatusfiltersource.push({ value: '报废',text:'报废'  });
meterstatusdatasource.push({ value: '报废',text:'报废'  });
meterstatusfiltersource.push({ value: '返修',text:'返修'  });
meterstatusdatasource.push({ value: '返修',text:'返修'  });
//for datagrid meterstatus field  formatter
function meterstatusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = meterstatusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = meterstatusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   meterstatus  field filter 
$.extend($.fn.datagrid.defaults.filters, {
meterstatusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: meterstatusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   meterstatus   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
meterstatuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: meterstatusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------水表类型---------//
var metertypefiltersource = [{ value: '', text: 'All'}];
var metertypedatasource = [];
metertypefiltersource.push({ value: '智能表',text:'智能表'  });
metertypedatasource.push({ value: '智能表',text:'智能表'  });
metertypefiltersource.push({ value: '机械表',text:'机械表'  });
metertypedatasource.push({ value: '机械表',text:'机械表'  });
//for datagrid metertype field  formatter
function metertypeformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = metertypedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = metertypedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   metertype  field filter 
$.extend($.fn.datagrid.defaults.filters, {
metertypefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: metertypefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   metertype   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
metertypeeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: metertypedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------增减项---------//
var negitivefiltersource = [{ value: '', text: 'All'}];
var negitivedatasource = [];
negitivefiltersource.push({ value: '正项',text:'正项'  });
negitivedatasource.push({ value: '正项',text:'正项'  });
negitivefiltersource.push({ value: '负项',text:'负项'  });
negitivedatasource.push({ value: '负项',text:'负项'  });
//for datagrid negitive field  formatter
function negitiveformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = negitivedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = negitivedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   negitive  field filter 
$.extend($.fn.datagrid.defaults.filters, {
negitivefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: negitivefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   negitive   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
negitiveeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: negitivedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------通知分组---------//
var notifygroupfiltersource = [{ value: '', text: 'All'}];
var notifygroupdatasource = [];
notifygroupfiltersource.push({ value: '0',text:'系统推送'  });
notifygroupdatasource.push({ value: '0',text:'系统推送'  });
notifygroupfiltersource.push({ value: '1',text:'业务推送'  });
notifygroupdatasource.push({ value: '1',text:'业务推送'  });
notifygroupfiltersource.push({ value: '2',text:'接口推送'  });
notifygroupdatasource.push({ value: '2',text:'接口推送'  });
notifygroupfiltersource.push({ value: '3',text:'手工推送'  });
notifygroupdatasource.push({ value: '3',text:'手工推送'  });
//for datagrid NotifyGroup field  formatter
function notifygroupformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = notifygroupdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = notifygroupdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   NotifyGroup  field filter 
$.extend($.fn.datagrid.defaults.filters, {
notifygroupfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: notifygroupfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   NotifyGroup   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
notifygroupeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: notifygroupdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------优先级---------//
var priorityfiltersource = [{ value: '', text: 'All'}];
var prioritydatasource = [];
priorityfiltersource.push({ value: '1',text:'高'  });
prioritydatasource.push({ value: '1',text:'高'  });
priorityfiltersource.push({ value: '2',text:'中'  });
prioritydatasource.push({ value: '2',text:'中'  });
priorityfiltersource.push({ value: '3',text:'低'  });
prioritydatasource.push({ value: '3',text:'低'  });
priorityfiltersource.push({ value: '4',text:'低级'  });
prioritydatasource.push({ value: '4',text:'低级'  });
//for datagrid Priority field  formatter
function priorityformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = prioritydatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = prioritydatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   Priority  field filter 
$.extend($.fn.datagrid.defaults.filters, {
priorityfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: priorityfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   Priority   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
priorityeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: prioritydatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------NLogResolved---------//
var resolvedfiltersource = [{ value: '', text: 'All'}];
var resolveddatasource = [];
resolvedfiltersource.push({ value: 'false',text:'未处理'  });
resolveddatasource.push({ value: 'false',text:'未处理'  });
resolvedfiltersource.push({ value: 'true',text:'已处理'  });
resolveddatasource.push({ value: 'true',text:'已处理'  });
//for datagrid Resolved field  formatter
function resolvedformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = resolveddatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = resolveddatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   Resolved  field filter 
$.extend($.fn.datagrid.defaults.filters, {
resolvedfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: resolvedfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   Resolved   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
resolvededitor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: resolveddatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------客户状态---------//
var statusfiltersource = [{ value: '', text: 'All'}];
var statusdatasource = [];
statusfiltersource.push({ value: '停用',text:'停用'  });
statusdatasource.push({ value: '停用',text:'停用'  });
statusfiltersource.push({ value: '启用',text:'启用'  });
statusdatasource.push({ value: '启用',text:'启用'  });
//for datagrid status field  formatter
function statusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = statusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = statusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   status  field filter 
$.extend($.fn.datagrid.defaults.filters, {
statusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: statusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   status   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
statuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: statusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------单位代码---------//
var unitfiltersource = [{ value: '', text: 'All'}];
var unitdatasource = [];
unitfiltersource.push({ value: '001',text:'台'  });
unitdatasource.push({ value: '001',text:'台'  });
unitfiltersource.push({ value: '002',text:'座'  });
unitdatasource.push({ value: '002',text:'座'  });
unitfiltersource.push({ value: '003',text:'辆'  });
unitdatasource.push({ value: '003',text:'辆'  });
unitfiltersource.push({ value: '004',text:'艘'  });
unitdatasource.push({ value: '004',text:'艘'  });
unitfiltersource.push({ value: '005',text:'架'  });
unitdatasource.push({ value: '005',text:'架'  });
unitfiltersource.push({ value: '006',text:'套'  });
unitdatasource.push({ value: '006',text:'套'  });
unitfiltersource.push({ value: '007',text:'个'  });
unitdatasource.push({ value: '007',text:'个'  });
unitfiltersource.push({ value: '035',text:'千克'  });
unitdatasource.push({ value: '035',text:'千克'  });
//for datagrid unit field  formatter
function unitformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = unitdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = unitdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   unit  field filter 
$.extend($.fn.datagrid.defaults.filters, {
unitfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: unitfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   unit   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
uniteditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: unitdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------水价---------//
var waterfiltersource = [{ value: '', text: 'All'}];
var waterdatasource = [];
waterfiltersource.push({ value: '3.5',text:'水价'  });
waterdatasource.push({ value: '3.5',text:'水价'  });
//for datagrid water field  formatter
function waterformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = waterdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = waterdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   water  field filter 
$.extend($.fn.datagrid.defaults.filters, {
waterfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: waterfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   water   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
watereditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: waterdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------水表级数---------//
var waterlevelfiltersource = [{ value: '', text: 'All'}];
var waterleveldatasource = [];
waterlevelfiltersource.push({ value: '一级',text:'一级'  });
waterleveldatasource.push({ value: '一级',text:'一级'  });
waterlevelfiltersource.push({ value: '二级',text:'二级'  });
waterleveldatasource.push({ value: '二级',text:'二级'  });
//for datagrid waterlevel field  formatter
function waterlevelformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = waterleveldatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = waterleveldatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   waterlevel  field filter 
$.extend($.fn.datagrid.defaults.filters, {
waterlevelfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: waterlevelfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   waterlevel   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
waterleveleditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: waterleveldatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------任务状态---------//
var workstatusfiltersource = [{ value: '', text: 'All'}];
var workstatusdatasource = [];
workstatusfiltersource.push({ value: '0',text:'等待指派'  });
workstatusdatasource.push({ value: '0',text:'等待指派'  });
workstatusfiltersource.push({ value: '1',text:'未开始'  });
workstatusdatasource.push({ value: '1',text:'未开始'  });
workstatusfiltersource.push({ value: '2',text:'进行中'  });
workstatusdatasource.push({ value: '2',text:'进行中'  });
workstatusfiltersource.push({ value: '3',text:'已完成'  });
workstatusdatasource.push({ value: '3',text:'已完成'  });
workstatusfiltersource.push({ value: '4',text:'关闭'  });
workstatusdatasource.push({ value: '4',text:'关闭'  });
//for datagrid workstatus field  formatter
function workstatusformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = workstatusdatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = workstatusdatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   workstatus  field filter 
$.extend($.fn.datagrid.defaults.filters, {
workstatusfilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: workstatusfiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   workstatus   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
workstatuseditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: workstatusdatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
//-------区域---------//
var zonefiltersource = [{ value: '', text: 'All'}];
var zonedatasource = [];
zonefiltersource.push({ value: '区域1',text:'区域1'  });
zonedatasource.push({ value: '区域1',text:'区域1'  });
zonefiltersource.push({ value: '区域2',text:'区域2'  });
zonedatasource.push({ value: '区域2',text:'区域2'  });
zonefiltersource.push({ value: '区域3',text:'区域3'  });
zonedatasource.push({ value: '区域3',text:'区域3'  });
//for datagrid zone field  formatter
function zoneformatter(value, row, index) { 
     let multiple = false; 
     if (value === null || value === '' || value === undefined) 
     { 
         return "";
     } 
     if (multiple) { 
         let valarray = value.split(','); 
         let result = zonedatasource.filter(item => valarray.includes(item.value));
         let textarray = result.map(x => x.text);
         if (textarray.length > 0)
             return textarray.join(",");
         else 
             return value;
      } else { 
         let result = zonedatasource.filter(x => x.value == value);
               if (result.length > 0)
                    return result[0].text;
               else
                    return value;
       } 
 } 
//for datagrid   zone  field filter 
$.extend($.fn.datagrid.defaults.filters, {
zonefilter: {
     init: function(container, options) {
        var input = $('<select class="easyui-combobox" >').appendTo(container);
        var myoptions = {
             panelHeight: 'auto',
             editable: false,
             data: zonefiltersource ,
             onChange: function () {
                input.trigger('combobox.filter');
             }
         };
         $.extend(options, myoptions);
         input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
         return input;
      },
     destroy: function(target) {
                  
     },
     getValue: function(target) {
         return $(target).combobox('getValue');
     },
     setValue: function(target, value) {
         $(target).combobox('setValue', value);
     },
     resize: function(target, width) {
         $(target).combobox('resize', width);
     }
   }
});
//for datagrid   zone   field  editor 
$.extend($.fn.datagrid.defaults.editors, {
zoneeditor: {
     init: function(container, options) {
        var input = $('<input type="text">').appendTo(container);
        var myoptions = {
         panelHeight: 'auto',
         editable: false,
         data: zonedatasource,
         multiple: false,
         valueField: 'value',
         textField: 'text'
     };
    $.extend(options, myoptions);
           input.combobox(options);
         input.combobox('textbox').bind('keydown', function (e) {   
            if (e.keyCode === 13) {
              $(e.target).emulateTab();
            }
          });  
           return input;
       },
     destroy: function(target) {
         $(target).combobox('destroy');
        },
     getValue: function(target) {
        let opts = $(target).combobox('options');
        if (opts.multiple) {
           return $(target).combobox('getValues').join(opts.separator);
         } else {
            return $(target).combobox('getValue');
         }
        },
     setValue: function(target, value) {
         let opts = $(target).combobox('options');
         if (opts.multiple) {
             if (value == '' || value == null) { 
                 $(target).combobox('clear'); 
              } else { 
                  $(target).combobox('setValues', value.split(opts.separator));
               }
          }
          else {
             $(target).combobox('setValue', value);
           }
         },
     resize: function(target, width) {
         $(target).combobox('resize', width);
        }
  }  
});
