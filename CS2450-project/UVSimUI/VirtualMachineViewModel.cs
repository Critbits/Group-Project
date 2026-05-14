using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UVSIM;

namespace UVSimUI;

public class VirtualMachineViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public VirtualMachine VM { get; set; }

    public VirtualMachineViewModel()
    {
        VM = new VirtualMachine();
    }

    //destructor?
    //~VirtualMachineViewModel() => ;

    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); 
}

