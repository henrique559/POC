using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Windows.Input;
using Control;
using Microsoft.Identity.Client;
using System.Data;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;
using Entity;
using System.Numerics;
using System.Linq.Expressions;


namespace Entity
{
    /*namespace para declaração de entidades*/
    class Usuario
    {
        private static int usuarioID;
        private string Nome;
        private int CPF;
        private string Email;
        private string Senha;
        private string telefoneUsuario;
        private Endereco enderecoUsuario;
        private Pagamento metodoPagamento;

        public Usuario(string nome, int cPF, string email, string senha, string telefoneUsuario, Endereco enderecoUsuario)
        {
            Usuario.usuarioID++;
            Nome = nome;
            CPF = cPF;
            Email = email;
            Senha = senha;
            this.telefoneUsuario = telefoneUsuario;
            this.enderecoUsuario = enderecoUsuario;
        }

        public string getEndereco
        { get => this.enderecoUsuario.EnderecoCompleto; }

        public string getNome { get => this.Nome; }
        public string getEmail { get => this.Email;  }
        public string getSenha { get => this.Senha;  }
        public string getTelefoneUsuario { get => this.telefoneUsuario; }



        public Pagamento getMetodoPagamento
        { get => this.metodoPagamento; }

        public int getCPF
        { get => this.CPF; }
    }
    abstract class Pagamento
    {
        public abstract bool efetuarPagamento(Venda compra);
    }
    class cartaoCredito : Pagamento
    {
        private string numeroCartao;
        private string nomeCartao;
        private int codigoSeguranca;

        public cartaoCredito(string numeroCartao, string nomeCartao, int codigoSeguranca)
        {
            this.numeroCartao = numeroCartao;
            this.nomeCartao = nomeCartao;
            this.codigoSeguranca = codigoSeguranca;
        }
        public override bool efetuarPagamento(Venda compra)
        {
            return true;
        }

    }
    class PIX : Pagamento
    {
        private string? chavePIX;
        public PIX(){}

        public override bool efetuarPagamento(Venda compra)
        {
            return true;
        }
    }
    class Endereco
    {
        private string usuarioEndereco;
        private string CEP;

        public Endereco(string usuarioEndereco, string CEP)
        {
            this.usuarioEndereco = usuarioEndereco;
            this.CEP = CEP;
        }
        public string getCEP
        {
            get => this.CEP;
            set => this.CEP = value;
        }

        public string EnderecoCompleto { get => $"{usuarioEndereco} - {CEP}"; }

    }
    class Produto
    {
        private string nomeProduto;
        private string descricaoProduto;
        private double precoUnitario;
        private int qntdDisponivel;
        private Categoria categoriaProduto;

        public Produto(string nomeProduto, double precoUnitario, string descricaoProduto, int qntdDisponivel, Categoria categoriaProduto )
        {
            this.nomeProduto = nomeProduto;
            this.descricaoProduto = descricaoProduto;
            this.precoUnitario = precoUnitario;
            this.qntdDisponivel = qntdDisponivel;
            this.categoriaProduto = categoriaProduto;
        }

        public string getNomeProduto { get => this.nomeProduto; set => this.nomeProduto = value; }
        public double getProdutoPreco { get => this.precoUnitario; set => this.getProdutoPreco = value; }
        public int getQuantidadeProduto { get => this.qntdDisponivel; set => this.getProdutoPreco = value; }
        public string getDescricaoProduto { get => this.descricaoProduto; set => this.descricaoProduto = value; }

        public string getCategoriaProduto { get => categoriaProduto.getTipoProduto; }
    }

    class Categoria
    {
        private string tipoProduto;
        private string Lote;

        public Categoria(string tipoProduto, string Lote)
        {
            this.tipoProduto = tipoProduto;
            this.Lote = Lote;
        }
        public int getIdCategoria { get; set; }
        public string getTipoProduto { get; set; }
        public string getLote { get; set;  }
        
    }

    class Estoque
    {
        private string dataValidade;
        private int quantidadeDisponivel;
        private Produto itemProduto;

        public Estoque(string dataValidade, int quantidadeDisponivel, Produto itemProduto)
        {
            this.dataValidade = dataValidade;
            this.quantidadeDisponivel = quantidadeDisponivel;
            this.itemProduto = itemProduto;
        }

        public string getDataValidade { get; set; }
        public string getQuantidadeDisponivel { get; set; }
        public Produto getItemProduto { get; set; }

    }

