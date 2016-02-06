using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.Common.DomainLogic;

namespace TaskTimeTracker.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DutyProvider provider = new DutyProvider();

            provider.StartNewDuty();

            provider.StartNewDuty();

            provider.FinishDutyAndUnpausePrevious();

            provider.StartNewDuty();

            provider.FinishDutyAndStartNew();

            provider.FinishDutyAndStartNew();

            var duty1 = provider.Iteration.Duties[0];

            provider.UnpauseDuty(duty1);
        }
    }
}
