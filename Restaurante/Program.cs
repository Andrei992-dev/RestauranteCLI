using Restaurante.Models;
using Restaurante.Services;

const string linha = "------------------------------------------------------------------------";
Console.WriteLine(linha);
Console.WriteLine("Gerenciador de Restaurante");
Console.WriteLine(linha);

RestauranteService restauranteService = new RestauranteService();

List<Garcon> garcons = restauranteService.CarregarGarcons();
List<Prato> pratos = restauranteService.CarregarPratos();
List<Pedido> pedidos = restauranteService.CarregarPedidos();

bool continuar = true;

while (continuar)
{
    // Exibir o menu
    Console.WriteLine("==== MENU ====");
    Console.WriteLine("1. Criar Garçom");
    Console.WriteLine("2. Listar Garçons");
    Console.WriteLine("3. Criar Prato");
    Console.WriteLine("4. Listar Pratos");
    Console.WriteLine("5. Criar Pedido");
    Console.WriteLine("6. Listar Pedidos");
    Console.WriteLine("7. Calcular Lucro do Dia");
    Console.WriteLine("8. Finalizar Aplicação");
    Console.WriteLine("Escolha uma opção:");

    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            Garcon novoGarcon = restauranteService.CriarGarconViaConsole(garcons);
            garcons.Add(novoGarcon);
            await restauranteService.SalvarGarconAsync(garcons);
            Console.WriteLine($"Garçom {novoGarcon.Nome} adicionado e salvo.");
            break;

        case "2":
            restauranteService.ListarGarcons(garcons);
            break;

        case "3":
            Prato novoPrato = restauranteService.CriarPratoViaConsole();
            pratos.Add(novoPrato);
            await restauranteService.SalvarPratoAsync(pratos);
            Console.WriteLine($"Prato {novoPrato.Nome} adicionado e salvo.");
            break;

        case "4":
            restauranteService.ListarPratos(pratos);
            break;

        case "5":
            Pedido novoPedido = restauranteService.CriarPedidoViaConsole(garcons, pratos);
            pedidos.Add(novoPedido);
            await restauranteService.SalvarPedidoAsync(pedidos);
            Console.WriteLine($"Pedido {novoPedido.Id} para cliente {novoPedido.ClienteNome} salvo.");
            break;

        case "6":
            if (pedidos.Count == 0)
            {
                Console.WriteLine("Nenhum pedido registrado.");
            }
            else
            {
                Console.WriteLine("==== Lista de Pedidos ====");
                foreach (var pedido in pedidos)
                {
                    Console.WriteLine($"Pedido ID: {pedido.Id}, Cliente: {pedido.ClienteNome}, Garçom ID: {pedido.GarconId}");
                    Console.WriteLine("Pratos Selecionados:");
                    foreach (var prato in pedido.PratosSelecionados)
                    {
                        Console.WriteLine($"- {prato.Nome}, Preço: {prato.Preco:C}");
                    }
                    Console.WriteLine("==========================");
                }
            }
            break;

        case "7":
            decimal lucroDoDia = restauranteService.CalcularLucroDoDia(pedidos, DateTime.Now);
            Console.WriteLine($"Lucro total dos pedidos do dia: {lucroDoDia:C}");
            break;

        case "8":
            continuar = false;
            Console.WriteLine("Finalizando a aplicação...");
            break;

        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }

    Console.WriteLine();
}


