﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class GeneratePdf : AsPdfResultBase, IGeneratePdf
    {
        readonly IRazorViewToStringRenderer _engine;
        public GeneratePdf(IRazorViewToStringRenderer engine)
        {
            _engine = engine;
        }

        public byte[] GetPDF(string html)
        {
            return WkhtmlDriver.Convert(WkhtmltopdfConfiguration.RotativaPath, this.GetConvertOptions(), html);
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model)
        {
            try
            {                
                var html = await _engine.RenderViewToStringAsync(View, model);
                return GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model)
        {
            try
            {
                var view = await _engine.RenderHtmlToStringAsync(ViewInHtml, model);
                return GetPDF(view);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddView(string path, string viewHTML) => _engine.AddView(path, viewHTML);

        public bool ExistsView(string path) => _engine.ExistsView(path);

        public void UpdateView(string path, string viewHTML) => _engine.UpdateView(path, viewHTML);
    }
}