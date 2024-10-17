using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public int GarconId { get; set; }
        public List<Prato> PratosSelecionados { get; set; } = new List<Prato>();
        public DateTime DataPedido { get; set; }
    }
}
