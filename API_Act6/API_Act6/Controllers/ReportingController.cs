using API_Act6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using System.Dynamic;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Hosting;
using System.IO;
using System.ComponentModel;
using System.Data;
using API_Act6.Reports;

namespace API_Act6.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class ReportingController : ApiController
    {
        // GET api/<controller>
        [System.Web.Mvc.Route("api/Reporting/getReportData")]
        [HttpGet]
        public dynamic getReportData(int citySelection)
        {
            CarDBEntities db = new CarDBEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<CarPrice> price;

            if(citySelection == 1)
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).Where(gr => db.OnlineSales.Any(cc => cc.CityID == gr.CityID)).ToList();
            }
            else if (citySelection == 2)
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).Where(gr => db.OnsiteSales.Any(cc => cc.CityID == gr.CityID)).ToList();
            }
            else
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).ToList();
            }

            return getExpandoReport(price);
        }

        private dynamic getExpandoReport(List<CarPrice> price)
        {
            dynamic outObject = new ExpandoObject();
            var provList = price.GroupBy(gg => gg.City.Province.ProvinceName);
            List<dynamic> provs = new List<dynamic>();
            foreach(var group in provList)
            {
                dynamic prov = new ExpandoObject();
                prov.ProvinceName = group.Key;
                prov.AveragePrice = group.Average(gg => gg.Price);
                provs.Add(prov);
            }
            outObject.Provinces = provs;

            var cities = price.GroupBy(gg => gg.City.CityName);
            List<dynamic> cityGroups = new List<dynamic>();
            foreach(var group in cities)
            {
                dynamic city = new ExpandoObject();
                city.CityName = group.Key;
                city.AveragePrice = group.Average(gg => gg.Price);
                List<dynamic> flexiPrices = new List<dynamic>();
                foreach (var item in group)
                {
                    dynamic priceObj = new ExpandoObject();
                    priceObj.Car = item.Car.CarMake + " " + item.Car.CarName;
                    priceObj.City = item.City.CityName;
                    priceObj.Price = item.Price;
                    flexiPrices.Add(priceObj);
                }
                city.CarPrices = flexiPrices;
                cityGroups.Add(city);
            }
            outObject.Cities = cityGroups;
            return outObject;
        }

        [System.Web.Mvc.Route("api/Reporting/downloadReport")]
        [HttpGet]
        public HttpResponseMessage downloadReport(int citySelection, int type)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            CarDBEntities db = new CarDBEntities();
            db.Configuration.ProxyCreationEnabled = false;
            List<CarPrice> price;

            if (citySelection == 1)
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).Where(gr => db.OnlineSales.Any(cc => cc.CityID == gr.CityID)).ToList();
            }
            else if (citySelection == 2)
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).Where(gr => db.OnsiteSales.Any(cc => cc.CityID == gr.CityID)).ToList();
            }
            else
            {
                price = db.CarPrices.Include(gg => gg.Car).Include(gg => gg.City).Include(gg => gg.City.Province).ToList();
            }
            return getCityReportFile(price, type);
        }

        private HttpResponseMessage getCityReportFile(List<CarPrice> prices, int FileType)
        {
            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(HostingEnvironment.MapPath("~/Reports/PriceReport.rpt")));

            PriceReportModel model = new PriceReportModel();
            foreach(CarPrice price in prices)
            {
                DataRow row = model.Prices.NewRow();
                row["Car"] = price.Car.CarMake + " " + price.Car.CarName;
                row["Price"] = price.Price;
                row["City"] = price.City.CityName;
                row["Province"] = price.City.Province.ProvinceName;
                model.Prices.Rows.Add(row);
            }
            report.SetDataSource(model);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            if(FileType == 1)
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                httpResponseMessage.Content = new StreamContent(stream);
                httpResponseMessage.Content.Headers.Add("x-filename", "Report.pdf");
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "PriceReport.pdf";
            }
            else
            {
                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.WordForWindows);
                stream.Seek(0, SeekOrigin.Begin);
                httpResponseMessage.Content = new StreamContent(stream);
                httpResponseMessage.Content.Headers.Add("x-filename", "Report.doc");
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/msword");
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "PriceReport.doc";
            }

            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            return httpResponseMessage;
        }
    }
}

   