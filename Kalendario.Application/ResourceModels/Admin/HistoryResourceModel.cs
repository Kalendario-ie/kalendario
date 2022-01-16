using System;
using Kalendario.Application.ResourceModels.Admin;

namespace Kalendario.Application.ResourceModels;

public class HistoryResourceModel
{
    public ApplicationUserAdminResourceModel User { set; get; }
    
    public DateTime Date { get; set; }

    public string EntityState { get; set; }
}