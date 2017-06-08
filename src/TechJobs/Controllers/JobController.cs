using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TechJobs.Data;
using TechJobs.Models;
using TechJobs.ViewModels;

namespace TechJobs.Controllers
{
    public class JobController : Controller
    {

        // Our reference to the data store
        private static JobData jobData;

        static JobController()
        {
            jobData = JobData.GetInstance();
        }

        // The detail display for a given Job at URLs like /Job?id=17
        public IActionResult Index(int id)
        {
            // TODO #1 - get the Job with the given ID and pass it into the view
            Job idJob = jobData.Find(id);
            return View(idJob);
        }

        public IActionResult New()
        {
            NewJobViewModel newJobViewModel = new NewJobViewModel();
            return View(newJobViewModel);
        }

        [HttpPost]
        public IActionResult New(NewJobViewModel newJobViewModel)
        {
            // TODO #6 - Validate the ViewModel and if valid, create a 
            // new Job and add it to the JobData data store. Then
            // redirect to the Job detail (Index) action/view for the new Job.

            var employer = jobData.Employers.Find(newJobViewModel.EmployerID);
            var location = jobData.Locations.Find(newJobViewModel.LocationID);
            var skill = jobData.CoreCompetencies.Find(newJobViewModel.CoreCompetencyID);
            var position = jobData.PositionTypes.Find(newJobViewModel.PositionID);

            if (!String.IsNullOrEmpty(newJobViewModel.Name)
                && !String.IsNullOrEmpty(employer.Value)
                && !String.IsNullOrEmpty(location.Value)
                && !String.IsNullOrEmpty(skill.Value)
                && !String.IsNullOrEmpty(position.Value))
            {
                Job newJob = new Job
                {
                    Name = newJobViewModel.Name,
                    Employer = employer,
                    Location = location,
                    CoreCompetency = skill,
                    PositionType = position
                };

                jobData.Jobs.Add(newJob);

                return Redirect("/Job?id=" + newJob.ID);
            }
            
            return View(newJobViewModel);
        }
    }
}
