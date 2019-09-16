using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Helper
{
    public static class CheckHelper
    {
        public static int isExist_Helper(ClientContext context, String fieldToCheck, String type)
        {
            var isExist = 0;
            ListCollection listCollection = context.Web.Lists;
            ContentTypeCollection cntCollection = context.Web.ContentTypes;
            FieldCollection fldCollection = context.Web.Fields;
            switch (type)
            {
                case "list":
                    context.Load(listCollection, lsts => lsts.Include(list => list.Title).Where(list => list.Title == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = listCollection.Count;
                    break;
                case "contenttype":
                    context.Load(cntCollection, cntyp => cntyp.Include(ct => ct.Name).Where(ct => ct.Name == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = cntCollection.Count;
                    break;
                case "contenttypeName":
                    context.Load(cntCollection, cntyp => cntyp.Include(ct => ct.Name, ct => ct.Id).Where(ct => ct.Name == fieldToCheck));
                    context.ExecuteQuery();
                    foreach (ContentType ct in cntCollection)
                    {
                        return 1;
                    }
                    break;
                case "field":
                    context.Load(fldCollection, fld => fld.Include(ft => ft.Title).Where(ft => ft.Title == fieldToCheck));
                    try
                    {
                        context.ExecuteQuery();
                        isExist = fldCollection.Count;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "Unknown Error")
                        {
                            isExist = fldCollection.Count;
                        }
                    }
                    break;
                case "listcntype":
                    List lst = context.Web.Lists.GetByTitle(fieldToCheck);
                    ContentTypeCollection lstcntype = lst.ContentTypes;
                    context.Load(lstcntype, lstc => lstc.Include(lc => lc.Name).Where(lc => lc.Name == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = lstcntype.Count;
                    break;
            }
            return isExist;
        }
    }
}
