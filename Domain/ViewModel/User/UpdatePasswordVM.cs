using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel.User;

public class UpdatePasswordVM
{
    public string? Current { get; set; }
    public string? New { get; set; }
    public string? NewAgain { get; set; }
}