    class produtoVenda
    {
        private Produto produtoCompra;
        private int qntdTotal;
        private Usuario Cliente;
        private double precoTotal;

        public produtoVenda(Produto produtoCompra, int qntdTotal, Usuario cliente, double precoTotal)
        {
            this.produtoCompra = produtoCompra;
            this.qntdTotal = qntdTotal;
            Cliente = cliente;
            this.precoTotal = precoTotal;
        }

        public Produto getProduto { get => this.produtoCompra; }

        public override string ToString()
        {
            return produtoCompra.getNomeProduto;
        }

        public int getQuantidade { get => this.qntdTotal; }
        public double getPreco {  get => this.precoTotal; }
    }

    class Venda
    {
        private List<produtoVenda>? produto;
        private double precoTotal;
        private double precoFrete;
        private Pagamento? tipoPagamento;
        private Endereco? entregarEndereco;
        private Recibo recibo;
        private string? dataVenda;

        public Venda(List<produtoVenda> produto, double precoTotal,
            double precoFrete, Pagamento tipoPagamento, Endereco entregarEndereco, string dataVenda)
        {
            this.produto = produto;
            this.precoTotal = precoTotal;
            this.precoFrete = precoFrete;
            this.tipoPagamento = tipoPagamento;
            this.entregarEndereco = entregarEndereco;
            this.dataVenda = dataVenda;
        }

        public List<produtoVenda>? getProdutosVenda
        {
            get => produto; 
        }
        
        public double getPrecoTotal {  get => this.precoTotal; }
        public double getPrecoFrete { get => this.precoFrete; }


        // TODO: Fazer a geração de recibo
        public virtual Recibo gerarRecibo(Venda venda) { return recibo; }

    }
       

    abstract class Recibo
    {

    }

    class  Administrador
    {
        private static bool estaLogado = false;
        private static string ID = "F350614";
        private static string Senha = "@111Fazenda_Urbana111@";


        public Administrador()
        { }

        public static string getID { get => ID; }
        public static bool getAdmLogin { get => estaLogado; set => estaLogado = value; }

        public static string getADMPassword { get => Senha;  }
        
    }

}

namespace Boundary
{
    class myMain
    {
        public static void Main(string[] args)
        {
            menuPrincipal main = new menuPrincipal();

            main.menu();
        }
    }



