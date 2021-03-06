﻿// Licensed to the Swastika I/O Foundation under one or more agreements.
// The Swastika I/O Foundation licenses this file to you under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Swastika.Cms.Lib.Models.Cms;
using Swastika.Cms.Lib.ViewModels.Info;
using Swastika.Domain.Data.ViewModels;

namespace Swastika.Cms.Lib.ViewModels.Navigation
{
    public class NavCategoryProductViewModel
        : ViewModelBase<SiocCmsContext, SiocCategoryProduct, NavCategoryProductViewModel>
    {
        public NavCategoryProductViewModel(SiocCategoryProduct model, SiocCmsContext _context = null, IDbContextTransaction _transaction = null)
            : base(model, _context, _transaction)
        {
        }

        public NavCategoryProductViewModel() : base()
        {
        }
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("isActived")]
        public bool IsActived { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        #region Views
        [JsonProperty("product")]
        public InfoProductViewModel Product { get; set; }

        #endregion Views

        #region overrides

        public override void ExpandView(SiocCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            var getProduct = InfoProductViewModel.Repository.GetSingleModel(p => p.Id == ProductId && p.Specificulture == Specificulture
                , _context: _context, _transaction: _transaction
            );
            if (getProduct.IsSucceed)
            {
                Product = getProduct.Data;
            }
        }

        #region Async

        //public override async Task<RepositoryResponse<CategoryProductViewModel>> CloneAsync(string desSpecificulture, SiocCmsContext _context = null, IDbContextTransaction _transaction = null)
        //{
        //    //Check is destinate cate and product already defined in des culture
        //    bool isValidDes = CategoryListItemViewModel.Repository.CheckIsExists(
        //        c => c.Id == this.CategoryId && c.Specificulture == desSpecificulture, _context, _transaction)
        //    && CategoryListItemViewModel.Repository.CheckIsExists(
        //        c => c.Id == this.CategoryId && c.Specificulture == desSpecificulture, _context, _transaction);
        //    RepositoryResponse<CategoryProductViewModel> result = new RepositoryResponse<CategoryProductViewModel>();

        //    if (isValidDes)
        //    {
        //        var data = new CategoryProductViewModel(
        //            new SiocCategoryProduct()
        //            {
        //                ProductId = this.ProductId,
        //                Specificulture = desSpecificulture,
        //                CategoryId = this.CategoryId
        //            },
        //            _context, _transaction)
        //        {
        //            IsActived = this.IsActived,
        //            Description = this.Description
        //        };
        //        var saveResult = await data.SaveModelAsync(_context: _context, _transaction: _transaction);
        //        if (saveResult.IsSucceed)
        //        {
        //            result.IsSucceed = true;
        //            result.Data = data;
        //        }
        //        return result;
        //    }
        //    else
        //    {
        //        return result;
        //    }

        //}

        #endregion Async

        #endregion overrides
    }
}
