using System;
using System.Globalization;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public abstract class AbstractInteractionViewModel
    {
        static readonly CultureInfo CurCultureInfo = new CultureInfo("en-us");
        /// <summary/>
        public string CreateTime { get; }
        /// <summary/>
        public string CurrentStep { get; }
        /// <summary/>
        public string Handler { get; }

        /// <summary/>
        protected AbstractInteractionViewModel(DateTime creatTime, string currentStep, string handler)
        {
            CreateTime = creatTime.ToString(CurCultureInfo);
            CurrentStep = currentStep;
            Handler = handler;
        }
    }
}