    class menuPrincipal
    {
        public void titulo()
        {
            Console.WriteLine("\n .---. .----. .-..-.   .-..----. .-.\r\n/   __}| {}  }| ||  `.'  || {}  }| |\r\n\\  {_ }| .-. \\| || |\\ /| || .--' | |\r\n `---' `-' `-'`-'`-' ` `-'`-'    `-'\n\n");
        }
        public virtual void menu()
        {

            while (true)
            {
                Console.Clear();
                if (loginStatus.checkLoginStatus)
                {
                    titulo();
                    int opt;
                    Console.WriteLine("Escolha as opções:\n1 - Minha conta\n2 - Produtos\n3 - Carrinho de Compras\n4 - Sair");
                    try
                    {
                        opt = Convert.ToInt32(Console.ReadLine());

                    }
                    catch (Exception a)
                    {
                        continue;
                    }
                    switch (opt)
                    {

                        case 1:
                            GerenciarConta();
                            break;

                        case 2:
                            break;

                        case 3:
                            break;

                        case 4:
                            Console.WriteLine("\nVocê saiu da sua conta");
                            Console.ReadKey(true);
                            loginStatus.checkLoginStatus = false;
                            break;

                        default:
                            break;
                    }
                }
                else if (Administrador.getAdmLogin)
                {
                    int opt;
                    Console.Clear();
                    titulo();
                    Console.WriteLine("Escolha as opções:\n1 - Visualizar Clientes\n2 - Inserir Produtos\n3 - Editar Contas\n4 - Deletar Contas\n5 - Sair");
                    try
                    {
                        opt = Convert.ToInt32(Console.ReadLine());

                    }
                    catch (Exception a)
                    {
                        continue;
                    }
                    switch (opt)
                    {
                        case 1:
                            visualizarDadosADM();
                            break;

                        case 2:
                            telaInserirProduto();
                            break;

                        case 3:
                            break;


                        case 4:
                            delClientes();
                            break;

                        case 5:
                            Administrador.getAdmLogin = false;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    titulo();
                    Console.WriteLine("Seja bem-vindo ao aplicativo da Grimpi");
                    int opt;
                    Console.WriteLine("Escolha as opções:\n1 - Login\n2 - Registro\n3 - Produtos\n4 - Sair");
                    try
                    {
                        opt = Convert.ToInt32(Console.ReadLine());

                    }
                    catch (Exception a)
                    {
                        continue;
                    }
                    switch (opt)
                    {
                        case 1:
                            fazerLogin();
                            break;

                        case 2:
                            fazerCadastro();
                            break;

                        case 3:
                            break;

                        case 4:

                            Console.WriteLine("Volte sempre!");
                            Console.ReadKey(true);
                            return;

                        default:
                            break;
                    }

                }
                Console.WriteLine($"\nlogin status: " + Convert.ToString(loginStatus.checkLoginStatus == true ? "logado" : "deslogado"));


            }

        }
        public void telaInserirProduto()
        {
            InserirProdutos produtos = new InserirProdutos();
            Console.Clear();

            produtos.getProduto();
        }

        public void fazerCadastro()
        {
            telaRegistro registro = new telaRegistro();
            registro.fazerCadastro();
        }

        public void fazerLogin()
        {
            telaLogin login = new telaLogin();
            login.fazerLogin();
        }

        public void GerenciarConta()
        {

            Console.Clear();

            while (true)
            {
                int opt;
                Console.Clear();
                Console.WriteLine("\tMinha conta\n");

                Console.WriteLine("1 - Visualizar Dados\n2 - Editar dados\n3 - Meus Pedidos\n");
                try
                {
                    opt = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception a)
                {
                    continue;
                }

                switch (opt)
                {
                    case 1:
                        visualizarDadosCliente();
                        break;

                    case 2:
                        editarDados();
                        break;
                    default:
                        return;
                }
            }
        }

        public virtual void editarDados()
        {
            ModificarDados modificarDados = new ModificarDados();
            modificarDados.modificarCliente(loginStatus.getEmail);
        }
        public void visualizarDadosCliente()
        {
            telaClienteDados telaDados = new telaClienteDados("Email", loginStatus.getEmail);
            telaDados.mostrarDados();
        }

        public void visualizarDadosADM()
        {
            Console.Clear();
            Console.WriteLine("\tDados dos Clientes\n");
            telaClienteDados telaDados = new telaClienteDados("Email", loginStatus.getEmail);
            telaDados.mostrarVariosDados();
        }

        public void delClientes()
        {
            Console.Clear();
            Console.WriteLine("Digite o CPF do cliente que deseja deletar: ");
            int cpf;
            try
            {
                cpf = Convert.ToInt32(Console.ReadLine());
            }
            catch(Exception a)
            { Console.WriteLine(a.Message); return;  }
            telaDeletarCliente telaDel = new telaDeletarCliente(cpf);

            telaDel.deletarCliente();
        }



    }


    class telaLogin
    {
        public bool fazerLogin()
        {
            Console.Clear();
            try
            {

                while (true)
                {

                    string? email = realizarLogin.getInput("Insira seu email: ");
                    string? senha = realizarLogin.getInput("\nInsira sua senha: ");


                    realizarLogin login = new realizarLogin(email, senha);

                    if (string.Equals(email, Administrador.getID))
                    {
                        if (login.realizarADMLogin(email, senha))
                        {
                            Administrador.getAdmLogin = true;
                        }
                    }

                    else if (login.checarLogin(email, senha))
                    {
                        Console.WriteLine("Login realizado com sucesso");
                        loginStatus.checkLoginStatus = true;
                        loginStatus.getEmail = email;

                    }
                    break;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey(true);
            }

            return false;

        }
    }

