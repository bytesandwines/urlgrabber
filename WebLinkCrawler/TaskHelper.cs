using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLinkCrawler
{
    public class TaskHelper
    {
        public static void RemoveTaskList<T>(ref List<Task<T>> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].Dispose();
                tasks[i] = null;
            }

            tasks = null;
        }
    }
}
