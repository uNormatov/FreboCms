using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FUIControls.FormControl
{
    public interface IComponentControl
    {
        bool IsComponent();
        void SelectData(int contentTypeId, int contentId, string fieldName);
        void InsertData(int contentTypeId, int contentId, string fieldName);
        void UpdateData(int contentType, int contentId, string fieldName);

    }
}