    class telaRegistro
    {
        public void fazerCadastro()
        {
            Console.Clear();

            realizarCadastro cadastro = new realizarCadastro();
            try
            {
                while (true)
                {
                    // Pegar input dos usuários 
                    string? nome = realizarCadastro.getInput("Insira seu nome completo: ");
                    string? email = realizarCadastro.getInput("\nInsira seu email: ");
                    int cpf = realizarCadastro.getInputCPF("\nInsira seu CPF: ");
                    string? telefoneUsuario = realizarCadastro.getInput("\nInsira seu telefone: ");
                    string? endereco = realizarCadastro.getInput("\nInsira seu endereco: ");

                    string? cep = realizarCadastro.getInput("\nInsira seu CEP: ");

                    while (true)
                    {
                        string? senha1 = realizarCadastro.getInput("\nInsira sua senha: ");
                        string? senha2 = realizarCadastro.getInput("\nInsira sua senha novamente: ");

                        if (string.Equals(senha1, senha2))
                        {

                            Entity.Endereco usuarioEndereco = new Entity.Endereco(endereco, cep);

                            //Criação do objeto usuario
                            Entity.Usuario Cliente = cadastro.efetuarCadastro(new Entity.Usuario(nome, cpf, email, senha1, telefoneUsuario, usuarioEndereco));


                            return;
                        }
                    }
                }
            }
            catch (Exception a)
            {
                Console.WriteLine(a);
                Console.ReadKey(true);
                return;
            }

        }
    }

    class telaClienteDados
    {
        string tipoConsulta;
        string pesquisa;

        public telaClienteDados(string tipoConsulta, string pesquisa)
        {
            this.tipoConsulta = tipoConsulta;
            this.pesquisa = pesquisa;
        }
        public void mostrarVariosDados()
        {
            VisualizarClientes visualizarCliente = new VisualizarClientes(tipoConsulta, pesquisa);
            visualizarCliente.PesquisarClientes();

        }

        public virtual void mostrarDados() 
        {
            VisualizarClientes visualizarCliente = new VisualizarClientes(tipoConsulta, pesquisa);
            visualizarCliente.pesquisarClienteEspecifico();
        }
    }

    class telaDeletarCliente
    {
        int CPF; 

        public telaDeletarCliente(int cPF)
        {
            CPF = cPF;
        }

        public void deletarCliente()
        {
            deletarDados del = new deletarDados();

            del.deletarCliente(CPF);
        }
    }
}

namespace Control
{
    static class loginStatus
    {
        static private bool estaLogado = false;
        static private string Email;

        public static bool checkLoginStatus { get => estaLogado; set => estaLogado = value; }
        public static string getEmail { get => Email; set => Email = value; }

    }
    class realizarLogin
    {
        private string email;
        private string password;

        public realizarLogin(string email, string password) 
        {
            this.email = email;
            this.password = password;
        }

        public bool checarLogin(string email, string password)
        {
  
            string consulta = "SELECT COUNT(*) FROM Cliente WHERE Email = @email AND Senha = @password";
            using (SqlConnection Connection = new SqlConnection(DatabaseConnection.getConnection))
            {
                // Criando o Comando e abrindo conexão
                try
                {
                    using (SqlCommand command = new SqlCommand(consulta, Connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        Connection.Open();

                        int resultado = (int) command.ExecuteScalar();
                        Console.WriteLine(Convert.ToString(resultado > 0 ? "Login realizado com sucesso!" : "Email ou senha incorretos."));
                        Console.ReadKey(true);


                        return resultado > 0;
                    }

                }
                catch(Exception a)
                {
                    Console.WriteLine(a);
                    Console.ReadKey(true);
                }
                Connection.Close();
                return false;


            }
        }

        public static string getInput(string prompt)
        {
            // Lida com input do usuário, evitando erros
            string? returnValue;
            while (true)
            {


                Console.WriteLine(prompt);
                try
                {
                    returnValue = Console.ReadLine();

                    if (string.IsNullOrEmpty(returnValue)) throw new Exception();
                }
                catch (Exception a)
                {
                    Console.WriteLine("Tente novamente.");
                    continue;
                }
                return returnValue;

            }

        }

        public bool realizarADMLogin(string ID, string password)
        {
            if (string.Equals(ID, Administrador.getID) && string.Equals(password, Administrador.getADMPassword))
            {
                return true;
            }

            return false;
        }

    }

    /*Classe para manter conexão com o banco de dados e montar a string de conexão*/
    static class DatabaseConnection
    {
        private static string server = @"DESKTOP-3A36BBQ";
        private static string database = "FazendaUrbana";
        private static string user = "PIM2024";
        private static string password = "fazendaurbana123";

        public static string getConnection { get => @"Data Source= " + server + ";Initial Catalog=" + database + ";Integrated Security= true;TrustServerCertificate=true "; }
    }

    /* Classe para realizar o cadastro do cliente */
    class realizarCadastro
    {
        private Entity.Usuario? Cliente;
        private Entity.Endereco? clienteEndereço;

