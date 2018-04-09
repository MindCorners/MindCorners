using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Models;
using MindCorners.Models.Results;

namespace MindCorners.DAL
{
    public class TextTemplateRepository : RestService
    {   
        public async Task<List<TextTemplate>> GetAll()
        {
            return await GetResult<List<TextTemplate>>(string.Format("TextTemplate/GetAll"));
        }
    }
}
