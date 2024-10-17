using Restaurante.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Restaurante.Services
{
    public class RestauranteService
    {
        private string _pathPedido;
        private string _pathPrato;
        private string _pathGarcon;

        public RestauranteService()
        {
            _pathGarcon = Path.Combine(AppContext.BaseDirectory, "Data", "Garcon.json");
            _pathPrato = Path.Combine(AppContext.BaseDirectory, "Data", "Pratos.json");
            _pathPedido = Path.Combine(AppContext.BaseDirectory, "Data", "Pedido.json");
        }
        #region Garcon
        public async Task SalvarGarconAsync(List<Garcon> garcons)
        {
            try
            {
                string directory = Path.GetDirectoryName(_pathGarcon);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(garcons, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_pathGarcon, json);
                Console.WriteLine("Garçons salvos com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar garçons: {ex.Message}");
            }
        }
        public Garcon CriarGarconViaConsole(List<Garcon> garcons)
        {
            Console.WriteLine("Digite o nome do garçom:");
            string nome = Console.ReadLine();

            // Gerar o próximo ID automaticamente
            int novoId = garcons.Any() ? garcons.Max(g => g.Id) + 1 : 1;

            return new Garcon { Id = novoId, Nome = nome };
        }
        public List<Garcon> CarregarGarcons()
        {
            if (File.Exists(_pathGarcon))
            {
                string json = File.ReadAllText(_pathGarcon);
                return JsonSerializer.Deserialize<List<Garcon>>(json) ?? new List<Garcon>();
            }
            return new List<Garcon>();
        }
        public void ListarGarcons(List<Garcon> garcons)
        {
            if (garcons.Count == 0)
            {
                Console.WriteLine("Nenhum garçom registrado.");
                return;
            }

            Console.WriteLine("==== Lista de Garçons ====");
            foreach (var garcon in garcons)
            {
                Console.WriteLine($"ID: {garcon.Id}, Nome: {garcon.Nome}");
            }
            Console.WriteLine("==========================");
        }
        #endregion

        #region Prato
        public async Task SalvarPratoAsync(List<Prato> pratos)
        {
            try
            {
                string directory = Path.GetDirectoryName(_pathPrato);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(pratos, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_pathPrato, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar pratos: {ex.Message}");
            }
        }
        public Prato CriarPratoViaConsole()
        {
            Console.WriteLine("Digite o nome do prato:");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o preço do prato:");
            decimal preco = Convert.ToDecimal(Console.ReadLine());

            return new Prato { Nome = nome, Preco = preco };
        }
        public List<Prato> CarregarPratos()
        {
            if (File.Exists(_pathPrato))
            {
                string json = File.ReadAllText(_pathPrato);
                return JsonSerializer.Deserialize<List<Prato>>(json) ?? new List<Prato>();
            }
            return new List<Prato>();
        }
        public void ListarPratos(List<Prato> pratos)
        {
            if (pratos.Count == 0)
            {
                Console.WriteLine("Nenhum prato registrado.");
                return;
            }

            Console.WriteLine("==== Lista de Pratos ====");
            foreach (var prato in pratos)
            {
                Console.WriteLine($" Nome: {prato.Nome}, Preço: {prato.Preco:C}");
            }
            Console.WriteLine("==========================");
        }
        #endregion

        #region Pedido
        public async Task SalvarPedidoAsync(List<Pedido> pedidos)
        {
            try
            {
                string directory = Path.GetDirectoryName(_pathPedido);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(pedidos, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_pathPedido, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar pedidos: {ex.Message}");
            }
        }
        public List<Pedido> CarregarPedidos()
        {
            if (File.Exists(_pathPedido))
            {
                string json = File.ReadAllText(_pathPedido);
                return JsonSerializer.Deserialize<List<Pedido>>(json) ?? new List<Pedido>();
            }
            return new List<Pedido>();
        }
        public Pedido CriarPedidoViaConsole(List<Garcon> garcons, List<Prato> pratos)
        {
            
            ListarGarcons(garcons);
            Console.WriteLine("Digite o ID do garçom para o pedido:");
            int garconId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite o nome do cliente:");
            string clienteNome = Console.ReadLine();

            List<Prato> pratosSelecionados = new List<Prato>();
            bool adicionarPratos = true;
            while (adicionarPratos)
            {
                ListarPratos(pratos);
                Console.WriteLine("Digite o número do prato para adicionar ao pedido:");
                Console.WriteLine("Se quiser finalizar digite 0");
                int pratoIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                if (pratoIndex >= 0 && pratoIndex < pratos.Count)
                {
                    pratosSelecionados.Add(pratos[pratoIndex]);
                    Console.WriteLine($"Prato {pratos[pratoIndex].Nome} adicionado ao pedido.");

                }
                else
                {
                    Console.WriteLine("Prato inválido.");
                }

                Console.WriteLine("Deseja adicionar mais pratos? (s/n)");
                adicionarPratos = Console.ReadLine()?.ToLower() == "s";
            }

            // Criar o novo pedido
            int novoId = CarregarPedidos().Any() ? CarregarPedidos().Max(p => p.Id) + 1 : 1;

            return new Pedido
            {
                Id = novoId,
                ClienteNome = clienteNome,
                GarconId = garconId,
                PratosSelecionados = pratosSelecionados,
                DataPedido = DateTime.Now
            };
        }
        #endregion

        public decimal CalcularLucroDoDia(List<Pedido> pedidos, DateTime data)
        {
            // Filtrar os pedidos pela data especificada
            var pedidosDoDia = pedidos.Where(p => p.DataPedido.Date == data.Date);

            // Calcular o lucro somando o valor dos pratos de cada pedido
            decimal lucroTotal = 0;
            foreach (var pedido in pedidosDoDia)
            {
                lucroTotal += pedido.PratosSelecionados.Sum(prato => prato.Preco);
            }

            return lucroTotal;
        }

    }
}
