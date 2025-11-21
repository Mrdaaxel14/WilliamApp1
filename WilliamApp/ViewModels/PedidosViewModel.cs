using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilliamApp.Services;
using WilliamApp.Models;

namespace WilliamApp.ViewModels
{
    public class PedidosViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Pedido> Pedidos { get; set; }
        private PedidoService service;

        public event PropertyChangedEventHandler PropertyChanged;

        public PedidosViewModel()
        {
            service = new PedidoService();
            Pedidos = new ObservableCollection<Pedido>();
            Cargar();
        }

        private async void Cargar()
        {
            var lista = await service.MisPedidos();
            Pedidos.Clear();
            foreach (var p in lista)
                Pedidos.Add(p);
        }
    }
}

