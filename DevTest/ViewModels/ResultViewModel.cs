using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevTest.ViewModels
{
    public class ResultViewModel
    {
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "Site")]
        public string site { get; set; }
        [Display(Name = "Date Joined")]
        public System.DateTime dateCreated { get; set; }
        [Display(Name = "Test Date")]
        public System.DateTime dateTaken { get; set; }
        [Display(Name = "Test Result")]
        public int testResultNumerator { get; set; }
        public int testResultDenominator { get; set; }

        public string testResult
        {
            get
            {
                return testResultNumerator.ToString("00") + "/" + testResultDenominator.ToString("00");
            }
        }

        public ResultViewModel(devTestUserResult result)
        {
            firstName = result.devTestUser.firstName;
            lastName = result.devTestUser.lastName;
            site = result.devTestUser.site;
            dateCreated = result.devTestUser.dateCreated;
            dateTaken = result.dateTaken;
            testResultNumerator = result.testResultDenominator;
            testResultDenominator = result.testResultDenominator;
        }
    }
}