using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DevTest;
using PagedList;

namespace DevTest.Controllers
{
    public class ResultController : Controller
    {
        private DevTestAdminEntities db = new DevTestAdminEntities();
        private string defaultSite = "Select a site";
        private int pageSize = 5;

        // GET: Result
        public ActionResult Index()
        {
            var devTestUserResults = db.devTestUserResults.Include(d => d.devTestUser);

            if (devTestUserResults == null)
            {
                return HttpNotFound();
            }

            List<string> siteList = new List<string>();
            siteList.Add(defaultSite);

            // try catch in case underlying data provider fails to open
            try
            {
                ViewBag.RecordCount = devTestUserResults.Count();
                siteList.AddRange(devTestUserResults.Select(r => r.devTestUser.site).Distinct());
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }

            ViewBag.Sites = new SelectList(siteList);
            ViewBag.ScoreLength = devTestUserResults.Select(r => r.testResultDenominator).Max().ToString().Length;
            ViewBag.DefaultSite = defaultSite;
            ViewBag.Record = 1;

            devTestUserResults = devTestUserResults.OrderBy(r => r.id);

            return View(devTestUserResults.ToPagedList(1, pageSize));
        }

        public ActionResult UpdateTable(string sort, string name, string site, string lowScore, string upScore, string dateStart, string dateEnd, int? page, int? record)
        {
            var devTestUserResults = db.devTestUserResults.Include(d => d.devTestUser);
            ViewBag.RecordCount = devTestUserResults.Count();
            int minScore = 0;
            int maxScore = devTestUserResults.Select(r => r.testResultDenominator).Max();

            if (!String.IsNullOrEmpty(name))
            {
                devTestUserResults = devTestUserResults.Where(r => r.devTestUser.firstName.Contains(name)
                                        || r.devTestUser.lastName.Contains(name));
            }

            if (!String.IsNullOrEmpty(site) && site != defaultSite)
            {
                devTestUserResults = devTestUserResults.Where(r => r.devTestUser.site == site);
            }

            if (!String.IsNullOrEmpty(lowScore))
            {
                Int32.TryParse(lowScore, out minScore);
            }

            if (!String.IsNullOrEmpty(upScore))
            {
                Int32.TryParse(upScore, out maxScore);
            }

            devTestUserResults = devTestUserResults.Where(r => r.testResultNumerator >= minScore && r.testResultNumerator <= maxScore);

            if (!String.IsNullOrEmpty(dateStart) && !String.IsNullOrEmpty(dateEnd))
            {
                DateTime start;
                DateTime end;
                if (DateTime.TryParse(dateStart, out start))
                {
                    if (DateTime.TryParse(dateEnd, out end))
                    {
                        devTestUserResults = devTestUserResults.Where(r => r.dateTaken >= start && r.dateTaken <= end);
                    }
                }
            }

            switch (sort)
            {
                case "firstNameDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.devTestUser.firstName);
                    break;
                case "firstNameAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.devTestUser.firstName);
                    break;
                case "lastNameDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.devTestUser.lastName);
                    break;
                case "lastNameAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.devTestUser.lastName);
                    break;
                case "siteDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.devTestUser.site);
                    break;
                case "siteAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.devTestUser.site);
                    break;
                case "joinedDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.devTestUser.dateCreated);
                    break;
                case "joinedAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.devTestUser.dateCreated);
                    break;
                case "scoreDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.testResultNumerator);
                    break;
                case "scoreAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.testResultNumerator);
                    break;
                case "takenDesc":
                    devTestUserResults = devTestUserResults.OrderByDescending(r => r.dateTaken);
                    break;
                case "takenAsc":
                    devTestUserResults = devTestUserResults.OrderBy(r => r.dateTaken);
                    break;
                default:
                    devTestUserResults = devTestUserResults.OrderBy(r => r.id);
                    break;
            }

            // set values for page number and record number if none supplied
            int pageNumber = (page ?? 1);
            int recordNumber = (record ?? 1);
            // validate page number and record number
            pageNumber = (pageNumber >= 1 && pageNumber <= ((devTestUserResults.Count() / pageSize) + 1)) ? pageNumber : 1;
            recordNumber = (recordNumber <= pageSize) ? recordNumber : 1;

            ViewBag.Sort = sort;
            ViewBag.Record = recordNumber;

            return PartialView("_ResultTable", devTestUserResults.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