        public realizarCadastro() { }

        public Entity.Usuario efetuarCadastro(Entity.Usuario? user)
        {
            /*   
             Essa parte do código irá pegar o objeto de algum usuário e colocar no banco de Dados
            */

            // Código para realizar a query no banco de dados
            string? consulta = "INSERT INTO Cliente (Nome, CPF, Email, Senha, Endereco, Telefone) VALUES (@nome, @cpf, @email,@senha,@endereco, @telefone);";
            
            try
            {
                using (SqlConnection Connection = new SqlConnection(DatabaseConnection.getConnection))
                {
                    // Inserindo dados do usuário na tabela
                    Connection.Open();

                    SqlCommand cmd = new SqlCommand(consulta, Connection);
                    cmd.Parameters.AddWithValue("@nome", user.getNome);
                    cmd.Parameters.AddWithValue("@cpf", user.getCPF);
                    cmd.Parameters.AddWithValue("@senha", user.getSenha);
                    cmd.Parameters.AddWithValue("@email", user.getEmail);
                    cmd.Parameters.AddWithValue("@endereco", user.getEndereco);
                    cmd.Parameters.AddWithValue("@telefone ", user.getTelefoneUsuario);


                    cmd.ExecuteNonQuery();
                    Connection.Close();

                }
            }
            catch(SqlException erro)
            {
                if(erro.ErrorCode == -2146232060)
                {
                    Console.WriteLine("Erro: esse CPF já foi utilizado.\nPressione [ENTER] para sair\n");
                    Console.ReadKey(true);
                }
                Console.WriteLine(erro.ErrorCode);
            }
            catch(Exception a)
            {
                Console.WriteLine("efetuarCadastro(): Um erro ocorreu. 404");
            }
  

            return user;
        }

        // Classe para lidar com input do usuário.
        public static string getInput(string prompt)
        {
            string? returnValue;
            while (true)
            {
                Console.WriteLine(prompt);
                try
                {
                    returnValue = Console.ReadLine();
                    if (string.IsNullOrEmpty(returnValue) || returnValue.Length < 5) throw new Exception();
                }
                catch (Exception a)
                {
                    Console.WriteLine("Tente novamente.");
                    continue;
                }
                return returnValue;
            }

        }

        public static int getInputCPF(string prompt)
        {
            int cpf;
            while (true)
            {
                Console.WriteLine(prompt);
                try
                {
                    cpf = Convert.ToInt32(Console.ReadLine());
                    return cpf;

                }
                catch (Exception a)
                {
                    Console.WriteLine("Tente novamente.");
                    continue;
                }
            }
        }
    }

    class VisualizarClientes
    {
        string tipoConsulta;
        string pesquisa;

        public VisualizarClientes(string pesquisa, string tipoConsulta)
        { 
            this.pesquisa = pesquisa; 
            this.tipoConsulta = tipoConsulta; 
        }
        public void pesquisarClienteEspecifico()
        {
            string consulta = "SELECT * FROM Cliente WHERE Email = @pesquisa;";
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(consulta, connection);

                    Console.Clear();
                    cmd.Parameters.AddWithValue("@pesquisa", loginStatus.getEmail);
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\tMeus dados\n");
                            reader.Read();
                            Console.WriteLine($"Nome:\t{reader["Nome"]}\nCPF:\t{reader["CPF"]}\nEmail:\t{reader["Email"]}\nSenha:\t{reader["Senha"]}\nEndereco:\t{reader["Endereco"]}\nTelefone:\t{reader["Telefone"]}\n");




                            Console.ReadKey(true);
                            Console.Clear();
                            return;
                        }
                    }
                    catch(SqlException a) { Console.WriteLine(a.Message); }


                }
            }
            catch(SqlException a )
            {
                Console.WriteLine(a.Message);
                Console.ReadKey(true);
            }


        }

