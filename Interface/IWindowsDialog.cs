using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSTK_Lite.Interface
{
    public interface IWindowsDialog
    {
        /// <summary>
        ///     Transfiere el DataContext a la ventana de diálogo usando como parámetro el ViewModel.
        /// </summary>
        void BindViewModel<TViewModel>(TViewModel viewModel);

        /// <summary>
        ///     Abre la ventana de diálogo.
        /// </summary>
        void ShowDialog();

        /// <summary>
        ///     Cierra la ventana de diálogo.
        /// </summary>
        void CloseDialog();
    }
}
