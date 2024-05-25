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
        private double precoUnitario;
        private int qntdDisponivel;

        public Produto(string nomeProduto, double precoUnitario, int qntdDisponivel)
        {
            this.nomeProduto = nomeProduto;
            this.precoUnitario = precoUnitario;
            this.qntdDisponivel = qntdDisponivel;
        }

        public string getNomeProduto { get => this.nomeProduto; }
        public double getProdutoPreco {  get => this.precoUnitario; }
        public int getQuantidadeProduto { get => this.qntdDisponivel;  }
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
    
        public virtual void menu()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\tFazenda Urbana\n\n");
                if(loginStatus.checkLoginStatus)
                {
                    Console.WriteLine("1 - Login\n2 - Registro\n3 - Minha conta\n4 - Produtos\n5 - Carrinho de Compras\n6 - Sair");
                  
                }
                else
                {
                    Console.WriteLine("1 - Login\n2 - Registro\n3 - Produtos\n4 - Sair");

                }
                Console.WriteLine($"\nlogin status: " + Convert.ToString(loginStatus.checkLoginStatus == true ? "logado" : "deslogado"));
                int opt = Convert.ToInt32(Console.ReadLine());

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

                    case 5:
                        return;

                    default:
                        break;
                }
            }

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
                    string? senha = realizarLogin.getInput("Insira sua senha: ");


                    realizarLogin login = new realizarLogin(email, senha);
                       
                    if (login.checarLogin(email, senha))
                    {
                        Console.WriteLine("Login realizado com sucesso");
                        loginStatus.checkLoginStatus = true;
                        
                    }
                    break;
                }
             
            }
            catch(Exception e)
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
            catch(Exception a)
            {
                Console.WriteLine(a);
                Console.ReadKey(true);
                return;
            }
            
        }
    }
}

namespace Control
{
    static class loginStatus
    {
        static private bool estaLogado = false;
        public static bool checkLoginStatus { get => estaLogado; set => estaLogado = value; }

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
                        Console.WriteLine(resultado);

                        Console.ReadKey(true);

                        return resultado > 0 ? true : false;
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

}