        public void PesquisarClientes()
        {
            string consulta = "SELECT * FROM Cliente;";
            using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(consulta, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    while (reader.Read())
                    {


                        Console.WriteLine($"Nome:\t{reader["Nome"]}\nCPF:\t{reader["CPF"]}\nEmail:\t{reader["Email"]}\nSenha:\t{reader["Senha"]}\nEndereco\t:{reader["Endereco"]}\nTelefone:\t{reader["Telefone"]}\n");
                    }


                    Console.ReadKey(true);
                    connection.Close();
                }

            }
        }
    }

    class deletarDados
    {
        public void deletarCliente(int CPF)
        {
            string consulta = "DELETE FROM Cliente WHERE CPF = @CPF";

            using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
            {
                connection.Open();
                using(SqlCommand cmd = new SqlCommand(consulta, connection))
                {
                    cmd.Parameters.AddWithValue("@CPF", CPF);
                    int resultado = cmd.ExecuteNonQuery();

                    Console.WriteLine($"{Convert.ToString(resultado > 0 ? "Cliente removido com sucesso" : "Nenhum cliente com esse CPF foi encontrado")}");
                }
                Console.ReadKey(true);
            }
        }
    }
    class InserirProdutos
    {
        int categoriaId;
        string consulta = "INSERT INTO Produto (nomeProduto, Descricao, tipoProduto, precoProduto) VALUES (@nomeProduto, @Descricao, @tipoProduto, @precoProduto);";
        string consulta2 = "SELECT _idProduto FROM Categoria WHERE tipoProduto LIKE @tipoProduto;";

        public InserirProdutos() { }

        public void InserirUmProduto(Entity.Produto produto, Entity.Categoria categoria)
        {
            produto = getProduto();
            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(consulta2, connection))
                    {
                        cmd.Parameters.AddWithValue("@tipoProduto", categoria.getTipoProduto);
                        using(SqlDataReader reader = cmd.ExecuteReader()) 
                        {
                            categoriaId = Convert.ToInt32(reader["_idProduto"]);
                        }
                    }

                    using (SqlCommand cmd2 = new SqlCommand(consulta, connection))
                    {
                        cmd2.Parameters.AddWithValue("@nomeProduto", produto.getNomeProduto);
                        cmd2.Parameters.AddWithValue("@Descricao", produto.getDescricaoProduto);
                        cmd2.Parameters.AddWithValue("@tipoProduto", categoriaId);
                        cmd2.Parameters.AddWithValue("@precoProduto", produto.getProdutoPreco);

                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException a)
            { 
                Console.WriteLine($"InserirProduto(): {a.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"InserirProduto(): {ex.Message}");
            }

        }

        public Entity.Estoque getEstoque(Entity.Produto produto)
        {
            int id;
            string consulta1 = "SELECT _idProduto FROM Produto WHERE nomeProduto LIKE @produto;";
            string consulta2 = "INSERT INTO Estoque (_idProduto, dataValidade, quantidadeDisponivel, itemProduto) VALUES ('@_idProduto', '@dataValidade', '@quantidadeDisponivel', '@itemProduto');";

            Console.WriteLine($"Produto: { produto.getNomeProduto } ");
            Console.WriteLine($"Quantidade do Produto: {produto.getQuantidadeProduto} ");
 

            Console.WriteLine("$Digite aqui a data de validade do produto: ");
            string dataValidade = getProdutoString();

            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(consulta1, connection))
                    {
                        cmd.Parameters.AddWithValue("@produto", produto.getNomeProduto);
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            id = Convert.ToInt32(reader["_idProduto"]);
                        }
                    }

                    using (SqlCommand cmd2 = new SqlCommand(consulta2, connection))
                    {
                        cmd2.Parameters.AddWithValue("@_idProduto", id);
                        cmd2.Parameters.AddWithValue("@dataValidade", dataValidade);
                        cmd2.Parameters.AddWithValue("@quantidadeDisponivel", produto.getQuantidadeProduto);
                        cmd2.Parameters.AddWithValue("@itemProduto", produto.getNomeProduto);

                        cmd2.ExecuteNonQuery();

                    }
                    connection.Close();

                }
            }
            catch(SqlException a)
            {
                Console.WriteLine($"getEstoque(): {a.Message}");
                Console.ReadKey(true);

            }
            catch(Exception a)
            {
                Console.Write($"getEstoque(): {a.Message}");
                Console.ReadKey(true);
            }

            return new Entity.Estoque(dataValidade, produto.getQuantidadeProduto, produto);



        }
        public Entity.Categoria getCategoria()
        {
            // Ervas Aromaticas
            string consulta = "INSERT INTO Categoria(tipoProduto, Lote) VALUES (@tipoProduto, @Lote)";

            Console.WriteLine("Insira o tipo de produto: ");
            string tipoProduto = getProdutoString();

            Console.WriteLine("\nInsira o Lote do produto: ");
            string Lote = getProdutoString();

            try
            {
                using (SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(consulta, connection))
                    {
                        command.Parameters.AddWithValue("@tipoProduto", tipoProduto);
                        command.Parameters.AddWithValue("@Lote", Lote);
                        command.ExecuteNonQuery();

                    }

                }
            }
            catch (SqlException a)
            {
                Console.WriteLine($"getCategoria(): {a.Message}");
                Console.ReadKey(true);
                return null;
            }
            catch(Exception a)
            {
                Console.WriteLine($"getCategoria(): {a.Message}");
                Console.ReadKey(true);
                return null;
            }
            return new (tipoProduto, Lote);


        }


        public Entity.Produto getProduto()
        {
            Entity.Produto produto;


            Console.WriteLine("Insira o nome do produto: ");
            string nome = getProdutoString();

            Console.WriteLine("\nInsira o preço do produto: ");
            double preco = getPreco();
            
            Console.WriteLine("\nInsira a descrição do produto: ");
            string descricao = getProdutoString();

            Console.WriteLine("\nInsira aqui a quantidade do produto");
            int quantidade = getQuantidade();

            Console.WriteLine("\n\tTipo de produto\n");

            Entity.Categoria categoria;
            categoria = getCategoria();
            try
            {
                if (categoria == null) throw new Exception();
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"getProduto(): {ex.Message}");
                return null;
            
            }

            produto = new Entity.Produto(nome, preco, descricao, quantidade, categoria);

            Console.WriteLine("\nEstoque\n");
            Entity.Estoque estoque;
            estoque = getEstoque(produto);
            try
            {
                if (estoque == null) throw new Exception();
            }
            catch(Exception a)
            {
                Console.WriteLine($"getProduto(): {a.Message}");
                return null;
            }

            return produto;


        }
        public int getQuantidade()
        {
            int quantidadeProduto;
            while (true)
            {
                try
                {
                    quantidadeProduto = Convert.ToInt32(Console.ReadLine());
                    return quantidadeProduto;

                }
                catch (Exception a)
                {
                    Console.WriteLine($"Tente novamente");
                    Console.ReadKey(true);

                    continue;
                }
            }

        }
        public double getPreco()
        {
            double returnValue;
            while(true)
            {
                try
                {
                    returnValue = Convert.ToDouble(Console.ReadLine());

                    return returnValue;
                }
                catch(Exception a)
                {
                    Console.WriteLine($"Tente novamente ");
                    Console.ReadKey(true);

                    continue;

                }
            }
        }
        public string getProdutoString()
        {
            string? returnValue;
            while (true)
            {
                try
                {
                    returnValue = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(returnValue)) throw new Exception();

                    return returnValue;
                }
                catch (Exception a)
                {
                    Console.WriteLine($"Tente novamente ");
                    Console.ReadKey(true);

                    continue;

                }
            }
        }
    }

    class ModificarDados
    {


        public void modificarCliente(string cliente)
        {
            InserirProdutos input = new InserirProdutos();
            string consulta = "UPDATE Cliente SET @coluna = @valor WHERE Email = @cliente";

            string[] colunas = new string[] { "Nome", "CPF", "Email", "Senha", "Endereco", "Telefone" };

            Console.Clear();
            Console.WriteLine("\tEditarDados\n");
            using(SqlConnection connection = new SqlConnection(DatabaseConnection.getConnection))
            {
                connection.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(consulta, connection))
                    {

                        for (int i = 0; i < colunas.Length; i++)
                        {
                            Console.WriteLine($"Insira o seu {colunas[i]}");
                            if (string.Equals(colunas[i], "CPF"))
                            {
                                int CPF = input.getQuantidade();
                                cmd.Parameters.AddWithValue("@cliente", cliente);
                                cmd.Parameters.AddWithValue("@valor", CPF);
                                cmd.Parameters.AddWithValue("@coluna", colunas[i]);

                                cmd.ExecuteNonQuery();
                            }
                            string valores = input.getProdutoString();

                            cmd.Parameters.AddWithValue("@valor", valores);
                            cmd.Parameters.AddWithValue("@cliente", cliente);
                            cmd.Parameters.AddWithValue("@coluna", colunas[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                catch(SqlException e) { Console.WriteLine(e.Message); Console.ReadKey(true) ; return; }
     
            }

     
        }

        public void modificarClienteEspecifico(string cliente)
        {

        }
    }

}