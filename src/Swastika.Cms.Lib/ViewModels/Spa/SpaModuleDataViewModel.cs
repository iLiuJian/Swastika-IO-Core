﻿// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0 license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swastika.Cms.Lib.Models.Cms;
using Swastika.Cms.Lib.Services;
using Swastika.Common.Helper;
using Swastika.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swastika.Cms.Lib.ViewModels.Spa
{
    public class SpaModuleDataViewModel
       : ViewModelBase<SiocCmsContext, SiocModuleData, SpaModuleDataViewModel>
    {
        #region Properties

        #region Models

        [JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("moduleId")]
        //public int ModuleId { get; set; }
        //[JsonProperty("fields")]
        //public string Fields { get; set; } = "[]";
        //[JsonProperty("value")]
        [JsonIgnore]
        public string Value { get; set; }

        //[JsonProperty("articleId")]
        //public string ArticleId { get; set; }
        //[JsonProperty("productId")]
        //public string ProductId { get; set; }
        //[JsonProperty("categoryId")]
        //public int? CategoryId { get; set; }
        //[JsonProperty("createdDateTime")]
        //public DateTime CreatedDateTime { get; set; }
        //[JsonProperty("updatedDateTime")]
        //public DateTime? UpdatedDateTime { get; set; }

        #endregion Models

        #region Views

        public List<ModuleDataValueViewModel> DataProperties { get; set; }
        public List<ModuleFieldViewModel> Columns { get; set; }
        public JObject JItem { get { return ParseJson(); } }

        #endregion Views

        #endregion Properties

        #region Contructors

        public SpaModuleDataViewModel() : base()
        {
        }

        public SpaModuleDataViewModel(SiocModuleData model, SiocCmsContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructors

        #region Overrides

        public override void ExpandView(SiocCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            IsClone = true;
            ListSupportedCulture = GlobalLanguageService.ListSupportedCulture;

            var objValue = Value != null ? JObject.Parse(Value) : new JObject();

            this.DataProperties = new List<ModuleDataValueViewModel>();
            //Columns = new List<ModuleFieldViewModel>(); // ModuleRepository.GetInstance().GetColumns(m => m.Id == ModuleId && m.Specificulture == Specificulture);
            //Fields = SpaModuleViewModel.Repository.GetSingleModel(m => m.Id == ModuleId && m.Specificulture == Specificulture, _context, _transaction).Data.Fields;
            //this.Columns = new List<ModuleFieldViewModel>();
            //if (!string.IsNullOrEmpty(Fields))
            //{
            //    JArray arrField = JArray.Parse(Fields);

            //    foreach (var field in arrField)
            //    {
            //        ModuleFieldViewModel thisField = new ModuleFieldViewModel()
            //        {
            //            Name = CommonHelper.ParseJsonPropertyName(field["Name"].ToString()),
            //            DataType = (SWCmsConstants.DataType)(int)field["DataType"],
            //            Width = field["Width"] != null ? field["Width"].Value<int>() : 3,
            //            IsDisplay = field["IsDisplay"] != null ? field["IsDisplay"].Value<bool>() : true
            //        };
            //        this.Columns.Add(thisField);
            //    }
            //}
            //foreach (var col in Columns)
            //{
            //    //    foreach (var field in objValue.Properties())
            //    //{
            //    JProperty prop = objValue.Property(col.Name);
            //    if (prop == null)
            //    {
            //        JObject val = new JObject
            //        {
            //            { "dataType", (int)col.DataType },
            //            { "value", null }
            //        };
            //        prop = new JProperty(col.Name, val);
            //    }
            //    //foreach (var prop in objValue.Properties())
            //    //{
            //    var dataVal = new ModuleDataValueViewModel()
            //    {
            //        ModuleId = ModuleId,
            //        DataType = (SWCmsConstants.DataType)col.DataType,
            //        Name = CommonHelper.ParseJsonPropertyName(prop.Name),
            //        StringValue = prop.Value["value"].Value<string>()
            //    };
            //    switch (col.DataType)
            //    {
            //        case SWCmsConstants.DataType.Int:
            //            dataVal.Value = prop.Value["value"].HasValues ? prop.Value["value"].Value<int>() : 0;
            //            break;

            //        case SWCmsConstants.DataType.Boolean:
            //            dataVal.Value = !string.IsNullOrEmpty(prop.Value["value"].ToString()) ? prop.Value["value"].Value<bool>() : false;
            //            break;
            //        case SWCmsConstants.DataType.String:
            //        case SWCmsConstants.DataType.Image:
            //        case SWCmsConstants.DataType.Icon:
            //        case SWCmsConstants.DataType.CodeEditor:
            //        case SWCmsConstants.DataType.Html:
            //        case SWCmsConstants.DataType.TextArea:
            //        default:
            //            dataVal.Value = prop.Value["value"].Value<string>();
            //            break;
            //    }
            //    this.DataProperties.Add(dataVal);
            //    //}
            //}
        }

        #endregion Overrides

        #region Expands

        public string ParseObjectValue()
        {
            JObject result = new JObject();
            foreach (var prop in DataProperties)
            {
                JObject obj = new JObject();
                obj.Add(new JProperty("dataType", prop.DataType));
                obj.Add(new JProperty("value", prop.StringValue));
                result.Add(new JProperty(CommonHelper.ParseJsonPropertyName(prop.Name), obj));
            }
            return result.ToString(Formatting.None);
        }

        public string GetStringValue(string name)
        {
            var prop = DataProperties.FirstOrDefault(p => p.Name == name);
            return prop != null && prop.Value != null ? prop.Value.ToString() : string.Empty;
        }

        public T GetValue<T>(string name) where T : IConvertible
        {
            var prop = DataProperties.FirstOrDefault(p => p.Name == name);
            return prop != null && prop.Value != null ? (T)prop.Value : default(T);
        }

        public ModuleDataValueViewModel GetDataProperty(string name)
        {
            return DataProperties.FirstOrDefault(p => p.Name == name);
        }

        public JObject ParseJson()
        {
            JObject result = new JObject
            {
                new JProperty("id", Id)
            };
            foreach (var prop in DataProperties)
            {
                result.Add(new JProperty(CommonHelper.ParseJsonPropertyName(prop.Name), prop.Value));
            }
            JObject model = new JObject
            {
                new JProperty("id", Id),
                //new JProperty("moduleId", ModuleId),
                new JProperty("specificulture", Specificulture),
                //new JProperty("fields", Fields),
                new JProperty("value", Value),
                //new JProperty("articleId", ArticleId),
                new JProperty("priority", Priority),
                //new JProperty("categoryId", CategoryId),
                //new JProperty("createdDateTime", CreatedDateTime)
            };
            //result.Add(new JProperty("model", model));
            return result;
        }

        #endregion Expands
    }

    public class ModuleDataValueViewModel
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public SWCmsConstants.DataType DataType { get; set; }
        public IConvertible Value { get; set; }

        public string StringValue { get; set; }

        public T GetValue<T>()
        {
            return this.Value != null ? (T)Value : default(T);
        }
    }
}
